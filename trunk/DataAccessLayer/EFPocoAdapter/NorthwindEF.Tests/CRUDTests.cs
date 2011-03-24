// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using EFPocoAdapter;
using System.Transactions;
using System.Data.EntityClient;
using System.Data.Objects.DataClasses;

namespace NorthwindEF.Tests
{
    [TestClass]
    public class CRUDTests
    {
        public CRUDTests()
        {
        }

        [TestMethod]
        public void CRUD1()
        {
            using (TransactionScope tran = new TransactionScope())
            {
                using (EntityConnection conn = new EntityConnection("name=NorthwindEntities"))
                {
                    conn.Open();


                    using (NorthwindEntities context = new NorthwindEntities(conn))
                    {
                        Customer newCustomer = new Customer();
                        newCustomer.CustomerID = "XXXX";
                        newCustomer.ContactPerson = new ContactInfo { Name = "Foo" };
                        newCustomer.CompanyName = "Una Firma";
                        context.Customers.InsertOnSaveChanges(newCustomer);
                        context.SaveChanges();
                    }

                    using (NorthwindEntities context = new NorthwindEntities(conn))
                    {
                        var cust = context.Customers.First(c => c.CustomerID == "XXXX");
                        Assert.AreEqual("Foo", cust.ContactPerson.Name);
                        Assert.AreEqual("Una Firma", cust.CompanyName);
                        cust.ContactPerson = new ContactInfo { Name = "Bar", Title = "Sir" };
                        context.SaveChanges();
                    }

                    using (NorthwindEntities context = new NorthwindEntities(conn))
                    {
                        var cust = context.Customers.First(c => c.CustomerID == "XXXX");
                        Assert.AreEqual("Bar", cust.ContactPerson.Name);
                        Assert.AreEqual("Sir", cust.ContactPerson.Title);
                        context.Customers.DeleteOnSaveChanges(cust);
                        context.SaveChanges();
                    }

                    using (NorthwindEntities context = new NorthwindEntities(conn))
                    {
                        var cust = context.Customers.FirstOrDefault(c => c.CustomerID == "XXXX");
                        Assert.IsNull(cust);
                    }
                }
            }
        }

        [TestMethod]
        public void CRUD2()
        {
            using (TransactionScope tran = new TransactionScope())
            {
                using (EntityConnection conn = new EntityConnection("name=NorthwindEntities"))
                {
                    conn.Open();

                    int orderID;

                    using (NorthwindEntities context = new NorthwindEntities(conn))
                    {
                        context.ChangeDetected += new EventHandler<ChangeDetectedEventArgs>(context_ChangeDetected);
                        context.CollectionChangeDetected += new EventHandler<CollectionChangeDetectedEventArgs>(context_CollectionChangeDetected);
                        Customer alfki = context.Customers.First(c => c.CustomerID == "ALFKI");
                        Order newOrder = new Order();
                        newOrder.OrderDetails = new List<OrderDetail>();
                        alfki.Orders.Add(newOrder);

                        foreach (var prod in context.Products.Take(3))
                        {
                            OrderDetail det = new OrderDetail()
                            {
                                Order = newOrder,
                                Product = prod,
                                Quantity = (short)((prod.ProductID % 3) + 1),
                                UnitPrice = 3.1415m,
                                Discount = 0.0f
                            };
                            newOrder.OrderDetails.Add(det);
                        }
                        context.SaveChanges();
                        orderID = newOrder.OrderID;
                    }
                    using (NorthwindEntities context = new NorthwindEntities(conn))
                    {
                        var order = context.Orders.Include("OrderDetails").Include("OrderDetails.Product").First(c => c.OrderID == orderID);
                        Assert.AreEqual(3, order.OrderDetails.Count);

                        foreach (var det in order.OrderDetails)
                        {
                            Assert.AreEqual((short)((det.Product.ProductID % 3) + 1), det.Quantity);
                            Assert.AreEqual(3.1415m, det.UnitPrice);
                            Assert.AreEqual(0.0f, det.Discount);
                            Assert.AreSame(order, det.Order);
                        }

                        context.Orders.DeleteOnSaveChanges(order);
                        Assert.AreEqual(0, order.OrderDetails.Count); // make sure we get cascade delete
                        context.SaveChanges();
                    }

                    using (NorthwindEntities context = new NorthwindEntities(conn))
                    {
                        var order = context.Orders.Where(c => c.OrderID == orderID).FirstOrDefault();
                        Assert.IsNull(order);
                    }
                }
            }
        }

        void context_CollectionChangeDetected(object sender, CollectionChangeDetectedEventArgs e)
        {
            // Console.WriteLine("Collection change: {0}.{1} Added: {2} Removed: {3}", e.Object, e.Member, e.AddedObjects.Count, e.RemovedObjects.Count);
        }

        void context_ChangeDetected(object sender, ChangeDetectedEventArgs e)
        {
            // Console.WriteLine("Change: {0}.{1} from {2} to {3}", e.Object, e.Member, e.OldValue, e.NewValue);
        }
    }
}
