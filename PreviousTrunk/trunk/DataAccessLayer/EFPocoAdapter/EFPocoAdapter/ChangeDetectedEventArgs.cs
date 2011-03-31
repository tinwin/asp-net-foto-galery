// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;

namespace EFPocoAdapter
{
    public class ChangeDetectedEventArgs : EventArgs
    {
        public object Entity { get; internal set; }
        public string Member { get; internal set; }
        public object OldValue { get; internal set; }
        public object NewValue { get; internal set; }
    }
}
