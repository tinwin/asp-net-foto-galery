// Copyright (c) Microsoft Corporation.  All rights reserved.

namespace EFPocoAdapter.DataClasses
{
    public interface IPocoAdapter<T>
    {
        T PocoEntity { get; }
    }
}
