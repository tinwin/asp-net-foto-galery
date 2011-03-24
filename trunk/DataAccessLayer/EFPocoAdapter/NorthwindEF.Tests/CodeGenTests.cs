// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EFPocoAdapter;
using EFPocoAdapter.CodeGeneration;
using NorthwindEF.Territories;
using System.IO;
using System.Data.EntityClient;
using System.Data.Metadata.Edm;
using System.Reflection;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Data.Common;
using System.ComponentModel;
using NorthwindEF.PocoAdapters;

namespace NorthwindEF.Tests
{
    [TestClass]
    public class CodeGenTests
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

        public CodeGenTests()
        {
        }

        [TestMethod]
        public void GeneratePocoAdapter()
        {
            PocoAdapterCodeGenerator pacg = new PocoAdapterCodeGenerator();

            EntityConnection ec = new EntityConnection("name=NorthwindEntities");

            pacg.Metadata = (EdmItemCollection)ec.GetMetadataWorkspace().GetItemCollection(DataSpace.CSpace);
            pacg.EdmToClrNamespaceMapping["NorthwindEFModel"] = "NorthwindEF";
            pacg.GenerateProxies = true;
            pacg.Assemblies.Add(typeof(Customer).Assembly);
            pacg.Assemblies.Add(typeof(Territory).Assembly);
            StringWriter myStringWriter = new StringWriter();
            pacg.Generate(new SourceWriter(myStringWriter));
            string sourceCode = myStringWriter.ToString();
            Assert.IsTrue(sourceCode.IndexOf("CustomerAdapter") > 0);
            Assert.IsTrue(sourceCode.IndexOf("TerritoryAdapter") > 0);
        }

        [TestMethod]
        public void GeneratePocoAdapterAssembly()
        {
            PocoAdapterCodeGenerator pacg = new PocoAdapterCodeGenerator();

            EntityConnection ec = new EntityConnection("name=NorthwindEntities");

            pacg.Metadata = (EdmItemCollection)ec.GetMetadataWorkspace().GetItemCollection(DataSpace.CSpace);
            pacg.EdmToClrNamespaceMapping["NorthwindEFModel"] = "NorthwindEF";
            pacg.GenerateProxies = true;
            pacg.Assemblies.Add(typeof(Customer).Assembly);
            pacg.Assemblies.Add(typeof(Territory).Assembly);
            Assembly result = pacg.GenerateAssembly(null, true);
            Assert.IsNotNull(result);
            Type containerClass = result.GetType("NorthwindEF.PocoAdapters.NorthwindEntitiesAdapter");
            Assert.IsNotNull(containerClass);
            Assert.IsTrue(typeof(ObjectContext).IsAssignableFrom(containerClass));

            Assert.AreEqual(pacg.Metadata.GetItems<EntityType>().Count(), result.GetTypes().Where(t=>t.IsDefined(typeof(EdmEntityTypeAttribute),false)).Count());
            Assert.AreEqual(pacg.Metadata.GetItems<ComplexType>().Count(), result.GetTypes().Where(t => t.IsDefined(typeof(EdmComplexTypeAttribute), false)).Count());
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GeneratePocoAdapterAssemblyWithBadOCMapping()
        {
            PocoAdapterCodeGenerator pacg = new PocoAdapterCodeGenerator();

            EntityConnection ec = new EntityConnection("name=NorthwindEntities");

            pacg.Metadata = (EdmItemCollection)ec.GetMetadataWorkspace().GetItemCollection(DataSpace.CSpace);
            pacg.GenerateProxies = true;
            pacg.Assemblies.Add(typeof(Customer).Assembly);
            pacg.Assemblies.Add(typeof(Territory).Assembly);

            // code wont' compile because certain CLR types don't exist
            pacg.GenerateAssembly(null, true);
        }


        [TestMethod]
        public void GeneratePocoClasses()
        {
            PocoClassCodeGenerator pacg = new PocoClassCodeGenerator();
            EntityConnection ec = new EntityConnection("name=NorthwindEntities");

            pacg.Metadata = (EdmItemCollection)ec.GetMetadataWorkspace().GetItemCollection(DataSpace.CSpace);
            pacg.EdmToClrNamespaceMapping["NorthwindEFModel"] = "NorthwindEF";
            pacg.GenerateProxies = true;
            pacg.Assemblies.Add(typeof(Customer).Assembly);
            pacg.Assemblies.Add(typeof(Territory).Assembly);
            StringWriter myStringWriter = new StringWriter();
            pacg.Generate(new SourceWriter(myStringWriter));
            string sourceCode = myStringWriter.ToString();
            Assert.IsTrue(sourceCode.IndexOf("InternationalOrder : NorthwindEF.Order") > 0);
        }

        [TestMethod]
        public void GeneratePocoClassesToDirectory()
        {
            PocoClassCodeGenerator pacg = new PocoClassCodeGenerator();
            EntityConnection ec = new EntityConnection("name=NorthwindEntities");
            StringWriter stringWriter = new StringWriter();

            pacg.Metadata = (EdmItemCollection)ec.GetMetadataWorkspace().GetItemCollection(DataSpace.CSpace);
            pacg.EdmToClrNamespaceMapping["NorthwindEFModel"] = "NorthwindEF";
            pacg.VerboseOutput = stringWriter;
            StringWriter myStringWriter = new StringWriter();
            string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            try
            {
                pacg.Generate(tempDirectory);
                Dictionary<string, string> fileName2Class = new Dictionary<string, string>();
                foreach (StructuralType st in pacg.Metadata.GetItems<StructuralType>().Where(c => c is EntityType || c is ComplexType))
                {
                    string className = pacg.GetPocoClrTypeName(st);
                    int lastDot = className.LastIndexOf('.');
                    string fn;
                    if (lastDot >= 0)
                    {
                        string namespaceName = className.Substring(0, lastDot);
                        string bareClassName = className.Substring(lastDot + 1);
                        fn = Path.Combine(Path.Combine(tempDirectory, namespaceName), bareClassName + ".cs");
                    }
                    else
                    {
                        fn = Path.Combine(tempDirectory, className + ".cs");
                    }

                    if (!File.Exists(fn))
                        throw new InvalidOperationException("File " + fn + " does not exist.");
                    fileName2Class[fn] = className;
                }

                // compile the result

                var providerOptions = new Dictionary<string, string>();
                providerOptions.Add("CompilerVersion", "v3.5");
                CSharpCodeProvider codeProvider = new CSharpCodeProvider(providerOptions);
                CompilerParameters parameters = new CompilerParameters();
                parameters.GenerateInMemory = true;
                var results = codeProvider.CompileAssemblyFromFile(parameters, fileName2Class.Keys.ToArray());
                Assert.IsFalse(results.Errors.HasErrors);
                var assembly = results.CompiledAssembly;

                // verify that the assembly has correct types
                foreach (var typeName in fileName2Class.Values)
                {
                    assembly.GetType(typeName, true);
                }
                
            }
            finally
            {
                if (Directory.Exists(tempDirectory))
                    Directory.Delete(tempDirectory, true);
            }
        }

        [TestMethod]
        public void GeneratePocoAdaptersToDirectory()
        {
            PocoAdapterCodeGenerator pacg = new PocoAdapterCodeGenerator();
            EntityConnection ec = new EntityConnection("name=NorthwindEntities");

            pacg.Metadata = (EdmItemCollection)ec.GetMetadataWorkspace().GetItemCollection(DataSpace.CSpace);
            pacg.EdmToClrNamespaceMapping["NorthwindEFModel"] = "NorthwindEF";
            pacg.GenerateProxies = true;
            pacg.Assemblies.Add(typeof(Customer).Assembly);
            pacg.Assemblies.Add(typeof(Territory).Assembly);
            StringWriter myStringWriter = new StringWriter();
            string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            try
            {
                pacg.Generate(tempDirectory);
                Dictionary<string, string> fileName2Class = new Dictionary<string, string>();
                foreach (StructuralType st in pacg.Metadata.GetItems<StructuralType>().Where(c => c is EntityType || c is ComplexType))
                {
                    string className = pacg.GetAdapterClrTypeName(st);
                    int lastDot = className.LastIndexOf('.');
                    string namespaceName = className.Substring(0, lastDot);
                    string bareClassName = className.Substring(lastDot + 1);
                    string fn = Path.Combine(Path.Combine(tempDirectory, namespaceName), bareClassName + ".cs");
                    if (!File.Exists(fn))
                        throw new InvalidOperationException("File " + fn + " does not exist.");
                    fileName2Class[fn] = className;
                }

                foreach (StructuralType st in pacg.Metadata.GetItems<StructuralType>().Where(c => c is EntityType && !c.Abstract))
                {
                    string className = pacg.GetProxyClrTypeName(st);
                    int lastDot = className.LastIndexOf('.');
                    string namespaceName = className.Substring(0, lastDot);
                    string bareClassName = className.Substring(lastDot + 1);
                    string fn = Path.Combine(Path.Combine(tempDirectory, namespaceName), bareClassName + ".cs");
                    if (!File.Exists(fn))
                        throw new InvalidOperationException("File " + fn + " does not exist.");
                    fileName2Class[fn] = className;
                }

                fileName2Class[Path.Combine(tempDirectory, "Global.cs")] = null;

                // compile the result

                var providerOptions = new Dictionary<string, string>();
                providerOptions.Add("CompilerVersion", "v3.5");
                CSharpCodeProvider codeProvider = new CSharpCodeProvider(providerOptions);
                CompilerParameters parameters = new CompilerParameters();
                parameters.GenerateInMemory = true;
                parameters.ReferencedAssemblies.Add(typeof(DbConnection).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(INotifyPropertyChanging).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(Action).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(EFPocoContext).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(EntityCommand).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(Customer).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(Territory).Assembly.Location);
                var results = codeProvider.CompileAssemblyFromFile(parameters, fileName2Class.Keys.ToArray());
                Assert.IsFalse(results.Errors.HasErrors);
                var assembly = results.CompiledAssembly;

                // verify that the assembly has correct types
                foreach (var typeName in fileName2Class.Values)
                {
                    if (typeName != null)
                    {
                        assembly.GetType(typeName, true);
                    }
                }

                Assert.IsTrue(assembly.IsDefined(typeof(EdmSchemaAttribute), false));
            }
            finally
            {
                if (Directory.Exists(tempDirectory))
                    Directory.Delete(tempDirectory, true);
            }
        }

        [TestMethod]
        public void GeneratePocoContainersToDirectory()
        {
            PocoContainerCodeGenerator pacg = new PocoContainerCodeGenerator();
            EntityConnection conn = new EntityConnection("name=NorthwindEntities");

            pacg.Metadata = (EdmItemCollection)conn.GetMetadataWorkspace().GetItemCollection(DataSpace.CSpace);
            pacg.EdmToClrNamespaceMapping["NorthwindEFModel"] = "NorthwindEF";
            pacg.GenerateProxies = true;
            pacg.Assemblies.Add(typeof(Customer).Assembly);
            pacg.Assemblies.Add(typeof(Territory).Assembly);
            StringWriter myStringWriter = new StringWriter();
            string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            try
            {
                pacg.Generate(tempDirectory);
                Dictionary<string, string> fileName2Class = new Dictionary<string, string>();
                foreach (EntityContainer ec in pacg.Metadata.GetItems<EntityContainer>())
                {
                    string className = "NorthwindEF." + ec.Name;
                    int lastDot = className.LastIndexOf('.');
                    string namespaceName = className.Substring(0, lastDot);
                    string bareClassName = className.Substring(lastDot + 1);
                    string fn = Path.Combine(Path.Combine(tempDirectory, namespaceName), bareClassName + ".cs");
                    if (!File.Exists(fn))
                        throw new InvalidOperationException("File " + fn + " does not exist.");
                    fileName2Class[fn] = className;
                }

                // compile the result

                var providerOptions = new Dictionary<string, string>();
                providerOptions.Add("CompilerVersion", "v3.5");
                CSharpCodeProvider codeProvider = new CSharpCodeProvider(providerOptions);
                CompilerParameters parameters = new CompilerParameters();
                parameters.GenerateInMemory = true;
                parameters.ReferencedAssemblies.Add(typeof(DbConnection).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(INotifyPropertyChanging).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(Action).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(EFPocoContext).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(EntityCommand).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(Customer).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(Territory).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(NorthwindEntitiesAdapter).Assembly.Location);
                var results = codeProvider.CompileAssemblyFromFile(parameters, fileName2Class.Keys.ToArray());
                Assert.IsFalse(results.Errors.HasErrors);
                var assembly = results.CompiledAssembly;

                // verify that the assembly has correct types
                foreach (var typeName in fileName2Class.Values)
                {
                    if (typeName != null)
                    {
                        Type t = assembly.GetType(typeName, true);
                        Assert.IsTrue(typeof(EFPocoContext).IsAssignableFrom(t));
                    }
                }
            }
            finally
            {
                if (Directory.Exists(tempDirectory))
                    Directory.Delete(tempDirectory, true);
            }
        }

        [TestMethod]
        public void GeneratePocoContainer()
        {
            PocoContainerCodeGenerator pacg = new PocoContainerCodeGenerator();
            EntityConnection ec = new EntityConnection("name=NorthwindEntities");

            pacg.Metadata = (EdmItemCollection)ec.GetMetadataWorkspace().GetItemCollection(DataSpace.CSpace);
            pacg.EdmToClrNamespaceMapping["NorthwindEFModel"] = "NorthwindEF";
            pacg.GenerateProxies = true;
            pacg.Assemblies.Add(typeof(Customer).Assembly);
            pacg.Assemblies.Add(typeof(Territory).Assembly);
            StringWriter myStringWriter = new StringWriter();
            pacg.Generate(new SourceWriter(myStringWriter));
            string sourceCode = myStringWriter.ToString();
            Assert.IsTrue(sourceCode.IndexOf("NorthwindEntities : EFPocoContext") > 0);
            Assert.IsTrue(sourceCode.IndexOf("IEntitySet<NorthwindEF.Order> Orders") > 0);
        }
    }
}
