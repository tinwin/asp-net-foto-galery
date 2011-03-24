// Copyright (c) Microsoft Corporation.  All rights reserved.

namespace EFPocoAdapter.DataClasses
{
    public interface IComplexTypeAdapter<T>
    {
        T CreatePocoStructure();
        void DetectChangesFrom(T pocoComplexObject, IPocoAdapter parentObject, string propertyName);
    }
}
