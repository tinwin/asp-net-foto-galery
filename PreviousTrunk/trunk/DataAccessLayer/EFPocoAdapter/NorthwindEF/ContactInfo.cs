// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;

namespace NorthwindEF
{
    [Serializable]
    public struct ContactInfo
    {
        public string Name { get; set; }
        public string Title {get; set; }
    }
}
