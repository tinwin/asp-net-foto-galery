// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Common;

namespace NorthwindEF.Tests
{
    [TestClass]
    public class BuilderMethodsTests
    {
        public BuilderMethodsTests()
        {
        }

        [TestMethod]
        public void DistinctTest()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                var cust = context.CreateQuery<Customer>("Customers").Distinct().First(c=>c.CustomerID == "ALFKI");
                Assert.AreEqual(cust.CompanyName, "Alfreds Futterkiste");
            }
        }

        [TestMethod]
        public void ExceptTest()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                var cust = context.CreateQuery<Customer>("Customers").Except(context.CreateQuery<Customer>("Customers").Where("it.CustomerID = 'ALFKI'")).First();
                Assert.AreEqual("ANATR", cust.CustomerID);
            }
        }

        [TestMethod]
        public void GroupByTest()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                var cust = context.CreateQuery<Customer>("Customers").GroupBy("it.Address.City", "it.Address.City AS City, COUNT(it.CustomerID) AS CustomerCount").Where("it.City = 'London'");
                var record = cust.ToArray().Single();
                Assert.AreEqual("London", record["City"]);
                Assert.AreEqual(6, record["CustomerCount"]);
            }
        }

        [TestMethod]
        public void IntersectTest()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                var q1 = context.CreateQuery<Customer>("Customers").Where("it.Address.City = 'London'");
                var q2 = context.CreateQuery<Customer>("Customers").Where("it.CompanyName LIKE 'A%'");
                var result1 = q1.ToArray();
                var result2 = q2.ToArray();
                var intersectResult = q1.Intersect(q2).ToArray();
                Assert.AreEqual(6, result1.Length);
                Assert.AreEqual(4, result2.Length);
                Assert.AreEqual(1, intersectResult.Length);
                Assert.AreEqual("AROUT", intersectResult[0].CustomerID);
            }
        }

        [TestMethod]
        public void OfTypeTest()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                var internationalOrders = context.CreateQuery<Order>("Orders").OfType<InternationalOrder>();
                foreach (var o in internationalOrders)
                {
                    Assert.IsInstanceOfType(o, typeof(InternationalOrder));
                }
            }
        }

        [TestMethod]
        public void OrderByTest()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                var orderedCustomers = context.CreateQuery<Customer>("Customers").OrderBy("it.Address.City").ToArray();
                var first = orderedCustomers.First();
                var last = orderedCustomers.Last();
                Assert.AreEqual("Aachen", first.Address.City);
                Assert.AreEqual("Warszawa", last.Address.City);
            }
        }

        [TestMethod]
        public void SelectTest()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                DbDataRecord[] results = context.CreateQuery<Customer>("Customers").Where("it.CustomerID = 'ALFKI'").Select("it.Address.City as City").ToArray();
                Assert.AreEqual(1, results.Length);
                Assert.AreEqual("Berlin", results[0]["City"]);
            }
        }

        [TestMethod]
        public void SelectValueTest()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                string[] results = context.CreateQuery<Customer>("Customers").Where("it.CustomerID = 'WOLZA'").SelectValue<string>("it.Address.City").ToArray();
                Assert.AreEqual(1, results.Length);
                Assert.AreEqual("Warszawa", results[0]);
            }
        }

        [TestMethod]
        public void SkipTest()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                var orderedCustomers = context.CreateQuery<Customer>("Customers").Skip("it.Address.City", "3").ToArray();
                Assert.AreEqual("VAFFE", orderedCustomers[0].CustomerID);
            }
        }

        [TestMethod]
        public void TopTest()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                var orderedCustomers = context.CreateQuery<Customer>("Customers").OrderBy("it.Address.City").Top("3").ToArray();
                Assert.AreEqual(3, orderedCustomers.Length);
                Assert.AreEqual("DRACD", orderedCustomers[0].CustomerID);
                Assert.AreEqual("RATTC", orderedCustomers[1].CustomerID);
                Assert.AreEqual("OLDWO", orderedCustomers[2].CustomerID);
            }
        }

        [TestMethod]
        public void UnionTest()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                var q1 = context.CreateQuery<Customer>("Customers").Where("it.Address.City = 'London'");
                var q2 = context.CreateQuery<Customer>("Customers").Where("it.CompanyName LIKE 'A%'");
                var result1 = q1.ToArray();
                var result2 = q2.ToArray();
                var unionResult = q1.Union(q2).ToArray();
                Assert.AreEqual(6, result1.Length);
                Assert.AreEqual(4, result2.Length);
                Assert.AreEqual(9, unionResult.Length);
            }
        }

        [TestMethod]
        public void UnionAllTest()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                var q1 = context.CreateQuery<Customer>("Customers").Where("it.Address.City = 'London'");
                var q2 = context.CreateQuery<Customer>("Customers").Where("it.CompanyName LIKE 'A%'");
                var result1 = q1.ToArray();
                var result2 = q2.ToArray();
                var unionAllResult = q1.UnionAll(q2).ToArray();
                Assert.AreEqual(6, result1.Length);
                Assert.AreEqual(4, result2.Length);
                Assert.AreEqual(10, unionAllResult.Length);
            }
        }

        [TestMethod]
        public void WhereTest()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                var cust = context.CreateQuery<Customer>("Customers").Where("it.CustomerID == 'ALFKI'").First();
                Assert.AreEqual(cust.CompanyName, "Alfreds Futterkiste");
            }
        }
    }
}
