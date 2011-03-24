// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using EFPocoAdapter.DataClasses;

namespace EFPocoAdapter.Internal
{
    internal class QueryTranslator : ExpressionVisitor
    {
        private Type _contextType;
        private Type _persistenceAwareContextType;
        private EFPocoContext _currentContext;
        private QueryTranslationCache _sharedCache;
        private QueryTranslationCache _privateCache;

        private Dictionary<ParameterExpression, ParameterExpression> _parameterExpressionTranslationCache = new Dictionary<ParameterExpression, ParameterExpression>();

        internal QueryTranslator(Type contextType, Type persistenceAwareContextType, QueryTranslationCache sharedCache, EFPocoContext currentContext)
        {
            _contextType = contextType;
            _persistenceAwareContextType = persistenceAwareContextType;
            _sharedCache = sharedCache;
            _privateCache = new QueryTranslationCache();
            _currentContext = currentContext;
        }

        internal Type PersistenceAwareContextType
        {
            get { return _persistenceAwareContextType; }
        }

        internal ParameterExpression GetTranslatedParameter(ParameterExpression par)
        {
            ParameterExpression result;

            if (_parameterExpressionTranslationCache.TryGetValue(par, out result))
                return result;
            else
                return par;
        }

        internal class EnumeratorConverter<TSourceType, TResultType>
        {
            private Func<TSourceType, TResultType> _adapter;

            public EnumeratorConverter(Func<TSourceType, TResultType> adapter)
            {
                _adapter = adapter;
            }

            public IEnumerable<TResultType> Convert(EFPocoContext context, object source)
            {
                using (ThreadLocalContext.Set(context))
                {
                    foreach (TSourceType s in (IEnumerable<TSourceType>)source)
                    {
                        using (ThreadLocalContext.Set(context))
                        {
                            yield return _adapter(s);
                        }
                    }
                }
            }
        }

        public Expression Translate<T>(Expression pocoExpression, out Func<EFPocoContext, object, T> adapterFunction)
        {
            Expression result = Visit(pocoExpression);

            if (result.Type.IsGenericType && typeof(IQueryable).IsAssignableFrom(result.Type))
            {
                var resultItemType = typeof(T).GetGenericArguments().First();
                var sourceItemType = result.Type.GetGenericArguments().First();
                Type delegateType = typeof(Func<,>).MakeGenericType(sourceItemType,resultItemType);
                var theAdapter = QueryResultsAdapter.GenerateAdapter(delegateType, sourceItemType, resultItemType);
                var enumConverterType = typeof(EnumeratorConverter<,>).MakeGenericType(sourceItemType, resultItemType);
                var enumeratorConverter = Activator.CreateInstance(enumConverterType, theAdapter);
                adapterFunction = (Func<EFPocoContext, object, T>)Delegate.CreateDelegate(typeof(Func<EFPocoContext, object, T>), enumeratorConverter, "Convert");
            }
            else
            {
                adapterFunction = QueryResultsAdapter.GenerateAdapter<T>(result.Type, pocoExpression.Type);
            }

            // no locking here because collections stored in _sharedCache are immutable
            // the merge below does not have to be perfect - if we lose some part of the cache
            // because of a race condition, we'll be able to recreate it next time

            _sharedCache.MergeWith(_privateCache);

            return result;
        }

        protected override Expression VisitConstant(ConstantExpression c)
        {
            IObjectQueryWrapper oqw = c.Value as IObjectQueryWrapper;
            if (oqw != null && oqw.WrappedQuery != null)
            {
                return Expression.Constant(oqw.WrappedQuery);
            }
            return base.VisitConstant(c);
        }

        protected override Expression VisitTypeIs(TypeBinaryExpression b)
        {
            Type newType = TranslateType(b.TypeOperand);
            if (newType != b.TypeOperand)
            {
                return Expression.TypeIs(Visit(b.Expression), newType);
            }
            else
            {
                return base.VisitTypeIs(b);
            }
        }

        protected override Expression VisitUnary(UnaryExpression u)
        {
            Type newType;

            switch (u.NodeType)
            {
                case ExpressionType.Convert:
                    newType = TranslateType(u.Type);
                    if (newType != u.Type)
                        return Expression.Convert(Visit(u.Operand), newType);
                    break;

                case ExpressionType.ConvertChecked:
                    newType = TranslateType(u.Type);
                    if (newType != u.Type)
                        return Expression.ConvertChecked(Visit(u.Operand), newType);
                    break;

                case ExpressionType.TypeAs:
                    newType = TranslateType(u.Type);
                    if (newType != u.Type)
                        return Expression.TypeAs(Visit(u.Operand), newType);
                    break;

            }
            return base.VisitUnary(u);
        }

        protected override Expression VisitMethodCall(MethodCallExpression m)
        {
            MethodBase translatedMethodInfo = TranslateMethodInfo(m.Method);
            if (translatedMethodInfo == m.Method)
            {
                return base.VisitMethodCall(m);
            }
            else
            {
                Expression[] translatedArguments = m.Arguments.Select(c => Visit(c)).ToArray();
                return Expression.Call(Visit(m.Object), (MethodInfo)translatedMethodInfo, translatedArguments);
            }
        }

        protected override Expression VisitLambda(LambdaExpression lambda)
        {
            return Expression.Lambda(TranslateType(lambda.Type), Visit(lambda.Body), 
                lambda.Parameters.Select(c => Visit(c)).Cast<ParameterExpression>().ToArray());
        }

        protected override NewExpression VisitNew(NewExpression nex)
        {
            if (nex.Members != null)
            {
                var translatedMembers = nex.Members.Select(c => TranslateMemberInfo(c));
                return Expression.New(TranslateConstructorInfo(nex.Constructor), VisitExpressionList(nex.Arguments), translatedMembers);
            }
            else
            {
                return Expression.New(TranslateConstructorInfo(nex.Constructor), VisitExpressionList(nex.Arguments));
            }
        }

        private MemberInfo TranslateMemberInfo(MemberInfo mi)
        {
            Type newType = TranslateType(mi.DeclaringType);
            MemberInfo newMember = newType.GetMember(mi.Name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static).First();
            return newMember;
        }

        private ConstructorInfo TranslateConstructorInfo(ConstructorInfo ci)
        {
            return (ConstructorInfo)TranslateMethodInfo(ci);
        }

        protected override Expression VisitMemberAccess(MemberExpression m)
        {
            Type newType = TranslateType(m.Member.DeclaringType);
            MemberInfo newMember = newType.GetMember(m.Member.Name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static).First();
            if (newMember != m.Member)
            {
                Expression newExpression = null;

                if (m.Expression.Type == _contextType && m.Expression.NodeType == ExpressionType.MemberAccess)
                {
                    MemberExpression expr2 = (MemberExpression)m.Expression;
                    if (expr2.Expression.NodeType == ExpressionType.Constant)
                    {
                        var contextFunc = Expression.Lambda<Func<EFPocoContext>>(m.Expression).Compile();
                        var context = contextFunc();

                        if (context != _currentContext)
                            throw new InvalidOperationException("Cross-context queries are not allowed.");

                        newExpression = Expression.Constant(_currentContext.PersistenceAwareContext);
                    }
                }

                if (newExpression == null)
                    newExpression = Visit(m.Expression);
                Expression result = Expression.MakeMemberAccess(newExpression, newMember);
                return result;
            }
            else
            {
                return base.VisitMemberAccess(m);
            }
        }

        protected override MemberAssignment VisitMemberAssignment(MemberAssignment assignment)
        {
            Expression e = this.Visit(assignment.Expression);
            return Expression.Bind(TranslateMemberInfo(assignment.Member), e);
        }

        protected override Expression VisitParameter(ParameterExpression p)
        {
            ParameterExpression result;

            if (_parameterExpressionTranslationCache.TryGetValue(p, out result))
            {
                return result;
            }
            else
            {
                Type newType = TranslateType(p.Type);
                result = Expression.Parameter(newType, p.Name);
                _parameterExpressionTranslationCache.Add(p, result);

                return result;
            }
        }

        private MethodBase TranslateMethodInfo(MethodBase mi)
        {
            MethodBase result;

            if (_sharedCache.MethodInfoMapping.TryGetValue(mi, out result))
            {
                return result;
            }

            if (_sharedCache.NonTranslatedMethodInfos.Contains(mi))
            {
                return mi;
            }

            if (_privateCache.MethodInfoMapping.TryGetValue(mi, out result))
            {
                return result;
            }

            if (_privateCache.NonTranslatedMethodInfos.Contains(mi))
            {
                return mi;
            }

            result = DoTranslateMethodInfo(mi);
            if (mi != result)
                _privateCache.MethodInfoMapping.Add(mi, result);
            else
                _privateCache.NonTranslatedMethodInfos.Add(mi);

            return result;
        }

        private MethodBase DoTranslateMethodInfo(MethodBase mi)
        {
            Type[] typeArguments = null;
            Type[] translatedTypeArguments = null;

            if (mi.IsGenericMethod)
            {
                typeArguments = mi.GetGenericArguments();
                translatedTypeArguments = TranslateTypes(typeArguments);
            }

            Type declaringType = TranslateType(mi.DeclaringType);
            Type[] parameterTypes = mi.GetParameters().Select(c => c.ParameterType).ToArray();
            Type[] translatedParameterTypes = TranslateTypes(parameterTypes);

            if (typeArguments == translatedTypeArguments && parameterTypes == translatedParameterTypes)
            {
                return mi;
            }

            if (mi.IsGenericMethod)
            {
                return declaringType.GetMember(mi.Name, MemberTypes.Method, BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance)
                    .Cast<MethodInfo>()
                    .Where(c => c.IsGenericMethodDefinition)
                    .Where(c => ParametersMatch(c.GetParameters(), translatedParameterTypes))
                    .Single()
                    .MakeGenericMethod(translatedTypeArguments);
            }
            else
            {
                return declaringType.GetMember(mi.Name, MemberTypes.Method | MemberTypes.Constructor, BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance)
                    .Cast<MethodBase>()
                    .Where(c => ParametersMatch(c.GetParameters(), translatedParameterTypes))
                    .Single();
            }
        }

        private bool ParametersMatch(IList<ParameterInfo> pi1, IList<Type> ti2)
        {
            if (pi1.Count != ti2.Count)
                return false;

            for (int i = 0; i < pi1.Count; ++i)
            {
                if (!TypeMatches(pi1[i].ParameterType, ti2[i]))
                    return false;
            }
            return true;
        }

        private bool TypeMatches(Type t1, Type t2)
        {
            if (t1.IsGenericParameter)
                return true;

            if (t1.IsAssignableFrom(t2))
                return true;

            if (t1.IsGenericType != t2.IsGenericType)
                return false;

            if (t1.IsGenericType)
            {
                Type gent1 = t1.GetGenericTypeDefinition();
                Type gent2 = t2.GetGenericTypeDefinition();

                if (gent1 != gent2)
                    return false;

                var args1 = t1.GetGenericArguments();
                var args2 = t2.GetGenericArguments();

                for (int i = 0; i < args1.Length; ++i)
                {
                    if (!TypeMatches(args1[i], args2[i]))
                        return false;
                }
                return true;
            }

            return false;
        }

        private Type TranslateType(Type t)
        {
            Type result;
            if (_sharedCache.TypeMapping.TryGetValue(t, out result))
            {
                return result;
            }
            if (_sharedCache.NonTranslatedTypes.Contains(t))
            {
                return t;
            }
            if (_privateCache.TypeMapping.TryGetValue(t, out result))
            {
                return result;
            }
            if (_privateCache.NonTranslatedTypes.Contains(t))
            {
                return t;
            }
            result = DoTranslateType(t);
            if (t != result)
            {
                _privateCache.TypeMapping.Add(t, result);
                _privateCache.ReverseTypeMapping.Add(result, t);
            }
            else
            {
                _privateCache.NonTranslatedTypes.Add(t);
            }
            return result;
        }
        private Type DoTranslateType(Type t)
        {
            Type result;
            Type genericTypeDefinition = null;

            if (_contextType.IsAssignableFrom(t))
            {
                return _persistenceAwareContextType;
            }
            if (t.IsGenericType)
            {
                 genericTypeDefinition = t.GetGenericTypeDefinition();
                 if (genericTypeDefinition == typeof(EFPocoQuery<>))
                 {
                     genericTypeDefinition = typeof(ObjectQuery<>);
                 }
                 if (genericTypeDefinition == typeof(ICollection<>) || genericTypeDefinition == typeof(IList<>) || genericTypeDefinition == typeof(List<>) || genericTypeDefinition == typeof(HashSet<>))
                     genericTypeDefinition = typeof(EntityCollection<>);
            }

            if (t.IsGenericType)
            {
                Type[] typeArguments = t.GetGenericArguments();
                Type[] translatedTypeArguments = TranslateTypes(typeArguments);
                if (translatedTypeArguments != typeArguments)
                {
                    result = genericTypeDefinition.MakeGenericType(translatedTypeArguments);
                    return result;
                }
            }
            return t;
        }

        private Type[] TranslateTypes(Type[] types)
        {
            bool translated = false;

            Type[] newTypes = new Type[types.Length];
            for (int i = 0; i < types.Length; ++i)
            {
                newTypes[i] = TranslateType(types[i]);
                if (newTypes[i] != types[i])
                    translated = true;
            }
            if (translated)
                return newTypes;
            else
                return types;
        }
    }
}