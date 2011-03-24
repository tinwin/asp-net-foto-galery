// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;

namespace EFPocoAdapter
{
    public class CollectionChangeDetectedEventArgs : EventArgs
    {
        public object Entity { get; internal set; }
        public string Member { get; internal set; }
        public ICollection<object> AddedObjects { get; internal set; }
        public ICollection<object> RemovedObjects { get; internal set; }
    }
}
