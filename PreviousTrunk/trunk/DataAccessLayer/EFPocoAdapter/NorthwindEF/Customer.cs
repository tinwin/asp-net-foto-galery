// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;

namespace NorthwindEF
{
    [Serializable]
    public class Customer
    {
        public String CustomerID { get; set; }
        public String CompanyName { get; set; }
        public ContactInfo ContactPerson { get; set; }
        public CommonAddress Address; // field
        public String Phone { get; set; }
        public String Fax { get; set; }
        public IList<Order> Orders { get; set; }
    }
}
