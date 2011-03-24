// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Linq;
using System.Linq.Expressions;
using EFPocoAdapter.DataClasses;

namespace EFPocoAdapter
{
    public static class ExtensionMethods
    {
        public static IQueryable<T> Include<T>(this IQueryable<T> query, string navigationProperty)
        {
            EFPocoQuery<T> oqt = query as EFPocoQuery<T>;
            if (oqt == null)
                throw new NotSupportedException("Include is not supported on this type of query.");

            return oqt.Include(navigationProperty);
        }

        public static IQueryable<T> Include<T>(this IQueryable<T> query, Expression<Func<T,object>> navigationPropertySelector)
        {
            return query.Include(ExpressionToPropertyName(navigationPropertySelector));
        }

        private static string ExpressionToPropertyName<T, T2>(Expression<Func<T, T2>> selector)
        {
            MemberExpression me = selector.Body as MemberExpression;
            if (me == null)
                throw new NotSupportedException("MemberException expected.");

            if (me.Expression.NodeType != ExpressionType.Parameter)
                throw new NotSupportedException("Paramter expected");

            if (selector.Parameters[0] != me.Expression)
                throw new NotSupportedException("Invalid parameter.");

            return me.Member.Name;
        }

        public static T GetPocoEntityOrNull<T>(this IPocoAdapter<T> adapter)
            where T : class
        {
            if (adapter == null)
                return null;
            else
                return adapter.PocoEntity;
        }
    }
}
