// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using System.Transactions;
using System.Data.EntityClient;
using EFPocoAdapter;
using System.Data.Objects.DataClasses;
using NorthwindEF.Territories;

namespace NorthwindEF.Tests
{
    [TestClass]
    public class EntitySetTests
    {
        public EntitySetTests()
        {
        }

        [TestMethod]
        public void SelectAll()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                context.Categories.ToArray();
                context.Customers.ToArray();
                context.Employees.ToArray();
                context.OrderDetails.ToArray();
                context.Orders.ToArray();
                context.Products.ToArray();
                context.Regions.ToArray();
                context.Suppliers.ToArray();
                context.Territories.ToArray();
            }
        }

        private void Touch<T>(T t)
        {
            foreach (PropertyInfo pi in t.GetType().GetProperties())
            {
                var value = pi.GetValue(t, null);
                if (pi.CanWrite)
                {
                    // set the same value
                    pi.SetValue(t, value, null);
                }
            }
        }

        private void TouchAllMembers<T>(IEnumerable<T> q)
        {
            Console.WriteLine("Touching {0}", q);
            HashSet<Type> touchedTypes = new HashSet<Type>();
            foreach (T t in q.ToArray())
            {
                if (!touchedTypes.Contains(t.GetType()))
                {
                    touchedTypes.Add(t.GetType());
                   Touch(t);
                }
            }
        }

        [TestMethod]
        public void TouchAllMembers()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                context.ChangeDetected += delegate { throw new InvalidOperationException("No change should be detected."); };
                context.CollectionChangeDetected += delegate { throw new InvalidOperationException("No change should be detected."); };

                TouchAllMembers(context.Categories);
                TouchAllMembers(context.Customers);
                TouchAllMembers(context.Employees);
                TouchAllMembers(context.OrderDetails);
                TouchAllMembers(context.Orders);
                TouchAllMembers(context.Products);
                TouchAllMembers(context.Regions);
                TouchAllMembers(context.Products);
                TouchAllMembers(context.Suppliers);
                TouchAllMembers(context.Territories);

                Touch(new Category(1, "SomeCategory", null,new List<Product>()));
                Touch(new Customer());
                Touch(new CurrentEmployee());
                Touch(new PreviousEmployee());
                Touch(new OrderDetail());
                Touch(new Product(new Supplier()));
                Touch(new DiscontinuedProduct(new Supplier()));
                Touch(new Region());
                Touch(new Supplier());
                Touch(new Territory());
                context.DetectChanges();
            }
        }

        [TestMethod]
        public void AddNewMemberTests()
        {
            using (TransactionScope ts = new TransactionScope())
            {
                using (EntityConnection conn = new EntityConnection("name=NorthwindEntities"))
                {
                    conn.Open();
                    using (NorthwindEntities context = new NorthwindEntities(conn))
                    {
                        context.Categories.InsertOnSaveChanges(new Category(100000, "Some Category", null, new List<Product>()));
                        context.Customers.InsertOnSaveChanges(new Customer { CustomerID = "YYYY", CompanyName = "Una Firma" });
                        context.Employees.InsertOnSaveChanges(new CurrentEmployee { LastName = "Kowalski", FirstName = "Jaroslaw" });
                        context.OrderDetails.InsertOnSaveChanges(new OrderDetail { Order = context.Orders.First(), Product = context.Products.First(), Quantity = 1 });
                        context.Orders.InsertOnSaveChanges(new Order() { });
                        context.Orders.InsertOnSaveChanges(new InternationalOrder() { CustomsDescription = "This is for customs" });
                        context.Products.InsertOnSaveChanges(new Product(new Supplier() { CompanyName = "Una Firma" }) { ProductName = "Some Product" });
                        context.Products.InsertOnSaveChanges(new DiscontinuedProduct(new Supplier() { CompanyName = "Una Firma" }) { ProductName = "Some Product" });
                        context.Regions.InsertOnSaveChanges(new Region() { RegionDescription = "Some new Region" });
                        context.Suppliers.InsertOnSaveChanges(new Supplier() { CompanyName = "Una Firma" });
                        context.SaveChanges();
                    }
                }
            }
        }

        [TestMethod]
        public void CategoriesTest()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                var cat = context.Categories.First(c => c.CategoryID == 1);
                Assert.AreEqual("Beverages", cat.CategoryName);
                Assert.AreEqual("Soft drinks, coffees, teas, beers, and ales", cat.Description);
                Assert.AreEqual(10746, cat.Picture.Length);
                Assert.AreEqual(0, cat.Products.Count);
                context.DetectChanges();
            }
        }

        [TestMethod]
        public void ProductsTest()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                var prod = context.Products.First(c => c.ProductID == 1);
                Assert.AreEqual(1, prod.ProductID);
                Assert.AreEqual("Chai", prod.ProductName);
                Assert.AreEqual("10 boxes x 20 bags", prod.QuantityPerUnit);
                Assert.AreEqual((short)10, prod.ReorderLevel);
                Assert.IsNull(prod.Supplier);
                context.DetectChanges();
            }
        }

        [TestMethod]
        public void AttachTest()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                var category = new Category(1, "Beverages", null, new List<Product>());
                context.Categories.Attach(category);

                // load product, assert that relationship span will automatically resolve category
                var product = context.Products.First(c => c.ProductID == 1);
                Assert.AreSame(category, product.Category);
            }
        }

        [TestMethod]
        public void AttachTest2()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                var category = new Category(1, "Beverages", null, new List<Product>());
                var product = new Product(new Supplier()) { ProductID = 1 };

                context.Categories.Attach(category);
                context.Products.Attach(product);

                var product1 = context.Products.ToArray().First(c => c.ProductID == 1);
                Assert.AreSame(product, product1);
                Assert.AreSame(category, product1.Category);
            }
        }

        [TestMethod]
        public void AttachTest3()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                var category = new Category(1, "Beverages", null, new List<Product>());
                var product = new Product(new Supplier()) { ProductID = 1, Category = category };

                context.Products.Attach(product);

                var cat = context.Categories.First(c => c.CategoryID == 1);
                Assert.AreSame(cat, category);
            }
        }

        [TestMethod]
        public void AttachTest4()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                var supplier = new Supplier() { CompanyName = "Una Firma" };
                var categoryWithProducts = new Category(1, "Beverages",null, new List<Product> { 
                        new Product(supplier) { ProductID = 1, ProductName = "Chai" },
                        new Product(supplier) { ProductID = 2, ProductName = "Chang" }, 
                        new Product(supplier) { ProductID = 34, ProductName = "Sasquatch Ale" }, 
                    });
                context.Categories.Attach(categoryWithProducts);
                var prod1 = context.Products.First(c => c.ProductID == 1);
                var prod2 = context.Products.First(c => c.ProductID == 2);
                var prod34 = context.Products.First(c => c.ProductID == 34);

                Assert.AreSame(categoryWithProducts, context.Categories.First(c => c.CategoryID == 1));
                Assert.AreSame(categoryWithProducts.Products[0], prod1);
                Assert.AreSame(categoryWithProducts.Products[1], prod2);
                Assert.AreSame(categoryWithProducts.Products[2], prod34);
            }
        }
    }
}
