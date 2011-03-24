// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using EFPocoAdapter.DataClasses;

namespace EFPocoAdapter
{
    internal class EFPocoQueryProvider : IQueryProvider
    {
        private EFPocoContext _context;

        public EFPocoQueryProvider(EFPocoContext context)
        {
            _context = context;
        }

        #region IQueryProvider Members

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new EFPocoQuery<TElement>(_context, expression);
        }

        public IQueryable CreateQuery(Expression expression)
        {
            Type elementType = TypeSystem.GetElementType(expression.Type);
            try
            {
                return (IQueryable)Activator.CreateInstance(typeof(EFPocoQuery<>).MakeGenericType(elementType), new object[] { _context, expression });
            }
            catch (TargetInvocationException tie)
            {
                throw tie.InnerException;
            }
        }

        public TResult Execute<TResult>(Expression expression)
        {
            Func<EFPocoContext, object, TResult> adapterFunction;
            Expression expr = _context.CreateContextBoundQueryTranslator().Translate(expression, out adapterFunction);
            using (ThreadLocalContext.Set(_context))
            {
                return adapterFunction(_context, _context.EntityFrameworkQueryProvider.Execute(expr));
            }
        }

        public object Execute(Expression expression)
        {
            var mi = this.GetType().GetMember("Execute", MemberTypes.Method, BindingFlags.Public | BindingFlags.Instance)
                .Cast<MethodInfo>()
                .Where(c => c.IsGenericMethodDefinition)
                .Single()
                .MakeGenericMethod(expression.Type);

            return mi.Invoke(this, new object[] { expression });
        }

        #endregion

        // from Matt Warren's blog
        internal static class TypeSystem
        {
            internal static Type GetElementType(Type seqType)
            {
                Type ienum = FindIEnumerable(seqType);
                if (ienum == null) return seqType;
                return ienum.GetGenericArguments()[0];
            }

            private static Type FindIEnumerable(Type seqType)
            {
                if (seqType == null || seqType == typeof(string))
                    return null;
                if (seqType.IsArray)
                    return typeof(IEnumerable<>).MakeGenericType(seqType.GetElementType());
                if (seqType.IsGenericType)
                {
                    foreach (Type arg in seqType.GetGenericArguments())
                    {
                        Type ienum = typeof(IEnumerable<>).MakeGenericType(arg);
                        if (ienum.IsAssignableFrom(seqType))
                        {
                            return ienum;
                        }
                    }
                }
                Type[] ifaces = seqType.GetInterfaces();
                if (ifaces != null && ifaces.Length > 0)
                {
                    foreach (Type iface in ifaces)
                    {
                        Type ienum = FindIEnumerable(iface);
                        if (ienum != null) return ienum;
                    }
                }
                if (seqType.BaseType != null && seqType.BaseType != typeof(object))
                {
                    return FindIEnumerable(seqType.BaseType);
                }
                return null;
            }
        }
    }
}
