// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EFPocoAdapter;
using System.Data.Metadata.Edm;
using System.Data.Objects;
using NorthwindEF.PocoAdapters;

namespace NorthwindEF.Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class UnitTest1
    {
        public UnitTest1()
        {
        }

        [TestMethod]
        public void SimpleLoad1()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                var cust = context.Customers.First(c => c.CustomerID == "ALFKI");
                Assert.IsNotNull(cust);
                Assert.IsFalse(context.IsPropertyLoaded(cust, c => c.Orders));
                Assert.AreEqual(6, cust.Orders.Count);
                Assert.AreEqual(cust.CompanyName, "Alfreds Futterkiste");
            }
        }

        [TestMethod]
        public void CreateObject()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                var cust = context.CreateObject<Customer>();
                Assert.IsTrue(context.IsProxy(cust));
                Assert.AreEqual("NorthwindEF.PocoProxies.CustomerProxy", cust.GetType().FullName);
            }
        }

        [TestMethod]
        public void MetadataWorkspaceTest()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                var mw = context.MetadataWorkspace;
                Assert.IsNotNull(mw);
                var customerEntityType = mw.GetItem<EntityType>("NorthwindEFModel.Customer", DataSpace.CSpace);
                Assert.IsNotNull(customerEntityType);
            }
        }

        [TestMethod]
        public void WrappedContextTests()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                var adapterCat = context.WrappedContext.Categories.First(c => c.CategoryID == 1);
                var pocoCat = context.Categories.First(c => c.CategoryID == 1);
                Assert.AreSame(adapterCat.PocoEntity, pocoCat);
            }
        }

        [TestMethod]
        public void UnattachedOperationsOnAdapters()
        {
            using (NorthwindEntitiesAdapter adapterContext = new NorthwindEntitiesAdapter())
            {
                var ca = new CustomerAdapter();
                var order = new OrderAdapter();
                Assert.IsNull(order.Customer);
                order.Customer = ca;
                Assert.AreSame(ca, order.Customer);
                order.Customer = null;
                Assert.IsNull(order.Customer);

                ca.Orders.Add(order);
                Assert.AreSame(ca, order.Customer);
                ca.Orders.Remove(order);
                Assert.IsNull(order.Customer);

                var prod = new ProductAdapter();
                var cat = new CategoryAdapter();
                Assert.IsNull(prod.Category);
                prod.Category = cat;
                Assert.AreSame(cat, prod.Category);
                cat.Products.Remove(prod);
                Assert.IsNull(prod.Category);
                cat.Products.Add(prod);
                Assert.AreSame(cat, prod.Category);
                prod.Category = null;
                Assert.IsNull(prod.Category);
            }
        }

        [TestMethod]
        public void ConnectionTest()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                Assert.AreSame(context.Connection, context.WrappedContext.Connection);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PersistenceAwareObjectNegativeTest()
        {
            using (EFPocoContext context = new MyContext())
            {
            }
        }

        internal class MyContext : EFPocoContext
        {
            public MyContext()
            {
                PersistenceAwareContext = new ObjectContext("name=NorthwindEntities");
            }
        }
    }
}
