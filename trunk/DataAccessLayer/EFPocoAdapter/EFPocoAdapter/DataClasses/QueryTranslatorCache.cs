// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Reflection;

namespace EFPocoAdapter.DataClasses
{
    public class QueryTranslationCache
    {
        internal volatile Dictionary<Type, Type> TypeMapping = new Dictionary<Type,Type>();
        internal volatile Dictionary<Type, Type> ReverseTypeMapping = new Dictionary<Type, Type>();
        internal volatile Dictionary<MethodBase, MethodBase> MethodInfoMapping = new Dictionary<MethodBase, MethodBase>();
        internal volatile HashSet<Type> NonTranslatedTypes = new HashSet<Type>();
        internal volatile HashSet<MethodBase> NonTranslatedMethodInfos = new HashSet<MethodBase>();
        internal Dictionary<Type, Func<object,IPocoAdapter>> AdapterCreators = new Dictionary<Type, Func<object,IPocoAdapter>>();

        public void AddTypeMapping(Type pocoType, Type adapterType, Func<object,IPocoAdapter> adapterCreator)
        {
            TypeMapping.Add(pocoType, adapterType);
            ReverseTypeMapping.Add(pocoType, pocoType);
            AdapterCreators[pocoType] = adapterCreator;
        }

        private static HashSet<K> Merge<K>(HashSet<K> set1, HashSet<K> set2)
        {
            if (set2.Count == 0)
                return set1;

            HashSet<K> merged = new HashSet<K>(set1);
            merged.UnionWith(set2);
            return merged;
        }

        private static Dictionary<K, V> Merge<K, V>(Dictionary<K, V> dict1, Dictionary<K, V> dict2)
        {
            if (dict2.Count == 0)
                return dict1;

            Dictionary<K, V> merged = new Dictionary<K, V>(dict1);
            foreach (var k in dict2)
            {
                merged.Add(k.Key, k.Value);
            }
            return merged;
        }

        internal void MergeWith(QueryTranslationCache otherCache)
        {
            // no locking here because collections stored in shared cache are immutable
            //
            // the merge below does not have to be perfect - if we lose some part of the cache
            // because of a race condition, we'll be able to recreate it next time
            //

            this.TypeMapping = Merge(this.TypeMapping, otherCache.TypeMapping);
            this.ReverseTypeMapping = Merge(this.ReverseTypeMapping, otherCache.ReverseTypeMapping);
            this.MethodInfoMapping = Merge(this.MethodInfoMapping, otherCache.MethodInfoMapping);
            this.NonTranslatedMethodInfos = Merge(this.NonTranslatedMethodInfos, otherCache.NonTranslatedMethodInfos);
            this.NonTranslatedTypes = Merge(this.NonTranslatedTypes, otherCache.NonTranslatedTypes);
        }
    }
}
