// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using EFPocoAdapter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NorthwindEF.Tests
{
    [TestClass]
    public class ReferencePropertiesTests
    {
        public ReferencePropertiesTests()
        {
        }

        [TestMethod]
        public void SimpleLoad()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                var prod = context.Products.First(c=>c.ProductID == 1);
                Assert.IsNull(prod.Category);
            }
        }

        [TestMethod]
        public void LoadWithInclude()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                var prod = context.Products.Include(c => c.Category).First(c => c.ProductID == 1);
                Assert.IsNotNull(prod.Category);
                Assert.AreEqual("Beverages", prod.Category.CategoryName);

                var prod1 = prod.Category.Products;

                Assert.AreEqual(1, prod.Category.Products.Count);
                Assert.IsFalse(context.IsPropertyLoaded(prod.Category, c => c.Products));
                context.LoadProperty(prod.Category, c => c.Products);
                Assert.IsTrue(context.IsPropertyLoaded(prod.Category, c => c.Products));
                var prod2 = prod.Category.Products;
                Assert.AreEqual(12, prod.Category.Products.Count);
                Assert.AreSame(prod1, prod2);
            }
        }

        [TestMethod]
        public void AutoWireUp1()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                var orderArray = context.Orders.ToArray();
                var custArray = context.Customers.ToArray();
                foreach (Customer c in custArray)
                {
                    foreach (Order o in c.Orders)
                    {
                        Assert.AreSame(o.Customer, c);
                    }
                }

                foreach (Order o in orderArray)
                {
                    if (o.Customer != null)
                        Assert.IsTrue(o.Customer.Orders.Contains(o));
                }
            }
        }

        [TestMethod]
        public void AutoWireUp2()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                var custArray = context.Customers.ToArray();
                var orderArray = context.Orders.ToArray();

                foreach (Customer c in custArray)
                {
                    foreach (Order o in c.Orders)
                    {
                        Assert.AreSame(o.Customer, c);
                    }
                }

                foreach (Order o in orderArray)
                {
                    if (o.Customer != null)
                        Assert.IsTrue(o.Customer.Orders.Contains(o));
                }
            }
        }
    }
}
