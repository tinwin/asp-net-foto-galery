// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Transactions;
using System.Data.EntityClient;

using EFPocoAdapter;

namespace NorthwindEF.Tests
{
    [TestClass]
    public class BugRegressionTests
    {
        [TestMethod]
        public void GetAdapterObjectDoesNotWorkForProxiesBug()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                var customerProxy = context.CreateObject<Customer>();
                Assert.IsTrue(context.IsProxy(customerProxy));
                var adapterObject = context.GetAdapterObject(customerProxy);
                Assert.AreSame(customerProxy, adapterObject.PocoEntity);
            }
        }

        [TestMethod]
        public void LazyLoadingShoundBeDisabledForAddedEntitites()
        {
            using (new TransactionScope())
            {
                using (EntityConnection conn = new EntityConnection("name=NorthwindEntities"))
                {
                    conn.Open();

                    int orderID;

                    using (NorthwindEntities context = new NorthwindEntities(conn))
                    {
                        var customerProxy = context.CreateObject<Customer>();
                        var orderProxy = context.CreateObject<Order>();

                        customerProxy.CustomerID = "XXXXX";
                        customerProxy.CompanyName = "Foo";

                        context.Customers.InsertOnSaveChanges(customerProxy);
                        //context.Orders.InsertOnSaveChanges(orderProxy);

                        Assert.IsTrue(context.IsProxy(customerProxy));
                        Assert.IsTrue(context.IsProxy(orderProxy));
                        Assert.IsNull(orderProxy.Customer);
                        Assert.AreEqual(0, customerProxy.Orders.Count);

                        orderProxy.Customer = customerProxy;
                        customerProxy.Orders.Add(orderProxy);

                        Assert.AreSame(customerProxy, orderProxy.Customer);
                        Assert.AreEqual(1, customerProxy.Orders.Count);
                        Assert.AreSame(orderProxy, customerProxy.Orders[0]);

                        context.SaveChanges();

                        orderID = orderProxy.OrderID;
                    }
                    using (NorthwindEntities context = new NorthwindEntities(conn))
                    {
                        var cust = context.Customers.Include(c=>c.Orders).First(c => c.CustomerID == "XXXXX");
                        Assert.AreEqual("XXXXX", cust.CustomerID);
                        Assert.AreEqual("Foo", cust.CompanyName);
                        Assert.AreEqual(1, cust.Orders.Count);
                        Assert.AreEqual(orderID, cust.Orders[0].OrderID);
                        Assert.AreSame(cust, cust.Orders[0].Customer);
                        Assert.IsTrue(context.IsProxy(cust));
                    }
                }
            }
        }
    }
}
