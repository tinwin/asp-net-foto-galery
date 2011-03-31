// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;

namespace NorthwindEF
{
    [Serializable]
    public class InternationalOrder : Order
    {
        public String CustomsDescription { get; set; }
        public Decimal ExciseTax { get; set; }
    }
}
