// Copyright (c) Microsoft Corporation.  All rights reserved.

using System.Data.Objects;

namespace EFPocoAdapter.Internal
{
    internal interface IObjectQueryWrapper
    {
        ObjectQuery WrappedQuery { get; }
    }
}
