// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;
using EFPocoAdapter.Internal;

namespace EFPocoAdapter
{
    public class EFPocoQuery<T> : IQueryable<T>, IOrderedQueryable<T>, IObjectQueryWrapper
    {
        private ObjectQuery _wrappedQuery;
        private EFPocoContext _context;
        private Expression _expression;

        public EFPocoQuery(EFPocoContext context, ObjectQuery query)
        {
            _context = context;
            _wrappedQuery = query;
            _expression = Expression.Constant(this);
        }

        public EFPocoQuery(EFPocoContext context, Expression expression)
        {
            _context = context;
            _wrappedQuery = null;
            _expression = expression;
        }

        internal EFPocoContext Context
        {
            get { return _context; }
        }

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            Func<EFPocoContext, object, IEnumerable<T>> adapterFunction;
            Expression expr = Context.CreateContextBoundQueryTranslator().Translate(Expression, out adapterFunction);
            ObjectQuery objectQueryOfT = (ObjectQuery)Context.EntityFrameworkQueryProvider.CreateQuery(expr);

            return adapterFunction(_context, objectQueryOfT).GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region IQueryable Members

        public Type ElementType
        {
            get { return typeof(T); }
        }

        public System.Linq.Expressions.Expression Expression
        {
            get { return _expression; }
        }

        public IQueryProvider Provider
        {
            get { return _context.QueryProvider; }
        }

        #endregion

        #region Query Builder Methods (Entity SQL)

        public EFPocoQuery<T> Distinct()
        {
            return new EFPocoQuery<T>(this.Context, (ObjectQuery)_wrappedQuery.GetType().GetMethod("Distinct").Invoke(_wrappedQuery, new object[] { }));
        }

        public EFPocoQuery<T> Except(EFPocoQuery<T> query)
        {
            return new EFPocoQuery<T>(this.Context, (ObjectQuery)_wrappedQuery.GetType().GetMethod("Except").Invoke(_wrappedQuery, new object[] { query.WrappedQuery }));
        }

        public EFPocoQuery<DbDataRecord> GroupBy(string keys, string projection, params ObjectParameter[] parameters)
        {
            return new EFPocoQuery<DbDataRecord>(this.Context, (ObjectQuery)_wrappedQuery.GetType().GetMethod("GroupBy").Invoke(_wrappedQuery, new object[] { keys, projection, parameters }));
        }

        public EFPocoQuery<T> Include(string path)
        {
            return new EFPocoQuery<T>(this.Context, (ObjectQuery)_wrappedQuery.GetType().GetMethod("Include").Invoke(_wrappedQuery, new object[] { path }));
        }

        public EFPocoQuery<T> Intersect(EFPocoQuery<T> query)
        {
            return new EFPocoQuery<T>(this.Context, (ObjectQuery)_wrappedQuery.GetType().GetMethod("Intersect").Invoke(_wrappedQuery, new object[] { query.WrappedQuery }));
        }

        public EFPocoQuery<TResultType> OfType<TResultType>()
        {
            return new EFPocoQuery<TResultType>(this.Context, (ObjectQuery)_wrappedQuery.GetType().GetMethod("OfType").MakeGenericMethod(GetAdapterType(typeof(TResultType))).Invoke(_wrappedQuery, new object[] { }));
        }

        public EFPocoQuery<T> OrderBy(string keys, params ObjectParameter[] parameters)
        {
            return new EFPocoQuery<T>(this.Context, (ObjectQuery)_wrappedQuery.GetType().GetMethod("OrderBy").Invoke(_wrappedQuery, new object[] { keys, parameters }));
        }

        public EFPocoQuery<DbDataRecord> Select(string projection, params ObjectParameter[] parameters)
        {
            return new EFPocoQuery<DbDataRecord>(this.Context, (ObjectQuery)_wrappedQuery.GetType().GetMethod("Select").Invoke(_wrappedQuery, new object[] { projection, parameters }));
        }

        public EFPocoQuery<TResultType> SelectValue<TResultType>(string projection, params ObjectParameter[] parameters)
        {
            return new EFPocoQuery<TResultType>(this.Context, (ObjectQuery)_wrappedQuery.GetType().GetMethod("SelectValue").MakeGenericMethod(GetAdapterType(typeof(TResultType))).Invoke(_wrappedQuery, new object[] { projection, parameters }));
        }

        public EFPocoQuery<T> Skip(string keys, string count, params ObjectParameter[] parameters)
        {
            return new EFPocoQuery<T>(this.Context, (ObjectQuery)_wrappedQuery.GetType().GetMethod("Skip").Invoke(_wrappedQuery, new object[] { keys, count, parameters }));
        }

        public EFPocoQuery<T> Top(string count, params ObjectParameter[] parameters)
        {
            return new EFPocoQuery<T>(this.Context, (ObjectQuery)_wrappedQuery.GetType().GetMethod("Top").Invoke(_wrappedQuery, new object[] { count, parameters }));
        }

        public EFPocoQuery<T> Union(EFPocoQuery<T> query)
        {
            return new EFPocoQuery<T>(this.Context, (ObjectQuery)_wrappedQuery.GetType().GetMethod("Union").Invoke(_wrappedQuery, new object[] { query.WrappedQuery }));
        }

        public EFPocoQuery<T> UnionAll(EFPocoQuery<T> query)
        {
            return new EFPocoQuery<T>(this.Context, (ObjectQuery)_wrappedQuery.GetType().GetMethod("UnionAll").Invoke(_wrappedQuery, new object[] { query.WrappedQuery }));
        }

        public EFPocoQuery<T> Where(string predicate, params ObjectParameter[] parameters)
        {
            return new EFPocoQuery<T>(this.Context, (ObjectQuery)_wrappedQuery.GetType().GetMethod("Where").Invoke(_wrappedQuery, new object[] { predicate, parameters }));
        }

        #endregion

        ObjectQuery IObjectQueryWrapper.WrappedQuery
        {
            get { return _wrappedQuery; }
        }

        internal ObjectQuery WrappedQuery
        {
            get { return _wrappedQuery; }
        }

        private Type GetAdapterType(Type t)
        {
            return _context.GetAdapterType(t);
        }
    }
}
