// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;

namespace NorthwindEF
{
    [Serializable]
    public class Product
    {
        public Product(Supplier supplier)
        {
            Supplier = supplier;
        }

        public Int32 ProductID { get; set; }
        public String ProductName { get; set; }
        public String QuantityPerUnit { get; set; }
        public Decimal UnitPrice { get; set; }
        public Int16? UnitsInStock { get; set; }
        public Int16? UnitsOnOrder { get; set; }
        public Int16? ReorderLevel { get; set; }
        public Category Category { get; set; }
        public Supplier Supplier { get; private set; }
    }
}
