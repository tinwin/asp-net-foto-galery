// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using EFPocoAdapter.DataClasses;
using System.Runtime.CompilerServices;

namespace EFPocoAdapter.Internal
{
    internal class QueryResultsAdapter
    {
        internal static MethodInfo ConvertToListMethodInfo = typeof(QueryResultsAdapter).GetMethod("ConvertToList", BindingFlags.Public | BindingFlags.Static);
        internal static MethodInfo ConvertToReadOnlyCollectionMethodInfo = typeof(QueryResultsAdapter).GetMethod("ConvertToReadOnlyCollection", BindingFlags.Public | BindingFlags.Static);
        internal static MethodInfo ConvertToHashSetMethodInfo = typeof(QueryResultsAdapter).GetMethod("ConvertToHashSet", BindingFlags.Public | BindingFlags.Static);
        internal static MethodInfo ConvertEntityObjectMethodInfo = typeof(QueryResultsAdapter).GetMethod("ConvertEntityObject", BindingFlags.Public | BindingFlags.Static);
        internal static MethodInfo ConvertComplexObjectMethodInfo = typeof(QueryResultsAdapter).GetMethod("ConvertComplexObject", BindingFlags.Public | BindingFlags.Static);

        public static List<TPoco> ConvertToList<TAdapter, TPoco>(IEnumerable<TAdapter> collection)
            where TAdapter : class, IPocoAdapter<TPoco>, IEntityWithRelationships
        {
            return collection.Select(c => c.PocoEntity).ToList();
        }

        public static ReadOnlyCollection<TPoco> ConvertToReadOnlyCollection<TAdapter, TPoco>(IEnumerable<TAdapter> collection)
            where TAdapter : class, IPocoAdapter<TPoco>, IEntityWithRelationships
        {
            return collection.Select(c => c.PocoEntity).ToList().AsReadOnly();
        }

        public static HashSet<TPoco> ConvertToHashSet<TAdapter, TPoco>(IEnumerable<TAdapter> collection)
            where TAdapter : class, IPocoAdapter<TPoco>, IEntityWithRelationships
        {
            return new HashSet<TPoco>(collection.Select(c => c.PocoEntity));
        }

        public static TPoco ConvertComplexObject<TAdapter, TPoco>(TAdapter adapter)
            where TAdapter : IComplexTypeAdapter<TPoco>
        {
            if (adapter == null)
                return default(TPoco);
            return adapter.CreatePocoStructure();
        }

        public static TPoco ConvertEntityObject<TAdapter, TPoco>(TAdapter adapter)
            where TAdapter : IPocoAdapter<TPoco>
        {
            if (adapter == null)
                return default(TPoco);
            return adapter.PocoEntity;
        }

        private static Expression GenerateAdapter(Expression sourceExpression, Type targetType)
        {
            Type sourceType = sourceExpression.Type;

            // directly assignable - no conversion
            if (targetType.IsAssignableFrom(sourceType))
                return sourceExpression;

            // entity object - get PocoEntity property
            if (typeof(EntityObject).IsAssignableFrom(sourceExpression.Type))
            {
                return Expression.Call(ConvertEntityObjectMethodInfo.MakeGenericMethod(sourceExpression.Type, targetType), sourceExpression);
            }

            // complex object - call CreatePocoEntity() method
            if (typeof(ComplexObject).IsAssignableFrom(sourceExpression.Type))
            {
                return Expression.Call(ConvertComplexObjectMethodInfo.MakeGenericMethod(sourceExpression.Type, targetType), sourceExpression);
            }

            // C#/VB anonymous types
            //    recursively construct target type with converted arguments
            if (targetType.IsDefined(typeof(CompilerGeneratedAttribute), false))
            {
                var ctor = targetType.GetConstructors().Single();
                return Expression.New(ctor, ctor.GetParameters()
                    .Select(par => GenerateAdapter(
                        Expression.MakeMemberAccess(sourceExpression, sourceType.GetMember(par.Name).Single()),
                            par.ParameterType)));
            }

            // call one of the helper methods to translate EntityCollection<T> to POCO collection types
            Type sourceElementType = null;
            Type ienumerableOfSourceElementType = null;

            if (sourceExpression.Type.IsGenericType)
            {
                sourceElementType = sourceExpression.Type.GetGenericArguments()[0];
                ienumerableOfSourceElementType = typeof(IEnumerable<>).MakeGenericType(sourceElementType);
            }

            if (ienumerableOfSourceElementType != null && ienumerableOfSourceElementType.IsAssignableFrom(sourceExpression.Type))
            {
                Type targetElementType = targetType.GetGenericArguments()[0];
                Type collectionType = targetType.GetGenericTypeDefinition();

                if (collectionType.IsInterface || collectionType == typeof(List<>))
                {
                    return Expression.Call(ConvertToListMethodInfo.MakeGenericMethod(sourceElementType, targetElementType), sourceExpression);
                }

                if (collectionType == typeof(HashSet<>))
                {
                    return Expression.Call(ConvertToHashSetMethodInfo.MakeGenericMethod(sourceElementType, targetElementType), sourceExpression);
                }

                if (collectionType == typeof(ReadOnlyCollection<>))
                {
                    return Expression.Call(ConvertToReadOnlyCollectionMethodInfo.MakeGenericMethod(sourceElementType, targetElementType), sourceExpression);
                }

                throw new NotImplementedException("Conversion of EntityCollection to " + collectionType + " not supported.");
            }

            throw new NotImplementedException("Conversion from " + sourceType.Name + " to " + targetType.Name + " is not implemented.");
        }

        public static Func<EFPocoContext, object, T> GenerateAdapter<T>(Type sourceType, Type targetType)
        {
            ParameterExpression contextParameter = Expression.Parameter(typeof(EFPocoContext), "context");
            ParameterExpression parameter = Expression.Parameter(typeof(object), "c");
            Expression adapterExpression = GenerateAdapter(Expression.Convert(parameter, sourceType), targetType);
            return Expression.Lambda<Func<EFPocoContext, object, T>>(adapterExpression, contextParameter, parameter).Compile();
        }

        public static Delegate GenerateAdapter(Type delegateType, Type sourceType, Type targetType)
        {
            ParameterExpression parameter = Expression.Parameter(sourceType, "c");
            Expression adapterExpression = GenerateAdapter(parameter, targetType);
            return Expression.Lambda(delegateType, adapterExpression, parameter).Compile();
        }
    }
}
