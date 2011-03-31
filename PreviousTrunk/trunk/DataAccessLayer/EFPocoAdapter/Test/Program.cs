// Copyright (c) Microsoft Corporation.  All rights reserved.

using System.Linq;

using NorthwindEF.Tests;
using NorthwindEF;
using System;
using System.Diagnostics;
using System.Data.Metadata.Edm;

using EFPocoAdapter;
using System.Data.Objects;
using EFPocoAdapter.DataClasses;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            using (NorthwindEntities context = new NorthwindEntities())
            {
                var cat = context.Categories.First();
                Console.WriteLine(cat.CategoryName);

                // load an object
                var cust = context.Customers.First(c => c.CustomerID == "ALFKI");
                Console.WriteLine(cust.ContactPerson.Name);

                // change its name
                cust.ContactPerson = new ContactInfo { Name=cust.ContactPerson.Name + " Jr.", Title=cust.ContactPerson.Title };
                // modify address (immutable value type)
                cust.Address = new CommonAddress(cust.Address + "...", cust.Address.City, cust.Address.Country, cust.Address.PostalCode, cust.Address.Country);

                // add and remove some associations
                // explicit load is needed when not using proxies
                if (!context.IsPropertyLoaded(cust, c => c.Orders))
                {
                    context.LoadProperty(cust, c => c.Orders);
                }

                // remove/add
                var ord = new Order();
                cust.Orders.RemoveAt(0);
                cust.Orders.Add(ord);

                if (context.EnableChangeTrackingUsingProxies)
                {
                    // reference fixup is live because cust.Orders is a proxy collection
                    Debug.Assert(Object.ReferenceEquals(ord.Customer, cust));
                }
                else
                {
                    Debug.Assert(ord.Customer == null);
                }

                context.DetectChanges();

                // reference fixup must have happened until now
                Debug.Assert(Object.ReferenceEquals(ord.Customer, cust));

                // context.SaveChanges();
            }
            Console.WriteLine("All done.");
        }

        static void context_CollectionChangeDetected(object sender, EFPocoAdapter.CollectionChangeDetectedEventArgs e)
        {
            ConsoleColor oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            try
            {
                Console.WriteLine("Change in {0}.{1}:", e.Entity.GetType().Name, e.Member);
                if (e.AddedObjects.Count > 0)
                    Console.WriteLine("  Added {0} object(s): {1}", e.AddedObjects.Count, String.Join(" ", e.AddedObjects.Select(c => "{" + GetDisplayString(c) + "}").ToArray()));
                if (e.RemovedObjects.Count > 0)
                    Console.WriteLine("  Removed {0} object(s): {1}", e.RemovedObjects.Count, String.Join(" ", e.RemovedObjects.Select(c => "{" + GetDisplayString(c) + "}").ToArray()));
            }
            finally
            {
                Console.ForegroundColor = oldColor;
            }
        }

        static void context_ChangeDetected(object sender, EFPocoAdapter.ChangeDetectedEventArgs e)
        {
            ConsoleColor oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            try
            {
                Console.WriteLine("Change on {0}.{1}:", e.Entity.GetType().Name, e.Member);
                Console.WriteLine("  Old: '{0}'", e.OldValue);
                Console.WriteLine("  New: '{0}'", e.NewValue);
            }
            finally
            {
                Console.ForegroundColor = oldColor;
            }
        }

        static string GetDisplayString(object o)
        {
            if (o == null)
                return "null";

            return o.GetType().Name;
        }
    }
}