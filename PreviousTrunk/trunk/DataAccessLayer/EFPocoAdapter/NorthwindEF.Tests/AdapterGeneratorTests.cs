// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EFPocoAdapter;
using NorthwindEF.Territories;
using System.IO;

namespace NorthwindEF.Tests
{
    [TestClass]
    public class AdapterGeneratorTests
    {
        class MyHandWrittenNorthwindEntities : EFPocoContext
        {
            public MyHandWrittenNorthwindEntities()
            {
            }

            public IEntitySet<Customer> Customers
            {
                get { return GetEntitySet<Customer>("Customers"); }
            }
        }

        public AdapterGeneratorTests()
        {
        }

        [TestMethod]
        public void GenerateAndUsePocoAdapter()
        {
            StringWriter myStringWriter = new StringWriter();

            var generator = new EFPocoAdapterGenerator<MyHandWrittenNorthwindEntities>();
            generator.EntityConnectionString = "name=NorthwindEntities";
            generator.EdmToClrNamespaceMapping["NorthwindEFModel"] = "NorthwindEF";
            generator.GenerateProxies = true;
            generator.ObjectAssemblies.Add(typeof(Customer).Assembly);
            generator.ObjectAssemblies.Add(typeof(Territory).Assembly);
            generator.VerboseOutput = myStringWriter;
            generator.SaveGeneratedCodeIn = "foo.cs";

            if (File.Exists("foo.cs"))
                File.Delete("foo.cs");

            var factory = generator.CreateContextFactory();
            using (var context = factory.CreateContext())
            {
                Assert.IsInstanceOfType(context, typeof(MyHandWrittenNorthwindEntities));
                var cust = context.Customers.First(c => c.CustomerID == "ALFKI");
            }
        }
    }
}
