// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.EntityClient;
using System.Transactions;
using NorthwindEF.PocoAdapters;
using EFPocoAdapter.DataClasses;
using EFPocoAdapter;

namespace NorthwindEF.Tests
{
    [TestClass]
    public class CollectionPropertiesTests
    {
        public CollectionPropertiesTests()
        {
        }

        [TestMethod]
        public void ClearTest()
        {
            using (new TransactionScope())
            {
                using (EntityConnection conn = new EntityConnection("name=NorthwindEntities"))
                {
                    conn.Open();
                    using (NorthwindEntities context = new NorthwindEntities(conn))
                    {
                        var cust = context.Customers.First(c => c.CustomerID == "ALFKI");
                        var oldOrders = cust.Orders.ToArray();
                        Assert.IsTrue(oldOrders.Length > 0);
                        foreach (var o in oldOrders)
                        {
                            Assert.IsNotNull(o.Customer);
                        }
                        Assert.IsTrue(cust.Orders is IListEntityCollectionAdapter<Order, OrderAdapter>);
                        cust.Orders.Clear();
                        foreach (var o in oldOrders)
                        {
                            Assert.IsNull(o.Customer);
                        }
                        context.SaveChanges();
                    }
                    using (NorthwindEntities context = new NorthwindEntities(conn))
                    {
                        var cust = context.Customers.First(c => c.CustomerID == "ALFKI");
                        Assert.AreEqual(0, cust.Orders.Count);
                    }
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void IndexerNegativeTest1()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                var cust = context.Customers.First(c => c.CustomerID == "ALFKI");
                var o = cust.Orders[-1];
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void IndexerNegativeTest2()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                var cust = context.Customers.First(c => c.CustomerID == "ALFKI");
                var o = cust.Orders[cust.Orders.Count];
            }
        }

        [TestMethod]
        public void IndexerTest()
        {
            using (new TransactionScope())
            {
                using (EntityConnection conn = new EntityConnection("name=NorthwindEntities"))
                {
                    conn.Open();
                    using (NorthwindEntities context = new NorthwindEntities(conn))
                    {
                        var cust = context.Customers.First(c => c.CustomerID == "ALFKI");
                        var oldOrders = cust.Orders.ToArray();

                        for (int i = 0; i < cust.Orders.Count; ++i)
                        {
                            var ord = cust.Orders[i];
                            cust.Orders[i] = new Order() { };
                            cust.Orders[i] = ord;
                            cust.Orders[i] = cust.Orders[i];
                        }
                        var newOrders = cust.Orders.ToArray();
                        Assert.AreEqual(oldOrders.Length, newOrders.Length);
                        for (int i = 0; i < oldOrders.Length; ++i)
                            Assert.AreSame(oldOrders[i], newOrders[i]);

                        context.SaveChanges();
                    }
                }
            }
        }

        [TestMethod]
        public void IsReadOnlyTest()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                var cust = context.Customers.First(c => c.CustomerID == "ALFKI");
                Assert.IsFalse(cust.Orders.IsReadOnly);
            }
        }

        [TestMethod]
        public void RemoveAtTest()
        {
            using (new TransactionScope())
            {
                using (EntityConnection conn = new EntityConnection("name=NorthwindEntities"))
                {
                    conn.Open();
                    using (NorthwindEntities context = new NorthwindEntities(conn))
                    {
                        var cust = context.Customers.First(c => c.CustomerID == "ALFKI");
                        var oldOrders = cust.Orders.ToArray();
                        cust.Orders.RemoveAt(0);
                        var newOrders = cust.Orders.ToArray();
                        context.SaveChanges();

                        Assert.AreEqual(oldOrders.Length - 1, newOrders.Length);
                        for (int i = 0; i < newOrders.Length; ++i)
                            Assert.AreSame(oldOrders[i + 1], newOrders[i]);
                    }
                }
            }
        }

        [TestMethod]
        public void RemoveTest()
        {
            using (new TransactionScope())
            {
                using (EntityConnection conn = new EntityConnection("name=NorthwindEntities"))
                {
                    conn.Open();
                    using (NorthwindEntities context = new NorthwindEntities(conn))
                    {
                        var cust = context.Customers.First(c => c.CustomerID == "ALFKI");
                        var oldOrders = cust.Orders.ToArray();
                        cust.Orders.Remove(cust.Orders.First());
                        var newOrders = cust.Orders.ToArray();
                        context.SaveChanges();

                        Assert.AreEqual(oldOrders.Length - 1, newOrders.Length);
                        for (int i = 0; i < newOrders.Length; ++i)
                            Assert.AreSame(oldOrders[i + 1], newOrders[i]);
                    }
                }
            }
        }

        [TestMethod]
        public void InsertTest()
        {
            using (new TransactionScope())
            {
                using (EntityConnection conn = new EntityConnection("name=NorthwindEntities"))
                {
                    conn.Open();
                    int newOrderID;
                    using (NorthwindEntities context = new NorthwindEntities(conn))
                    {
                        Order newOrder = new Order();
                        var cust = context.Customers.First(c => c.CustomerID == "ALFKI");
                        cust.Orders.Insert(0, newOrder);
                        context.SaveChanges();
                        newOrderID = newOrder.OrderID;
                    }

                    using (NorthwindEntities context = new NorthwindEntities(conn))
                    {
                        Order newOrder = context.Orders.Include("Customer").First(c => c.OrderID == newOrderID);
                        Assert.AreEqual("ALFKI", newOrder.Customer.CustomerID);
                    }
                }
            }
        }

        [TestMethod]
        public void IndexOfTest()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                var cust = context.Customers.First(c => c.CustomerID == "ALFKI");
                for (int i = 0; i < cust.Orders.Count; ++i)
                    Assert.AreEqual(i, cust.Orders.IndexOf(cust.Orders[i]));
                Assert.AreEqual(-1, cust.Orders.IndexOf(null));
                Assert.AreEqual(-1, cust.Orders.IndexOf(new Order()));
            }
        }
    }
}
