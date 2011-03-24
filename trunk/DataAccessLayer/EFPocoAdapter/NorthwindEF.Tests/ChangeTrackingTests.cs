// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using EFPocoAdapter;

namespace NorthwindEF.Tests
{
    [TestClass]
    public class ChangeTrackingTests
    {
        public ChangeTrackingTests()
        {
        }

        [TestMethod]
        public void ChangeTrackingTestString1()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                List<ChangeDetectedEventArgs> changes = new List<ChangeDetectedEventArgs>();

                var o = context.Orders.First(c=>c.OrderID == 10248);
                var oldShipAddress = o.ShipAddress;
                o.ShipAddress = "newShipAddress";
                context.ChangeDetected += delegate(object sender, ChangeDetectedEventArgs e) { changes.Add(e); };
                context.DetectChanges();
                Assert.AreEqual(1, changes.Count);
                Assert.AreSame(o, changes[0].Entity);
                Assert.AreEqual("ShipAddress", changes[0].Member);
                Assert.AreEqual(oldShipAddress, changes[0].OldValue);
                Assert.AreEqual("newShipAddress", changes[0].NewValue);
            }
        }

        [TestMethod]
        public void ChangeTrackingTestString2()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                List<ChangeDetectedEventArgs> changes = new List<ChangeDetectedEventArgs>();

                var o = context.Orders.First(c => c.OrderID == 10248);
                var oldShipAddress = o.ShipAddress;
                o.ShipAddress = null;
                context.ChangeDetected += delegate(object sender, ChangeDetectedEventArgs e) { changes.Add(e); };
                context.DetectChanges();
                Assert.AreEqual(1, changes.Count);
                Assert.AreSame(o, changes[0].Entity);
                Assert.AreEqual("ShipAddress", changes[0].Member);
                Assert.AreEqual(oldShipAddress, changes[0].OldValue);
                Assert.IsNull(changes[0].NewValue);
                changes.Clear();
                o.ShipAddress = "SomeNewAddress";
                context.DetectChanges();
                Assert.AreEqual(1, changes.Count);
                Assert.AreSame(o, changes[0].Entity);
                Assert.AreEqual("ShipAddress", changes[0].Member);
                Assert.AreEqual(o.ShipAddress, changes[0].NewValue);
                Assert.IsNull(changes[0].OldValue);
            }
        }

        [TestMethod]
        public void ChangeTrackingByteArray1()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                List<ChangeDetectedEventArgs> changes = new List<ChangeDetectedEventArgs>();

                var cat = context.Categories.First(c => c.CategoryID == 1);
                byte[] oldPic = cat.Picture;
                cat.Picture = new byte[3] { 1, 2, 3 };
                context.ChangeDetected += delegate(object sender, ChangeDetectedEventArgs e) { changes.Add(e); };
                context.DetectChanges();
                Assert.AreEqual(1, changes.Count);
                Assert.AreSame(cat, changes[0].Entity);
                Assert.AreEqual("Picture", changes[0].Member);
                Assert.AreEqual(oldPic, changes[0].OldValue);
                Assert.AreSame(cat.Picture, changes[0].NewValue);
            }
        }

        [TestMethod]
        public void ChangeTrackingByteArray2()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                List<ChangeDetectedEventArgs> changes = new List<ChangeDetectedEventArgs>();

                var cat = context.Categories.First(c => c.CategoryID == 1);
                context.ChangeDetected += delegate(object sender, ChangeDetectedEventArgs e) { changes.Add(e); };
                cat.Picture[0]++;
                context.DetectChanges();
                Assert.AreEqual(0, changes.Count);

                // we are not detecting changes to individual bytes in an array
            }
        }

        [TestMethod]
        public void ChangeTrackingByteArray3()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                List<ChangeDetectedEventArgs> changes = new List<ChangeDetectedEventArgs>();

                var cat = context.Categories.First(c => c.CategoryID == 1);
                cat.Picture = null;
                context.ChangeDetected += delegate(object sender, ChangeDetectedEventArgs e) { changes.Add(e); };
                context.DetectChanges();
                Assert.AreEqual(1, changes.Count);
                Assert.AreSame(cat, changes[0].Entity);
                Assert.AreEqual("Picture", changes[0].Member);
                Assert.IsNull(changes[0].NewValue);
                Assert.IsNotNull(changes[0].OldValue);
                cat.Picture = new byte[3] { 1, 2, 3 };
                changes.Clear();
                context.DetectChanges();
                Assert.AreEqual(1, changes.Count);
                Assert.AreSame(cat, changes[0].Entity);
                Assert.AreEqual("Picture", changes[0].Member);
                Assert.IsNull(changes[0].OldValue);
                Assert.IsNotNull(changes[0].NewValue);
            }
        }

        [TestMethod]
        public void ChangeTrackingTestNullableDate1()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                List<ChangeDetectedEventArgs> changes = new List<ChangeDetectedEventArgs>();

                var o = context.Orders.First(c => c.OrderID == 10248);
                var oldOrderDate = o.OrderDate;
                o.OrderDate = DateTime.Now;
                context.ChangeDetected += delegate(object sender, ChangeDetectedEventArgs e) { changes.Add(e); };
                context.DetectChanges();
                Assert.AreEqual(1, changes.Count);
                Assert.AreSame(o, changes[0].Entity);
                Assert.AreEqual("OrderDate", changes[0].Member);
                Assert.AreEqual(oldOrderDate, changes[0].OldValue);
                Assert.AreEqual(o.OrderDate, changes[0].NewValue);
            }
        }

        [TestMethod]
        public void ChangeTrackingTestNulalbleDate2()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                List<ChangeDetectedEventArgs> changes = new List<ChangeDetectedEventArgs>();

                var o = context.Orders.First(c => c.OrderID == 10248);
                var oldOrderDate = o.OrderDate;
                o.OrderDate = null;
                context.ChangeDetected += delegate(object sender, ChangeDetectedEventArgs e) { changes.Add(e); };
                context.DetectChanges();
                Assert.AreEqual(1, changes.Count);
                Assert.AreSame(o, changes[0].Entity);
                Assert.AreEqual("OrderDate", changes[0].Member);
                Assert.AreEqual(oldOrderDate, changes[0].OldValue);
                Assert.IsNull(changes[0].NewValue);
                changes.Clear();
                o.OrderDate = DateTime.Now;
                context.DetectChanges();
                Assert.AreEqual(1, changes.Count);
                Assert.AreSame(o, changes[0].Entity);
                Assert.AreEqual("OrderDate", changes[0].Member);
                Assert.AreEqual(o.OrderDate, changes[0].NewValue);
                Assert.IsNull(changes[0].OldValue);
            }
        }

        [TestMethod]
        public void ChangeTrackingValueType1()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                List<ChangeDetectedEventArgs> changes = new List<ChangeDetectedEventArgs>();

                var od = context.Products.First();
                var oldValue = od.UnitPrice;

                od.UnitPrice++;
                context.ChangeDetected += delegate(object sender, ChangeDetectedEventArgs e) { changes.Add(e); };
                context.DetectChanges();
                Assert.AreEqual(1, changes.Count);
                Assert.AreSame(od, changes[0].Entity);
                Assert.AreEqual("UnitPrice", changes[0].Member);
                Assert.AreEqual(oldValue, changes[0].OldValue);
                Assert.AreEqual(od.UnitPrice, changes[0].NewValue);
            }
        }

        [TestMethod]
        public void ChangeTrackingReference1()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                List<ChangeDetectedEventArgs> changes = new List<ChangeDetectedEventArgs>();

                context.ChangeDetected += delegate(object sender, ChangeDetectedEventArgs e) { changes.Add(e); };
                var prod = context.Products.Include(c=>c.Category).First();
                var oldCat = prod.Category;
                var oldCatID = oldCat.CategoryID;
                var someOtherCategory = context.Categories.First(c => c.CategoryID != oldCatID);
                prod.Category = someOtherCategory;
                Assert.AreEqual(0, changes.Count);
                context.DetectChanges();
                Assert.AreEqual(1, changes.Count);
                Assert.AreSame(prod, changes[0].Entity);
                Assert.AreEqual("Category", changes[0].Member);
                Assert.AreEqual(oldCat, changes[0].OldValue);
                Assert.AreEqual(prod.Category, changes[0].NewValue);
            }
        }

        [TestMethod]
        public void ChangeTrackingReference2()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                List<ChangeDetectedEventArgs> changes = new List<ChangeDetectedEventArgs>();

                context.ChangeDetected += delegate(object sender, ChangeDetectedEventArgs e) { changes.Add(e); };
                var prod = context.Products.Include(c => c.Category).First();
                var oldCat = prod.Category;
                var oldCatID = oldCat.CategoryID;
                prod.Category = null;
                Assert.AreEqual(0, changes.Count);
                context.DetectChanges();
                Assert.AreEqual(1, changes.Count);
                Assert.AreSame(prod, changes[0].Entity);
                Assert.AreEqual("Category", changes[0].Member);
                Assert.AreEqual(oldCat, changes[0].OldValue);
                Assert.AreEqual(prod.Category, changes[0].NewValue);
            }
        }

        [TestMethod]
        public void ChangeTrackingUsingProxy1()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                List<ChangeDetectedEventArgs> changes = new List<ChangeDetectedEventArgs>();

                var od = context.OrderDetails.First();
                var oldValue = od.UnitPrice;

                context.ChangeDetected += delegate(object sender, ChangeDetectedEventArgs e) { changes.Add(e); };
                Assert.AreEqual(0, changes.Count);
                od.UnitPrice++;
                Assert.AreEqual(1, changes.Count);
                Assert.AreSame(od, changes[0].Entity);
                Assert.AreEqual("UnitPrice", changes[0].Member);
                Assert.AreEqual(oldValue, changes[0].OldValue);
                Assert.AreEqual(od.UnitPrice, changes[0].NewValue);
            }
        }

        [TestMethod]
        public void ChangeTrackingUsingProxy2()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                List<ChangeDetectedEventArgs> changes = new List<ChangeDetectedEventArgs>();

                var od = context.OrderDetails.First();
                var oldValue = od.Quantity;

                context.ChangeDetected += delegate(object sender, ChangeDetectedEventArgs e) { changes.Add(e); };
                Assert.AreEqual(0, changes.Count);
                od.Quantity++;
                Assert.AreEqual(1, changes.Count);
                Assert.AreSame(od, changes[0].Entity);
                Assert.AreEqual("Quantity", changes[0].Member);
                Assert.AreEqual(oldValue, changes[0].OldValue);
                Assert.AreEqual(od.Quantity, changes[0].NewValue);
            }
        }
 
        [TestMethod]
        public void ChangeTrackingUsingProxy3()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                List<ChangeDetectedEventArgs> changes = new List<ChangeDetectedEventArgs>();

                var od = context.OrderDetails.First();
                var oldValue = od.Discount;

                context.ChangeDetected += delegate(object sender, ChangeDetectedEventArgs e) { changes.Add(e); };
                Assert.AreEqual(0, changes.Count);
                od.Discount++;
                Assert.AreEqual(1, changes.Count);
                Assert.AreSame(od, changes[0].Entity);
                Assert.AreEqual("Discount", changes[0].Member);
                Assert.AreEqual(oldValue, changes[0].OldValue);
                Assert.AreEqual(od.Discount, changes[0].NewValue);
            }
        }

        [TestMethod]
        public void ChangeTrackingUsingProxy4()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                List<ChangeDetectedEventArgs> changes = new List<ChangeDetectedEventArgs>();

                context.ChangeDetected += delegate(object sender, ChangeDetectedEventArgs e) { changes.Add(e); };
                foreach (var emp in context.Employees)
                {
                    if (emp.BirthDate.HasValue)
                        emp.BirthDate = emp.BirthDate.Value.AddDays(1);
                };

                Assert.AreEqual(context.Employees.Count(), changes.Count);
            }
        }

        [TestMethod]
        public void ChangeTrackingUsingProxy5()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                List<ChangeDetectedEventArgs> changes = new List<ChangeDetectedEventArgs>();

                context.ChangeDetected += delegate(object sender, ChangeDetectedEventArgs e) { changes.Add(e); };
                context.Territories.First().TerritoryDescription += "?";
                Assert.AreEqual(1, changes.Count);
                Assert.IsTrue(((string)changes[0].NewValue).EndsWith("?"));
                Assert.IsFalse(((string)changes[0].OldValue).EndsWith("?"));
            }
        }

        [TestMethod]
        public void ChangeTrackingUsingProxyReference1()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                List<ChangeDetectedEventArgs> changes = new List<ChangeDetectedEventArgs>();

                var o = context.Orders.First();
                o.Customer = context.Customers.First();
            }
        }
    }
}
