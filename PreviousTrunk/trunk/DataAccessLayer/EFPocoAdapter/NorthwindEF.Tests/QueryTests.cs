// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;

namespace NorthwindEF.Tests
{
    [TestClass]
    public class QueryTests
    {
        public QueryTests()
        {
        }

        [TestMethod]
        public void First()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                var p = context.Customers.First();
                Console.WriteLine(p);
                //p.ToArray();
            }
        }

        [TestMethod]
        public void NestedAnonymousTypes()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                var p = context.Customers.Select(c => new { A = c, K = 1, B = new { c } }).ToArray();
            }
        }

        [TestMethod]
        public void CollectionsTest1()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                var p = context.Customers.Where(c => c.CustomerID == "ALFKI").Select(c => new { Cust = c, Ord = c.Orders }).First();
                Assert.AreEqual("ALFKI", p.Cust.CustomerID);
                Assert.AreEqual(6, p.Ord.Count);
            }
        }

        [TestMethod]
        public void CollectionsTest2()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                var p = context.Customers.Where(c => c.CustomerID == "ALFKI").Select(c => c.Orders).Take(1).ToArray();
                Assert.AreEqual(1, p.Length);
                Assert.AreEqual(6, p[0].Count);
            }
        }

        [TestMethod]
        public void ProjectReferences1()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                var p = context.Products.Where(c => c.ProductID == 1).Select(c => c.Category).First();
                Assert.AreEqual("Beverages", p.CategoryName);
            }
        }

        [TestMethod]
        public void ProjectReferences2()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                var p = context.Products.Where(c => c.ProductID == 1).Select(c => new { c.Category }).First();
                Assert.AreEqual("Beverages", p.Category.CategoryName);
            }
        }

        [TestMethod]
        public void ComplexTypeTests()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                var p = context.Customers.Where(c => c.CustomerID == "ALFKI").Select(c => new { c.Address }).First();
            }
        }

        [TestMethod]
        public void QueryOfPrimitiveType1()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                var foo = context.CreateQuery<string>("LEFT('FooBar',3)").ToArray().First();
                Assert.AreEqual("Foo", foo);
            }
        }

        [TestMethod]
        public void QueryOfType()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                var q = context.Products.Where(p => p.ProductName == "Foobar").OfType<DiscontinuedProduct>();
                q.ToList();
            }
        }

        [TestMethod]
        public void TypeIs()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                var q = context.Products.Where(p => p is DiscontinuedProduct).OfType<DiscontinuedProduct>();
                q.ToList();
            }
        }

        [TestMethod]
        public void NullableResult1()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                var q = context.Products.Select(c => c.ReorderLevel);
                q.ToList();
            }
        }

        [TestMethod]
        public void NullableResult2()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                var q = context.Products.Select(c => c.ReorderLevel).First();
            }
        }

        [TestMethod]
        public void TypeAs()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                var q = context.Products.Select(p => (p as DiscontinuedProduct).ProductName);
                q.ToList();
            }
        }

        [TestMethod]
        public void NestedQuery1()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                var q = context.Categories.Select(c => new { Category = c, Products = (IEnumerable<Product>)c.Products });
                q.ToList();
            }
        }

        [TestMethod]
        public void NestedQuery2()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                var q = context.Categories.Select(c => new { Category = c, Products = c.Products.Where(p => p.ProductName == "Chai") });
                q.ToList();
            }
        }

        [TestMethod]
        public void NestedQuery3()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                var q = from c in context.Categories
                        from p in context.Products
                        where p.Category.CategoryID == c.CategoryID && p.Category == c
                        select new { p, c };
                bool any = false;
                foreach (var r in q)
                {
                    Assert.AreSame(r.p.Category, r.c);
                    any = true;
                }
                Assert.IsTrue(any);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void NestedQueryNegativeTest()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                using (NorthwindEntities context2 = new NorthwindEntities())
                {
                    var q = from c in context.Categories
                            from p in context2.Products
                            where p.Category.CategoryID == c.CategoryID && p.Category == c
                            select new { p, c };
                    q.ToArray();
                }
            }
        }

        [TestMethod]
        public void ElementTypeTest()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                Assert.AreSame(typeof(Product), context.Products.ElementType);
            }

        }

        [TestMethod]
        public void GetEnumeratorTest()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                bool any = false;

                foreach (object o in (IEnumerable)context.Products)
                {
                    Assert.IsTrue(o is Product);
                    any = true;
                }
                Assert.IsTrue(any);
            }
        }
    }
}
