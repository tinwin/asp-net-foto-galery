// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using EFPocoAdapter;
using NorthwindEF.Territories;

namespace NorthwindEF
{
    public class MyHandWrittenNorthwindEntities : EFPocoContext
    {
        public MyHandWrittenNorthwindEntities()
        {
        }

        public IEntitySet<Employee> Employees
        {
            get { return GetEntitySet<Employee>("Employees"); }
        }

        public IEntitySet<Territory> Territories
        {
            get { return GetEntitySet<Territory>("Territories"); }
        }

        public IEntitySet<Region> Regions
        {
            get { return GetEntitySet<Region>("Regions"); }
        }

        public IEntitySet<Supplier> Suppliers
        {
            get { return GetEntitySet<Supplier>("Suppliers"); }
        }

        public IEntitySet<Product> Products
        {
            get { return GetEntitySet<Product>("Products"); }
        }

        public IEntitySet<Category> Categories
        {
            get { return GetEntitySet<Category>("Categories"); }
        }

        public IEntitySet<Customer> Customers
        {
            get { return GetEntitySet<Customer>("Customers"); }
        }

        public IEntitySet<Order> Orders
        {
            get { return GetEntitySet<Order>("Orders"); }
        }

        public IEntitySet<OrderDetail> OrderDetails
        {
            get { return GetEntitySet<OrderDetail>("OrderDetails"); }
        }
    }
}
