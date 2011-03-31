// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;

namespace NorthwindEF
{
    public class DiscontinuedProduct : Product
    {
        public DiscontinuedProduct(Supplier supplier)
            : base(supplier)
        {
        }
        public DateTime? DiscontinuedDate { get; set; }
    }
}
