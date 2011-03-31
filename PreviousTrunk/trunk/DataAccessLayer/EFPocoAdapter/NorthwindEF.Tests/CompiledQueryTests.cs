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
    public class CompiledQueryTests
    {
        public CompiledQueryTests()
        {
        }

        [TestMethod]
        public void CompiledQueryTest0()
        {
            var getALFKI = EFPocoCompiledQuery.Compile((NorthwindEntities context) => context.Customers.Where(c => c.CustomerID == "ALFKI").First());
            var getWOLZA = EFPocoCompiledQuery.Compile((NorthwindEntities context) => context.Customers.Where(c => c.CustomerID == "WOLZA").First());

            using (NorthwindEntities context = new NorthwindEntities())
            {
                Assert.AreEqual("ALFKI", getALFKI(context).CustomerID);
                Assert.AreEqual("WOLZA", getWOLZA(context).CustomerID);
            }
        }

        [TestMethod]
        public void CompiledQueryTest1()
        {
            var getCustomerByID = EFPocoCompiledQuery.Compile((NorthwindEntities context, string customerID) => context.Customers.Where(c => c.CustomerID == customerID).First());

            using (NorthwindEntities context = new NorthwindEntities())
            {
                Assert.AreEqual("ALFKI", getCustomerByID(context, "ALFKI").CustomerID);
                Assert.AreEqual("WOLZA", getCustomerByID(context, "WOLZA").CustomerID);
            }
        }

        [TestMethod]
        public void CompiledQueryTest2()
        {
            var getCustomerByID = EFPocoCompiledQuery.Compile((NorthwindEntities context, string customerID, string city) => context.Customers.Where(c => c.CustomerID == customerID && c.Address.City == city).First());

            using (NorthwindEntities context = new NorthwindEntities())
            {
                Assert.AreEqual("ALFKI", getCustomerByID(context, "ALFKI", "Berlin").CustomerID);
                Assert.AreEqual("WOLZA", getCustomerByID(context, "WOLZA", "Warszawa").CustomerID);
            }
        }

        [TestMethod]
        public void CompiledQueryTest3()
        {
            var getCustomerByID = EFPocoCompiledQuery.Compile((NorthwindEntities context, string customerID, string city, string country) => context.Customers.Where(c => c.CustomerID == customerID && c.Address.City == city && c.Address.Country == country).First());

            using (NorthwindEntities context = new NorthwindEntities())
            {
                Assert.AreEqual("ALFKI", getCustomerByID(context, "ALFKI", "Berlin", "Germany").CustomerID);
                Assert.AreEqual("WOLZA", getCustomerByID(context, "WOLZA", "Warszawa", "Poland").CustomerID);
            }
        }
    }
}
