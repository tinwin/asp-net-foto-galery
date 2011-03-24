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
    public class ContextInjectionTests
    {
        public ContextInjectionTests()
        {
        }

        [TestMethod]
        public void SimpleContextInjection()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                var c = context.Customers.First();
                Assert.AreSame(context, context.GetAdapterObject(c).Context);
            }
        }

        [TestMethod]
        public void ContextInjectionWithInclude()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                var o = context.Orders.Include(c => c.OrderDetails).First();
                Assert.AreSame(context, context.GetAdapterObject(o).Context);
                foreach (var d in o.OrderDetails)
                {
                    var oc = context.GetAdapterObject(d).Context;
                    Assert.AreSame(context, oc);
                }
            }
        }

        [TestMethod]
        public void ContextInjectionWithLoad()
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                var o = context.Orders.First();
                context.LoadProperty(o, c => c.OrderDetails);
                Assert.AreSame(context, context.GetAdapterObject(o).Context);
                foreach (var d in o.OrderDetails)
                {
                    var oc = context.GetAdapterObject(d).Context;
                    Assert.AreSame(context, oc);
                }
            }
        }
    }
}
