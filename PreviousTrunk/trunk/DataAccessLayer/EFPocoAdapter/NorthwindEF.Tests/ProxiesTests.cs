// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using EFPocoAdapter;
using NorthwindEF.PocoProxies;
using NorthwindEF.PocoAdapters;
using EFPocoAdapter.DataClasses;
using System.Collections;
using System.Reflection;

namespace NorthwindEF.Tests
{
    [TestClass]
    public class ProxiesTests
    {
        public ProxiesTests()
        {
        }

        [TestMethod]
        public void QueryWithProxiesDisabledTest()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                context.EnableChangeTrackingUsingProxies = false;

                var cust = context.Customers.Include("Orders").First(c => c.CustomerID == "ALFKI");
                Assert.IsTrue(cust.GetType() == typeof(Customer));
                Assert.IsTrue(cust.Orders.Count > 0);
                foreach (var o in cust.Orders)
                {
                    Assert.IsTrue(o.GetType() == typeof(Order) || o.GetType() == typeof(InternationalOrder));
                }
            }
        }

        [TestMethod]
        public void QueryWithProxiesEnabledTest()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                context.EnableChangeTrackingUsingProxies = true;

                var cust = context.Customers.Include("Orders").First(c => c.CustomerID == "ALFKI");
                Assert.IsTrue(cust.GetType() == typeof(CustomerProxy));
                Assert.IsTrue(cust.Orders.Count > 0);
                Assert.IsTrue(cust.Orders is IListEntityCollectionAdapter<Order, OrderAdapter>);
                foreach (var o in cust.Orders)
                {
                    Assert.IsTrue(o.GetType() == typeof(OrderProxy) || o.GetType() == typeof(InternationalOrderProxy));
                }
            }
        }

        [TestMethod]
        public void ConvertPocoToProxiesTest()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                context.ChangeDetected += delegate
                {
                    throw new InvalidOperationException();
                };
                context.CollectionChangeDetected += delegate
                {
                    throw new InvalidOperationException();
                };
                context.EnableChangeTrackingUsingProxies = false;
                var c0 = context.Customers.Include("Orders").First(c => c.CustomerID == "ALFKI");
                Assert.IsFalse(c0 is CustomerProxy);
                Assert.IsTrue(c0.Orders.Count > 0);
                context.ConvertPocoToProxies();

                // query again
                var cust = context.Customers.Include("Orders").First(c => c.CustomerID == "ALFKI");

                Assert.IsTrue(cust.GetType() == typeof(CustomerProxy));
                Assert.IsTrue(cust.Orders is IListEntityCollectionAdapter<Order, OrderAdapter>);
                Assert.IsTrue(cust.Orders.Count > 0);
                foreach (var o in cust.Orders)
                {
                    Assert.IsTrue(o.GetType() == typeof(OrderProxy) || o.GetType() == typeof(InternationalOrderProxy));
                }
            }
        }

        [TestMethod]
        public void ConvertProxiesToPoco()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                context.EnableChangeTrackingUsingProxies = true;
                var c0 = context.Customers.Include("Orders").First(c => c.CustomerID == "ALFKI");
                Assert.IsTrue(c0 is CustomerProxy);
                context.ConvertProxiesToPoco();

                var cust = context.Customers.Include("Orders").First(c => c.CustomerID == "ALFKI");
                Assert.IsTrue(cust.GetType() == typeof(Customer));
                Assert.IsTrue(cust.Orders.Count > 0);
                foreach (var o in cust.Orders)
                {
                    Assert.IsTrue(o.GetType() == typeof(Order) || o.GetType() == typeof(InternationalOrder));
                }
            }
        }

        private HashSet<object> _visitedObjects;

        [TestMethod]
        public void ConvertAndCompare()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                context.EnableChangeTrackingUsingProxies = false;
                context.DeferredLoadingEnabled = false;

                ArrayList allObjects = new ArrayList();

                allObjects.AddRange(context.Categories.OrderBy(c => c.CategoryID).Take(10).ToArray());
                allObjects.AddRange(context.Customers.OrderBy(c => c.CustomerID).Take(10).ToArray());
                allObjects.AddRange(context.Employees.OrderBy(c => c.EmployeeID).Take(10).ToArray());
                allObjects.AddRange(context.OrderDetails.OrderBy(c => c.OrderID).ThenBy(c => c.ProductID).Take(10).ToArray());
                allObjects.AddRange(context.Orders.OrderBy(c => c.OrderID).Take(10).ToArray());
                allObjects.AddRange(context.Products.OrderBy(c => c.ProductID).Take(10).ToArray());
                allObjects.AddRange(context.Regions.OrderBy(c => c.RegionID).Take(10).ToArray());
                allObjects.AddRange(context.Suppliers.OrderBy(c => c.SupplierID).ToArray());
                allObjects.AddRange(context.Territories.OrderBy(c => c.TerritoryDescription).Take(10).ToArray());

                context.ConvertPocoToProxies();
                ArrayList allObjects2 = new ArrayList();

                allObjects2.AddRange(context.Categories.OrderBy(c => c.CategoryID).Take(10).ToArray());
                allObjects2.AddRange(context.Customers.OrderBy(c => c.CustomerID).Take(10).ToArray());
                allObjects2.AddRange(context.Employees.OrderBy(c => c.EmployeeID).Take(10).ToArray());
                allObjects2.AddRange(context.OrderDetails.OrderBy(c => c.OrderID).ThenBy(c => c.ProductID).Take(10).ToArray());
                allObjects2.AddRange(context.Orders.OrderBy(c => c.OrderID).Take(10).ToArray());
                allObjects2.AddRange(context.Products.OrderBy(c => c.ProductID).Take(10).ToArray());
                allObjects2.AddRange(context.Regions.OrderBy(c => c.RegionID).Take(10).ToArray());
                allObjects2.AddRange(context.Suppliers.OrderBy(c => c.SupplierID).ToArray());
                allObjects2.AddRange(context.Territories.OrderBy(c => c.TerritoryDescription).Take(10).ToArray());

                _visitedObjects = new HashSet<object>();
                CompareObjectsAndProxies(allObjects.ToArray(), allObjects2.ToArray());
            }
        }

        private void CompareObjectsAndProxies(object[] pocoObjects, object[] proxyObjects)
        {
            Assert.AreEqual(pocoObjects.Length, proxyObjects.Length);
            for (int i = 0; i < pocoObjects.Length; ++i)
                CompareTwoObjects(pocoObjects[i], proxyObjects[i]);
        }

        private void CompareTwoObjects(object pocoObject, object proxyObject)
        {
            if (_visitedObjects.Contains(pocoObject))
                return;

            _visitedObjects.Add(pocoObject);

            Assert.IsFalse(pocoObject is IEntityProxy);
            Assert.IsTrue(proxyObject is IEntityProxy);
            Assert.IsTrue(pocoObject.GetType().IsAssignableFrom(proxyObject.GetType()));
            PropertyInfo[] props = pocoObject.GetType().GetProperties();
            foreach (PropertyInfo pi in props)
            {
                // skip read-only properties
                if (pi.GetSetMethod() == null)
                    continue;

                object pocoValue = pi.GetValue(pocoObject, null);
                object proxyValue = pi.GetValue(proxyObject, null);

                if (pi.PropertyType.IsValueType || pi.PropertyType == typeof(string))
                {
                    Assert.AreEqual(pocoValue, proxyValue);
                }
                else if (pocoValue == null || proxyValue == null)
                {
                    Assert.IsNull(pocoValue);
                    Assert.IsNull(proxyValue);
                }
                else if (pocoValue is byte[])
                {
                    Assert.AreSame(pocoValue, proxyValue);
                }
                else if (pocoValue is IEnumerable)
                {
                    CompareCollections(pocoValue, proxyValue);
                }
                else
                {
                    CompareTwoObjects(pocoValue, proxyValue);
                    //Console.WriteLine("Compare: {0} and {1}", pocoValue, proxyValue);
                }
            }
        }

        private void CompareCollections(object pocoValue, object proxyValue)
        {
            Assert.IsTrue(pocoValue is IEnumerable);
            Assert.IsTrue(proxyValue is IEnumerable);
            if (pocoValue.GetType().IsGenericType)
            {
                Assert.IsTrue(proxyValue.GetType().IsGenericType);
                //Assert.AreSame(typeof(IListEntityCollectionAdapter<,>), proxyValue.GetType().GetGenericTypeDefinition());
                Assert.AreSame(pocoValue.GetType().GetGenericArguments()[0], proxyValue.GetType().GetGenericArguments()[0]);
            }
            CompareObjectsAndProxies(((IEnumerable)pocoValue).Cast<object>().ToArray(), ((IEnumerable)proxyValue).Cast<object>().ToArray());
        }

        [TestMethod]
        public void IEntityProxyTest()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                context.EnableChangeTrackingUsingProxies = true;
                VerifyProxy(context, context.Categories.First());
                VerifyProxy(context, context.Customers.First());
                VerifyProxy(context, context.Employees.Where(c => c is CurrentEmployee).First());
                VerifyProxy(context, context.Employees.Where(c => c is PreviousEmployee).First());
                VerifyProxy(context, context.OrderDetails.First());
                VerifyProxy(context, context.Orders.OfType<InternationalOrder>().First());
                VerifyProxy(context, context.Orders.Where(c => !(c is InternationalOrder)).First());
                VerifyProxy(context, context.Products.Where(c => !(c is DiscontinuedProduct)).First());
                VerifyProxy(context, context.Products.Where(c => c is DiscontinuedProduct).First());
                VerifyProxy(context, context.Regions.First());
                VerifyProxy(context, context.Suppliers.First());
                VerifyProxy(context, context.Territories.First());
            }
        }

        private void VerifyProxy(EFPocoContext context, object entity)
        {
            IEntityProxy ep = entity as IEntityProxy;
            Assert.IsNotNull(ep);

            object adapter = context.GetAdapterObject(entity);
            Assert.AreSame(adapter, ep.Adapter);
        }
    }
}
