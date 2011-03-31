// Copyright (c) Microsoft Corporation.  All rights reserved.

namespace EFPocoAdapter.DataClasses
{
    public interface IPocoAdapter
    {
        EFPocoContext Context { get; set; }
        void DetectChanges();
        object PocoEntity { get; set; }

        void InitCollections(bool allowProxies);
        object CreatePocoEntity();
        object CreatePocoEntityProxy();
        void PopulatePocoEntity(bool allowProxies);

        void RaiseChangeDetected(string member, object oldValue, object newValue);
        bool CanLoadProperty(string member);
    }
}
