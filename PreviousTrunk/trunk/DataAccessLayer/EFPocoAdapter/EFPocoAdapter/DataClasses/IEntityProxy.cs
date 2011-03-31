// Copyright (c) Microsoft Corporation.  All rights reserved.

namespace EFPocoAdapter.DataClasses
{
    public interface IEntityProxy
    {
        IPocoAdapter Adapter { get; set; }
    }
}
