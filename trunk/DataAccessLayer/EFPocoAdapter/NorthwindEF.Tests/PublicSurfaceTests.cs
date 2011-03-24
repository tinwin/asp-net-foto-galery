// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Common;

namespace NorthwindEF.Tests
{
    [TestClass]
    public class PublicSurfaceTests
    {
        public PublicSurfaceTests()
        {
        }

        [TestMethod]
        public void PublicSurfaceTest1()
        {
            List<Type> actualTypes = typeof(EFPocoAdapter.EFPocoContext).Assembly.GetExportedTypes().Where(c => c.Namespace == "EFPocoAdapter").OrderBy(c => c.Name).ToList();
            List<Type> expectedTypes = new List<Type>{
                                             typeof(EFPocoAdapter.ChangeDetectedEventArgs),
                                             typeof(EFPocoAdapter.CollectionChangeDetectedEventArgs),
                                             typeof(EFPocoAdapter.EFPocoAdapterGenerator<>),
                                             typeof(EFPocoAdapter.EFPocoCompiledQuery),
                                             typeof(EFPocoAdapter.EFPocoContext),
                                             typeof(EFPocoAdapter.EFPocoContext<>),
                                             typeof(EFPocoAdapter.EFPocoContextFactory<>),
                                             typeof(EFPocoAdapter.EFPocoQuery<>),
                                             typeof(EFPocoAdapter.ExtensionMethods),
                                             typeof(EFPocoAdapter.IEntitySet<>),
                                             typeof(EFPocoAdapter.ILazyLoadable),
                                         };

            Assert.AreEqual(expectedTypes.Count, actualTypes.Count);
            for (int i = 0; i < expectedTypes.Count; ++i)
                Assert.AreSame(expectedTypes[i], actualTypes[i]);
        }

        [TestMethod]
        public void PublicSurfaceTest2()
        {
            List<Type> actualTypes = typeof(EFPocoAdapter.EFPocoContext).Assembly.GetExportedTypes().Where(c => c.Namespace == "EFPocoAdapter.DataClasses").OrderBy(c => c.Name).ToList();
            List<Type> expectedTypes = new List<Type>{
                                             typeof(EFPocoAdapter.DataClasses.IComplexTypeAdapter<>),
                                             typeof(EFPocoAdapter.DataClasses.IEntityProxy),
                                             typeof(EFPocoAdapter.DataClasses.IListEntityCollectionAdapter<,>),
                                             typeof(EFPocoAdapter.DataClasses.IPocoAdapter),
                                             typeof(EFPocoAdapter.DataClasses.IPocoAdapter<>),
                                             typeof(EFPocoAdapter.DataClasses.IPocoAdapterObjectContext),
                                             typeof(EFPocoAdapter.DataClasses.PocoAdapterBase<>),
                                             typeof(EFPocoAdapter.DataClasses.QueryTranslationCache),
                                             typeof(EFPocoAdapter.DataClasses.ThreadLocalContext),
                                         };

            Assert.AreEqual(expectedTypes.Count, actualTypes.Count);
            for (int i = 0; i < expectedTypes.Count; ++i)
                Assert.AreSame(expectedTypes[i], actualTypes[i]);
        }

        [TestMethod]
        public void PublicSurfaceTest3()
        {
            List<Type> actualTypes = typeof(EFPocoAdapter.EFPocoContext).Assembly.GetExportedTypes().Where(c => c.Namespace == "EFPocoAdapter.Internal").OrderBy(c => c.Name).ToList();
            Assert.AreEqual(0, actualTypes.Count);
        }
    }
}
