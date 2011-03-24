// Copyright (c) Microsoft Corporation.  All rights reserved.

namespace EFPocoAdapter
{
    public interface ILazyLoadable
    {
        bool IsLoaded { get; }
        void Load();
    }
}
