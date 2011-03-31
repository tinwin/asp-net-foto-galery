// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Data.EntityClient;
using System.Data.Objects;
using System.Linq.Expressions;
using System.Reflection;

namespace EFPocoAdapter
{
    public class EFPocoContextFactory<T>
        where T : EFPocoContext, new()
    {
        private Func<ObjectContext> _creatorFunc0;
        private Func<string, ObjectContext> _creatorFunc1;
        private Func<EntityConnection, ObjectContext> _creatorFunc2;

        public EFPocoContextFactory(Type contextType)
        {
            ConstructorInfo constructorInfo0 = contextType.GetConstructor(new Type[] { });
            var creatorExpression0 = Expression.Lambda<Func<ObjectContext>>(Expression.New(constructorInfo0));

            ConstructorInfo constructorInfo1 = contextType.GetConstructor(new Type[] { typeof(string) });
            ParameterExpression parameterExpression1 = Expression.Parameter(typeof(string), "connectionString");
            var creatorExpression1 = Expression.Lambda<Func<string, ObjectContext>>(Expression.New(constructorInfo1, parameterExpression1), parameterExpression1);

            ConstructorInfo constructorInfo2 = contextType.GetConstructor(new Type[] { typeof(EntityConnection) });
            ParameterExpression parameterExpression2 = Expression.Parameter(typeof(EntityConnection), "connection");
            var creatorExpression2 = Expression.Lambda<Func<EntityConnection, ObjectContext>>(Expression.New(constructorInfo2, parameterExpression2), parameterExpression2);

            _creatorFunc0 = creatorExpression0.Compile();
            _creatorFunc1 = creatorExpression1.Compile();
            _creatorFunc2 = creatorExpression2.Compile();
        }

        public T CreateContext()
        {
            return new T { PersistenceAwareContext = _creatorFunc0() };
        }

        public T CreateContext(string connectionString)
        {
            return new T { PersistenceAwareContext = _creatorFunc1(connectionString) };
        }

        public T CreateContext(EntityConnection connection)
        {
            return new T { PersistenceAwareContext = _creatorFunc2(connection) };
        }
    }
}
