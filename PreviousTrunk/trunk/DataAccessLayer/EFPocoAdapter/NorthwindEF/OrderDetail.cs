// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;

namespace NorthwindEF
{
    [Serializable]
    public class OrderDetail
    {
        public Int32 OrderID { get; set; }
        public Int32 ProductID { get; set; }
        public virtual Decimal UnitPrice { get; set; }
        public virtual Int16 Quantity { get; set; }
        public virtual Single Discount { get; set; }
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
