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
    public class LazyLoadingTests
    {
        public LazyLoadingTests()
        {
        }

        // Customer.Orders and Order.Customer are loaded on demand

        [TestMethod]
        public void LazyLoading1()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                var order = context.Orders.First(c => c.OrderID == 10248);
                Assert.IsNotNull(order.Customer);
                Assert.AreEqual("VINET", order.Customer.CustomerID);
            }
        }

        [TestMethod]
        public void LazyLoading2()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                var cust = context.Customers.First(c => c.CustomerID == "ALFKI");
                Assert.AreEqual(6, cust.Orders.Count);
            }
        }

        [TestMethod]
        public void ExplicitLoading1()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                var prod = context.Products.First(c => c.ProductID == 1);
                Assert.IsNull(prod.Category);
                Assert.IsFalse(context.IsPropertyLoaded(prod, c => c.Category));
                context.LoadProperty(prod, c => c.Category);
                Assert.IsNotNull(prod.Category);
                Assert.AreEqual("Beverages", prod.Category.CategoryName);
                Assert.IsTrue(context.IsPropertyLoaded(prod, c => c.Category));
            }
        }

        [TestMethod]
        public void ExplicitLoading2()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                var prod = context.Products.First(c => c.ProductID == 1);
                Assert.IsNull(prod.Category);
                Assert.IsFalse(context.IsPropertyLoaded(prod, "Category"));
                context.LoadProperty(prod, "Category");
                Assert.IsNotNull(prod.Category);
                Assert.AreEqual("Beverages", prod.Category.CategoryName);
                Assert.IsTrue(context.IsPropertyLoaded(prod, "Category"));
            }
        }

        [TestMethod]
        public void ExplicitLoading3()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                var category = context.Categories.First(c=>c.CategoryID == 1);
                Assert.IsNotNull(category.Products);
                Assert.AreEqual(0, category.Products.Count);
                Assert.IsFalse(context.IsPropertyLoaded(category, c => c.Products));
                context.LoadProperty(category, c => c.Products);
                Assert.IsTrue(context.IsPropertyLoaded(category, c => c.Products));
                Assert.AreEqual(12, category.Products.Count);
            }
        }

        [TestMethod]
        public void ExplicitLoading4()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                var category = context.Categories.First(c => c.CategoryID == 1);
                Assert.IsNotNull(category.Products);
                Assert.AreEqual(0, category.Products.Count);
                Assert.IsFalse(context.IsPropertyLoaded(category, "Products"));
                context.LoadProperty(category, "Products");
                Assert.IsTrue(context.IsPropertyLoaded(category, "Products"));
                Assert.AreEqual(12, category.Products.Count);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ExplicitLoadingNegative1()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                var category = context.Categories.First(c => c.CategoryID == 1);
                context.IsPropertyLoaded(category, "SomePropertyThatIsNotDefined");
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ExplicitLoadingNegative2()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                var category = context.Categories.First(c => c.CategoryID == 1);
                context.IsPropertyLoaded(category, c => c.Picture);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ExplicitLoadingNegative3()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                var category = context.Categories.First(c => c.CategoryID == 1);
                context.IsPropertyLoaded(category, c => c.Products.Count);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ExplicitLoadingNegative4()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                var category = context.Categories.First(c => c.CategoryID == 1);
                context.IsPropertyLoaded(category, c => DateTime.Now);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ExplicitLoadingNegative5()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                var category = context.Categories.First(c => c.CategoryID == 1);
                context.IsPropertyLoaded(category, c => "AAA");
            }
        }

        [TestMethod]
        public void LazyLoadingShoundBeDisabledForDetachedEntitites()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                var customerProxy = context.CreateObject<Customer>();
                var orderProxy = context.CreateObject<Order>();

                Assert.IsTrue(context.IsProxy(customerProxy));
                Assert.IsTrue(context.IsProxy(orderProxy));
                Assert.IsNull(orderProxy.Customer);
                Assert.AreEqual(0, customerProxy.Orders.Count);
            }
        }

        [TestMethod]
        public void LazyLoadingShoundBeDisabledForDetachedEntitites2()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                Customer cust1 = context.Customers.First(c => c.CustomerID == "ALFKI");
                Customer cust2 = context.Customers.First(c => c.CustomerID == "WHITC");
                context.Customers.Detach(cust1);
                Assert.AreEqual(0, cust1.Orders.Count);
                Assert.AreNotEqual(0, cust2.Orders.Count);
            }
        }

        [TestMethod]
        public void LazyLoadingShoundBeDisabledForDetachedEntitites3()
        {
            Customer cust1, cust2;
            int count2;

            using (NorthwindEntities context = new NorthwindEntities())
            {
                cust1 = context.Customers.First(c => c.CustomerID == "ALFKI");
                cust2 = context.Customers.First(c => c.CustomerID == "WHITC");
                count2 = cust2.Orders.Count;
                Assert.AreNotEqual(0, count2);
            }
            Assert.AreEqual(0, cust1.Orders.Count);
            Assert.AreEqual(count2, cust2.Orders.Count);
        }

        [TestMethod]
        public void LazyLoadingShoundBeDisabledForDetachedEntitites4()
        {
            Order order1, order2;

            using (NorthwindEntities context = new NorthwindEntities())
            {
                order1 = context.Orders.First(c => c.Customer.CustomerID == "ALFKI");
                order2 = context.Orders.First(c => c.Customer.CustomerID == "WHITC");
                Assert.AreEqual("WHITC", order2.Customer.CustomerID);
            }
            Assert.IsNull(order1.Customer);
        }

        [TestMethod]
        public void LazyLoadingShoundBeDisabledForDetachedEntitites5()
        {
            Order order1, order2;

            using (NorthwindEntities context = new NorthwindEntities())
            {
                order1 = context.Orders.First(c => c.Customer.CustomerID == "ALFKI");
                context.Orders.Detach(order1);
                order2 = context.Orders.First(c => c.Customer.CustomerID == "WHITC");
                Assert.AreEqual("WHITC", order2.Customer.CustomerID);
                Assert.IsNull(order1.Customer);
            }
        }
    }
}
