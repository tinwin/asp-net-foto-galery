// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;

namespace NorthwindEF
{
    [Serializable]
    public class Order
    {
        public Int32 OrderID { get; set; }
        public Int32 EmployeeID { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public Decimal? Freight { get; set; }
        public String ShipName { get; set; }
        public String ShipAddress { get; set; }
        public String ShipCity { get; set; }
        public String ShipRegion { get; set; }
        public String ShipPostalCode { get; set; }
        public String ShipCountry { get; set; }
        public virtual Customer Customer { get; set; }
        public IList<OrderDetail> OrderDetails { get; set; }
    }
}
