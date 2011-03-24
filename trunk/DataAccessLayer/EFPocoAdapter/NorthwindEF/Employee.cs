// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using NorthwindEF.Territories;

namespace NorthwindEF
{
    public abstract class Employee
    {
        public Int32 EmployeeID { get; set; }
        public String LastName { get; set; }
        public String FirstName { get; set; }
        public String Title { get; set; }
        public String TitleOfCourtesy { get; set; }
        public virtual DateTime? BirthDate { get; set; }
        public DateTime? HireDate { get; set; }
        public CommonAddress Address { get; set; }
        public String HomePhone { get; set; }
        public String Extension { get; set; }
        public Byte[] Photo { get; set; }
        public String Notes { get; set; }
        public String PhotoPath { get; set; }
        public HashSet<Territory> Territories { get; set; }
    }
}
