// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EFPocoAdapter;
using NorthwindEF.Territories;
using System.IO;
using System.Data.EntityClient;
using NorthwindEF.PocoAdapters;

namespace NorthwindEF.Tests
{
    [TestClass]
    public class EFPocoContextFactoryTests
    {
        class MyHandWrittenNorthwindEntities : EFPocoContext
        {
            public IEntitySet<Customer> Customers
            {
                get { return GetEntitySet<Customer>("Customers"); }
            }
        }

        class MyOtherHandWrittenNorthwindEntities : EFPocoContext
        {
            public IEntitySet<Product> Products
            {
                get { return GetEntitySet<Product>("Products"); }
            }
        }

        public EFPocoContextFactoryTests()
        {
        }

        [TestMethod]
        public void EFPocoContextFactoryTest1()
        {
            EFPocoContextFactory<MyHandWrittenNorthwindEntities> factory = new EFPocoContextFactory<MyHandWrittenNorthwindEntities>(typeof(NorthwindEntitiesAdapter));
            var context = factory.CreateContext();
            Assert.IsInstanceOfType(context, typeof(MyHandWrittenNorthwindEntities));
            Assert.IsNotNull(context.Customers.First(c => c.CustomerID == "ALFKI"));
        }

        [TestMethod]
        public void EFPocoContextFactoryTest2()
        {
            EFPocoContextFactory<MyHandWrittenNorthwindEntities> factory = new EFPocoContextFactory<MyHandWrittenNorthwindEntities>(typeof(NorthwindEntitiesAdapter));
            var context = factory.CreateContext("name=NorthwindEntities");
            Assert.IsInstanceOfType(context, typeof(MyHandWrittenNorthwindEntities));
            Assert.IsNotNull(context.Customers.First(c => c.CustomerID == "ALFKI"));
        }

        [TestMethod]
        public void EFPocoContextFactoryTest3()
        {
            EFPocoContextFactory<MyHandWrittenNorthwindEntities> factory = new EFPocoContextFactory<MyHandWrittenNorthwindEntities>(typeof(NorthwindEntitiesAdapter));
            using (EntityConnection entityConnection = new EntityConnection("name=NorthwindEntities"))
            {
                var context = factory.CreateContext(entityConnection);
                Assert.IsInstanceOfType(context, typeof(MyHandWrittenNorthwindEntities));
                Assert.IsNotNull(context.Customers.First(c => c.CustomerID == "ALFKI"));
            }
        }

        [TestMethod]
        public void EFPocoContextFactoryTest4()
        {
            EFPocoContextFactory<MyOtherHandWrittenNorthwindEntities> factory = new EFPocoContextFactory<MyOtherHandWrittenNorthwindEntities>(typeof(NorthwindEntitiesAdapter));
            using (EntityConnection entityConnection = new EntityConnection("name=NorthwindEntities"))
            {
                var context = factory.CreateContext(entityConnection);
                Assert.IsInstanceOfType(context, typeof(MyOtherHandWrittenNorthwindEntities));
                Assert.AreEqual("Chai", context.Products.First(c => c.ProductID == 1).ProductName);
            }
        }
    }
}
