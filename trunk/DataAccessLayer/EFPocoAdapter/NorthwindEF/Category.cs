// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;

namespace NorthwindEF
{
    [Serializable]
    public class Category
    {
        public Category(int categoryID, string categoryName, string description, List<Product> products)
        {
            CategoryID = categoryID;
            CategoryName = categoryName;
            Description = description;
            Products = products;
        }

        public Int32 CategoryID { get; private set; }
        public String CategoryName { get; private set; }
        public String Description { get; private set; }
        public Byte[] Picture { get; set;  }
        public List<Product> Products { get; private set; }
    }
}
