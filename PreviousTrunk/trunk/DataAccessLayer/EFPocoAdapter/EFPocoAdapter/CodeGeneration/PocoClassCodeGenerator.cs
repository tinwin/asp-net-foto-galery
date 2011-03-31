// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Data.Metadata.Edm;
using System.IO;
using System.Linq;

namespace EFPocoAdapter.CodeGeneration
{
    public class PocoClassCodeGenerator : CodeGeneratorBase
    {
        private void WritePocoComplexType(SourceWriter sourceWriter, ComplexType complexType, bool withNamespace)
        {
            sourceWriter.Variables["codeNamespace"] = GetPocoClrTypeNamespace(complexType);
            sourceWriter.Variables["pocoClassName"] = GetPocoClrTypeName(complexType);
            sourceWriter.Variables["className"] = GetBaseTypeName(GetPocoClrTypeName(complexType));
            sourceWriter.Variables["complexTypeName"] = complexType.Name;
            sourceWriter.Variables["complexTypeNamespace"] = complexType.NamespaceName;

            if (withNamespace)
            {
                if (!String.IsNullOrEmpty(sourceWriter.Variables["codeNamespace"]))
                {
                    sourceWriter.WriteLine("namespace {codeNamespace}");
                    sourceWriter.Indent();
                }
                WriteCommonUsings(sourceWriter);
            }
            sourceWriter.WriteLine();
            sourceWriter.WriteLine("public struct {className}");
            using (sourceWriter.Indent())
            {
                foreach (EdmProperty prop in complexType.Properties)
                {
                    sourceWriter.Variables["propertyName"] = prop.Name;
                    sourceWriter.Variables["isNullable"] = prop.Nullable ? "true" : "false";
                    sourceWriter.Variables["propertyType"] = GetPocoClrType(prop);
                    sourceWriter.WriteLine("public {propertyType} {propertyName} { get; set; }");
                }
            }
            if (withNamespace)
            {
                if (!String.IsNullOrEmpty(sourceWriter.Variables["codeNamespace"]))
                {
                    sourceWriter.Unindent();
                }
            }
        }
        private void WritePocoEntityType(SourceWriter sourceWriter, EntityType entityType, bool withNamespace)
        {
            sourceWriter.Variables["codeNamespace"] = GetPocoClrTypeNamespace(entityType);
            sourceWriter.Variables["entityTypeName"] = entityType.Name;
            sourceWriter.Variables["entityTypeNamespace"] = entityType.NamespaceName;
            sourceWriter.Variables["className"] = GetBaseTypeName(GetPocoClrTypeName(entityType));
            sourceWriter.Variables["baseClassName"] = entityType.BaseType != null ? " : " + GetPocoClrTypeName((StructuralType)entityType.BaseType) : "";
            sourceWriter.Variables["optionalAbstract"] = entityType.Abstract ? "abstract " : "";
            sourceWriter.Variables["optionalNew"] = entityType.BaseType != null ? "new " : "";
            sourceWriter.Variables["pocoClassName"] = GetPocoClrTypeName(entityType);
            sourceWriter.Variables["proxyClassName"] = GetProxyClrTypeName(entityType);

            if (withNamespace)
            {
                if (!String.IsNullOrEmpty(sourceWriter.Variables["codeNamespace"]))
                {
                    sourceWriter.WriteLine("namespace {codeNamespace}");
                    sourceWriter.Indent();
                }
                WriteCommonUsings(sourceWriter);
            }
            sourceWriter.WriteLine();
            sourceWriter.WriteLine("public {optionalAbstract}class {className}{baseClassName}");
            using (sourceWriter.Indent())
            {
                WriteScalarProperties(sourceWriter, entityType);
                WriteNavigationProperties(sourceWriter, entityType);
            }
            if (withNamespace)
            {
                if (!String.IsNullOrEmpty(sourceWriter.Variables["codeNamespace"]))
                {
                    sourceWriter.Unindent();
                }
            }
        }

        private void WriteScalarProperties(SourceWriter sourceWriter, EntityType entityType)
        {
            foreach (EdmProperty prop in entityType.Properties.Where(c => c.DeclaringType == entityType))
            {
                using (sourceWriter.NewVariableScope())
                {
                    sourceWriter.Variables["propertyName"] = prop.Name;
                    bool isKey = entityType.KeyMembers.Contains(prop);

                    sourceWriter.Variables["isKey"] = isKey ? "true" : "false";
                    sourceWriter.Variables["isNullable"] = prop.Nullable ? "true" : "false";
                    sourceWriter.Variables["optionalVirtual"] = "";
                    if (GetChangeTrackingMode(prop) == ChangeTrackingMode.Proxy)
                    {
                        sourceWriter.Variables["optionalVirtual"] = "virtual ";
                    }

                    sourceWriter.Variables["propertyType"] = GetPocoClrType(prop);
                    if (!IsHidden(prop))
                        sourceWriter.WriteLine("public {optionalVirtual}{propertyType} {propertyName} { get; set; }");
                }
            }
        }

        private void WriteNavigationProperties(SourceWriter sourceWriter, EntityType entityType)
        {
            foreach (NavigationProperty navprop in entityType.NavigationProperties.Where(c => c.DeclaringType == entityType))
            {
                using (sourceWriter.NewVariableScope())
                {
                    sourceWriter.Variables["propertyName"] = navprop.Name;
                    sourceWriter.Variables["associationNamespace"] = navprop.RelationshipType.NamespaceName;
                    sourceWriter.Variables["associationName"] = navprop.RelationshipType.Name;
                    sourceWriter.Variables["relatedEndName"] = navprop.ToEndMember.Name;
                    sourceWriter.Variables["optionalVirtual"] = "";
                    if (GetChangeTrackingMode(navprop) == ChangeTrackingMode.Proxy)
                    {
                        sourceWriter.Variables["optionalVirtual"] = "virtual ";
                    }
                    string itemType = GetPocoClrTypeName(((RefType)navprop.ToEndMember.TypeUsage.EdmType).ElementType);
                    if (navprop.ToEndMember.RelationshipMultiplicity == RelationshipMultiplicity.Many)
                    {
                        itemType = "IList<" + itemType + ">";
                    }
                    sourceWriter.Variables["propertyType"] = itemType;
                    if (!IsHidden(navprop))
                        sourceWriter.WriteLine("public {optionalVirtual}{propertyType} {propertyName} { get; set; }");
                }
            }
        }

        public override void Generate(SourceWriter sourceWriter)
        {
            var namespaces = Metadata.GetItems<StructuralType>().Select(c=>c.NamespaceName).Distinct();
            foreach (var ns in namespaces)
            {
                sourceWriter.Variables["namespaceName"] = ns;
                sourceWriter.WriteLine("namespace {namespaceName}");
                using (sourceWriter.Indent())
                {
                    WriteCommonUsings(sourceWriter);
                    foreach (ComplexType ct in Metadata.GetItems<ComplexType>().Where(c => c.NamespaceName == ns))
                    {
                        WritePocoComplexType(sourceWriter, ct, false);
                    }
                    foreach (EntityType et in Metadata.GetItems<EntityType>().Where(c => c.NamespaceName == ns))
                    {
                        WritePocoEntityType(sourceWriter, et, false);
                    }
                }
            }
        }

        private static void WriteCommonUsings(SourceWriter sourceWriter)
        {
            sourceWriter.WriteLine("using System;");
            sourceWriter.WriteLine("using System.Collections.Generic;");
        }

        public override void Generate(string outputDirectory)
        {
            WritePocoComplexTypes(outputDirectory);
            WritePocoEntityTypes(outputDirectory);
        }

        private void WritePocoEntityTypes(string outputDirectory)
        {
            foreach (EntityType et in Metadata.GetItems<EntityType>())
            {
                using (StreamWriter output = new StreamWriter(GetFileNameFor(outputDirectory, GetPocoClrTypeName(et))))
                {
                    SourceWriter sourceWriter = new SourceWriter(output);
                    WritePocoEntityType(sourceWriter, et, true);
                }
            }
        }

        private void WritePocoComplexTypes(string outputDirectory)
        {
            foreach (ComplexType ct in Metadata.GetItems<ComplexType>())
            {
                using (StreamWriter output = new StreamWriter(GetFileNameFor(outputDirectory, GetPocoClrTypeName(ct))))
                {
                    SourceWriter sourceWriter = new SourceWriter(output);
                    WritePocoComplexType(sourceWriter, ct, true);
                }
            }
        }
    }
}
