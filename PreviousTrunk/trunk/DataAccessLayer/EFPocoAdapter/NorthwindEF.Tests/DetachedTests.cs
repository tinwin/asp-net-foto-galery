// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.Transactions;
using System.Data.EntityClient;
using System.IO;
using System.Data;
using EFPocoAdapter;
using System.Data.Objects;

namespace NorthwindEF.Tests
{
    [TestClass]
    public class DetachedTests
    {
        public DetachedTests()
        {
        }

        [TestMethod]
        public void ApplyPropertyChangesTest()
        {
            using (TransactionScope tran = new TransactionScope())
            {
                using (EntityConnection conn = new EntityConnection("name=NorthwindEntities"))
                {
                    conn.Open();

                    Customer customer1, customer2;

                    using (NorthwindEntities context = new NorthwindEntities(conn))
                    {
                        context.EnableChangeTrackingUsingProxies = false;
                        customer1 = context.Customers.First(c => c.CustomerID == "ALFKI");
                        customer2 = new Customer()
                        {
                            CustomerID = customer1.CustomerID,
                            ContactPerson = customer1.ContactPerson,
                            CompanyName = customer1.CompanyName,
                            Fax = "555-1234",
                            Orders = new List<Order>(customer1.Orders),
                            Phone = customer1.Phone,
                            Address = customer1.Address,
                        };
                        Assert.AreNotEqual(customer1.Fax, "555-1234");
                    }

                    using (NorthwindEntities context = new NorthwindEntities(conn))
                    {
                        context.Customers.Attach(customer1);
                        Assert.AreNotEqual(customer1.Fax, "555-1234");
                        context.Customers.ApplyPropertyChanges(customer2);
                        Assert.AreEqual(customer1.Fax, "555-1234");
                        context.SaveChanges();
                    }

                    using (NorthwindEntities context = new NorthwindEntities(conn))
                    {
                        var cust  = context.Customers.First(c => c.CustomerID == "ALFKI");
                        Assert.AreEqual(cust.Fax, "555-1234");
                    }
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CannotAttachProxyToMultipleActiveContextsTest()
        {
            Customer customer;

            using (NorthwindEntities context = new NorthwindEntities())
            {
                customer = context.Customers.First();
                Assert.IsTrue(context.IsProxy(customer));

                using (NorthwindEntities context2 = new NorthwindEntities())
                {
                    context2.Customers.Attach(customer);
                }
            }
        }

        [TestMethod]
        public void CanAttachDetachedProxyToMultipleActiveContextsTest()
        {
            Customer customer;

            using (NorthwindEntities context = new NorthwindEntities())
            {
                customer = context.Customers.First();
                Assert.IsTrue(context.IsProxy(customer));
                context.Customers.Detach(customer);

                using (NorthwindEntities context2 = new NorthwindEntities())
                {
                    context2.Customers.Attach(customer);
                }
            }
        }

        [TestMethod]
        public void CanAttachProxyToAnotherContextTest()
        {
            Customer customer;

            using (NorthwindEntities context = new NorthwindEntities())
            {
                customer = context.Customers.First();
                Assert.IsTrue(context.IsProxy(customer));
            }

            using (NorthwindEntities context2 = new NorthwindEntities())
            {
                context2.Customers.Attach(customer);
            }
        }

        [TestMethod]
        public void CanAttachDetachedProxyToAnotherContextTest()
        {
            Customer customer;

            using (NorthwindEntities context = new NorthwindEntities())
            {
                customer = context.Customers.First();
                Assert.IsTrue(context.IsProxy(customer));
                context.Customers.Detach(customer);
            }

            using (NorthwindEntities context2 = new NorthwindEntities())
            {
                context2.Customers.Attach(customer);
            }
        }

        [TestMethod]
        public void CanAttachNonProxyToMultipleActiveContextsTest()
        {
            Customer customer;

            using (NorthwindEntities context = new NorthwindEntities())
            {
                context.EnableChangeTrackingUsingProxies = false;
                customer = context.Customers.First();
                Assert.IsFalse(context.IsProxy(customer));

                using (NorthwindEntities context2 = new NorthwindEntities())
                {
                    context2.Customers.Attach(customer);
                }
            }
        }

        [TestMethod]
        public void CanAttachNonProxyToAnotherContextTest()
        {
            Customer customer;

            using (NorthwindEntities context = new NorthwindEntities())
            {
                context.EnableChangeTrackingUsingProxies = false;
                customer = context.Customers.First();
                Assert.IsFalse(context.IsProxy(customer));
            }

            using (NorthwindEntities context2 = new NorthwindEntities())
            {
                context2.Customers.Attach(customer);
            }
        }

        [TestMethod]
        public void TestLoadEditSaveChildObjectsWithoutWCFTransfer()
        {
            Customer cust;
            Customer original;

            // server-side call #1
            using (NorthwindEntities context = new NorthwindEntities())
            {
                context.EnableChangeTrackingUsingProxies = false;
                context.DeferredLoadingEnabled = false;
                cust = context.Customers.Include(c => c.Orders).First(c => c.CustomerID == "ALFKI");
                original = SerializeAndDeserialize(cust);
            }

            // client-side operations:

            // change a scalar property
            cust.Fax = "newFax";

            // change complex property
            cust.ContactPerson = new ContactInfo()
            {
                Name = "newName1",
                Title = cust.ContactPerson.Title,
            };

            // create new order
            Order order = new Order() { OrderDate = new DateTime(2008, 1, 1) };
            cust.Orders.Add(order);

            // modify one of the existing orders
            cust.Orders[0].ShipName = "mynewShipName1";

            // server-side call #2 passes "original" and "cust"
            // run everything in a transaction that never gets committed
            using (new TransactionScope())
            {
                using (EntityConnection ec = new EntityConnection("name=NorthwindEntities"))
                {
                    ec.Open();

                    using (NorthwindEntities context = new NorthwindEntities(ec))
                    {
                        // attach original customer and orders
                        context.Customers.Attach(original);

                        // apply changes to the customer
                        AssertOSM(context, EntityState.Unchanged, 7, 6);
                        AssertOSM(context, EntityState.Modified, 0, 0);
                        context.Customers.ApplyPropertyChanges(cust);
                        AssertOSM(context, EntityState.Unchanged, 6, 6);
                        AssertOSM(context, EntityState.Modified, 1, 0);
                        AssertOSM(context, EntityState.Added, 0, 0);
                        AssertOSM(context, EntityState.Deleted, 0, 0);

                        // apply changes to existing orders 
                        // add new orders
                        foreach (Order o in cust.Orders)
                        {
                            if (o.OrderID == 0)
                            {
                                o.Customer = original;
                                original.Orders.Add(o); //add newly created orders
                            }
                            else
                            {
                                // apply property changes for existing ones
                                context.Orders.ApplyPropertyChanges(o);
                            }
                        }

                        context.SaveChanges();
                    }

                    using (NorthwindEntities context = new NorthwindEntities(ec))
                    {
                        Customer c2 = context.Customers.Include("Orders").First(c => c.CustomerID == "ALFKI");
                        
                        // make sure scalar type property has been updated
                        Assert.AreEqual("newFax", c2.Fax);

                        // make sure we have new order
                        Assert.AreEqual(7, c2.Orders.Count);
                        Assert.IsTrue(c2.Orders.Any(o => o.OrderDate == new DateTime(2008, 1, 1)));

                        // make sure order property has been updated
                        Assert.IsTrue(c2.Orders.Any(o => o.ShipName == "mynewShipName1"));

                        // make sure complex type property has been updated
                        Assert.AreEqual("newName1", c2.ContactPerson.Name);
                    }
                }
            }
        }

        private void AssertOSM(NorthwindEntities context, EntityState state, int entityCount, int relationshipCount)
        {
            var entries = context.WrappedContext.ObjectStateManager.GetObjectStateEntries(state);
            try
            {
                Assert.AreEqual(relationshipCount, entries.Where(c => c.IsRelationship).Count(), "Invalid relationship count");
                Assert.AreEqual(entityCount, entries.Where(c => !c.IsRelationship).Count(), "Invalid entity count");
            }
            catch (Exception)
            {
                int counter = 0;
                foreach (ObjectStateEntry se in entries)
                {
                    Console.WriteLine("{0} state={1} key={2} set={3} relationship={4}", counter++, se.State, se.EntityKey, se.EntitySet, se.IsRelationship ? "yes" : "no");
                }
                throw;
            }
        }

        private T SerializeAndDeserialize<T>(T t)
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, t);
            ms.Position = 0;
            return (T)bf.Deserialize(ms);
        }

    }
}
