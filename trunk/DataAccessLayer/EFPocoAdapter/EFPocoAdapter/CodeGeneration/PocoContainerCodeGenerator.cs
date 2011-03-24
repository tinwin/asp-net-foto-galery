// Copyright (c) Microsoft Corporation.  All rights reserved.

using System.Data.Metadata.Edm;
using System.IO;
using System.Linq;

namespace EFPocoAdapter.CodeGeneration
{
    public class PocoContainerCodeGenerator : CodeGeneratorBase
    {
        private void WriteEntityContainers(string outputDirectory)
        {
            foreach (EntityContainer container in Metadata.GetItems<EntityContainer>())
            {
                using (StreamWriter output = new StreamWriter(
                    GetFileNameFor(outputDirectory,
                        GetPocoClrTypeNamespace(Metadata.GetItems<EntityType>().First()) + "." + container.Name)))
                {
                    SourceWriter sourceWriter = new SourceWriter(output);
                    WriteEntityContainer(sourceWriter, container);
                }
            }
        }

        private void WriteEntityContainers(SourceWriter sourceWriter)
        {
            foreach (EntityContainer container in Metadata.GetItems<EntityContainer>())
            {
                WriteEntityContainer(sourceWriter, container);
            }
        }

        private void WriteEntityContainer(SourceWriter sourceWriter, EntityContainer container)
        {
            sourceWriter.WriteLine("using System;");
            sourceWriter.WriteLine("using System.Data.EntityClient;");
            sourceWriter.WriteLine("using EFPocoAdapter;");
            sourceWriter.Variables["codeNamespace"] = GetPocoClrTypeNamespace(Metadata.GetItems<EntityType>().First());
            sourceWriter.Variables["adapterNamespace"] = GetAdapterClrTypeNamespace(Metadata.GetItems<EntityType>().First());
            sourceWriter.WriteLine();
            sourceWriter.WriteLine("namespace {codeNamespace}");
            using (sourceWriter.Indent())
            {
                using (sourceWriter.NewVariableScope())
                {
                    sourceWriter.Variables["containerName"] = container.Name;
                    sourceWriter.WriteLine("public interface I{containerName}");
                    using (sourceWriter.Indent())
                    {
                        foreach (EntitySet entitySet in container.BaseEntitySets.OfType<EntitySet>())
                        {
                            sourceWriter.Variables["entitySetName"] = entitySet.Name;
                            sourceWriter.Variables["entitySetElementType"] = GetPocoClrTypeName(entitySet.ElementType);
                            sourceWriter.WriteLine("IEntitySet<{entitySetElementType}> {entitySetName} { get; }");
                        }
                    }
                    sourceWriter.WriteLine();

                    sourceWriter.WriteLine("public partial class {containerName} : EFPocoContext<{adapterNamespace}.{containerName}Adapter>, I{containerName}");
                    using (sourceWriter.Indent())
                    {
                        sourceWriter.WriteLine("public {containerName}() : base(new {adapterNamespace}.{containerName}Adapter()) { }");
                        sourceWriter.WriteLine("public {containerName}(string connectionString) : base(new {adapterNamespace}.{containerName}Adapter(connectionString)) { }");
                        sourceWriter.WriteLine("public {containerName}(EntityConnection connection) : base(new {adapterNamespace}.{containerName}Adapter(connection)) { }");

                        foreach (EntitySet entitySet in container.BaseEntitySets.OfType<EntitySet>())
                        {
                            sourceWriter.Variables["entitySetName"] = entitySet.Name;
                            sourceWriter.WriteLine();
                            sourceWriter.Variables["entitySetElementType"] = GetPocoClrTypeName(entitySet.ElementType);
                            sourceWriter.WriteLine("public IEntitySet<{entitySetElementType}> {entitySetName}");
                            using (sourceWriter.Indent())
                            {
                                sourceWriter.WriteLine("get { return GetEntitySet<{entitySetElementType}>(\"{entitySetName}\"); }");
                            }
                        }
                    }
                }
            }
        }
        public override void Generate(SourceWriter sourceWriter)
        {
            WriteEntityContainers(sourceWriter);
        }

        public override void Generate(string outputDirectory)
        {
            WriteEntityContainers(outputDirectory);
        }
    }
}
