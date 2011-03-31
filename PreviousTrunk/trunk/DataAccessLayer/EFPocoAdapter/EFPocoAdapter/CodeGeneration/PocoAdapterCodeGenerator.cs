// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Data.Metadata.Edm;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;

namespace EFPocoAdapter.CodeGeneration
{
    public class PocoAdapterCodeGenerator : CodeGeneratorBase
    {
        public PocoAdapterCodeGenerator()
        {
        }

        private void WriteAdapterEntityContainers(string outputDirectory)
        {
            foreach (EntityContainer container in Metadata.GetItems<EntityContainer>())
            {
                using (StreamWriter output = new StreamWriter(
                    GetFileNameFor(outputDirectory, 
                        GetAdapterClrTypeNamespace(Metadata.GetItems<EntityType>().First()) + "." + container.Name )))
                {
                    SourceWriter sourceWriter = new SourceWriter(output);
                    WriteEntityContainer(sourceWriter, container, true);
                }
            }
        }

        private static void WriteCommonUsings(SourceWriter sourceWriter)
        {
            sourceWriter.WriteLine("using System;");
            sourceWriter.WriteLine("using System.Data;");
            sourceWriter.WriteLine("using System.Data.Objects;");
            sourceWriter.WriteLine("using System.Data.Objects.DataClasses;");
            sourceWriter.WriteLine("using System.Collections.Generic;");
            sourceWriter.WriteLine("using System.Reflection;");
            sourceWriter.WriteLine("using System.ComponentModel;");
            sourceWriter.WriteLine("using EFPocoAdapter;");
            sourceWriter.WriteLine("using EFPocoAdapter.DataClasses;");
        }

        private void WriteEntityContainer(SourceWriter sourceWriter, EntityContainer container, bool withNamespace)
        {
            sourceWriter.Variables["codeNamespace"] = GetAdapterClrTypeNamespace(Metadata.GetItems<EntityType>().First());
            if (withNamespace)
            {
                sourceWriter.WriteLine("namespace {codeNamespace}");
                sourceWriter.Indent();
                WriteCommonUsings(sourceWriter);
            }
            sourceWriter.WriteLine();
            using (sourceWriter.NewVariableScope())
            {
                sourceWriter.Variables["containerName"] = container.Name;
                sourceWriter.WriteLine("public partial class {containerName}Adapter : ObjectContext, IPocoAdapterObjectContext");
                using (sourceWriter.Indent())
                {
                    sourceWriter.WriteLine("private static QueryTranslationCache _qtc = new QueryTranslationCache();");

                    sourceWriter.WriteLine("static {containerName}Adapter()");
                    using (sourceWriter.Indent())
                    {
                        foreach (StructuralType st in Metadata.GetItems<StructuralType>().Where(c => c is EntityType || c is ComplexType))
                        {
                            sourceWriter.Variables["pocoTypeName"] = "global::" + GetPocoClrTypeName(st);
                            sourceWriter.Variables["adapterTypeName"] = "global::" + GetAdapterClrTypeName(st);
                            sourceWriter.Variables["creatorFunc"] = (st.Abstract || st is ComplexType) ? "null" : "(arg) => new " + sourceWriter.Variables["adapterTypeName"] + "((" + sourceWriter.Variables["pocoTypeName"] + ")arg)";
                            sourceWriter.WriteLine("_qtc.AddTypeMapping(typeof({pocoTypeName}), typeof({adapterTypeName}), {creatorFunc});");
                        }
                    }

                    sourceWriter.WriteLine("public {containerName}Adapter() : base(\"name={containerName}\", \"{containerName}\") { OnContextCreated(); }");
                    sourceWriter.WriteLine("public {containerName}Adapter(string connectionString) : base(connectionString, \"{containerName}\") { OnContextCreated(); }");
                    sourceWriter.WriteLine("public {containerName}Adapter(System.Data.EntityClient.EntityConnection connection) : base(connection, \"{containerName}\") { OnContextCreated(); }");

                    sourceWriter.WriteLine("partial void OnContextCreated();");

                    sourceWriter.WriteLine("public QueryTranslationCache QueryTranslationCache");
                    using (sourceWriter.Indent())
                    {
                        sourceWriter.WriteLine("get { return _qtc; }");
                    }

                    foreach (EntitySet entitySet in container.BaseEntitySets.OfType<EntitySet>())
                    {
                        sourceWriter.Variables["entitySetName"] = entitySet.Name;
                        sourceWriter.WriteLine();
                        sourceWriter.Variables["entitySetElementType"] = GetAdapterClrTypeName(entitySet.ElementType);
                        sourceWriter.WriteLine("private System.Data.Objects.ObjectQuery<{entitySetElementType}> _{entitySetName};");
                        sourceWriter.WriteLine("public System.Data.Objects.ObjectQuery<{entitySetElementType}> {entitySetName}");
                        using (sourceWriter.Indent())
                        {
                            sourceWriter.WriteLine("get");
                            using (sourceWriter.Indent())
                            {
                                sourceWriter.WriteLine("if (_{entitySetName} == null)");
                                sourceWriter.WriteLine("    _{entitySetName} = base.CreateQuery<{entitySetElementType}>(\"[{entitySetName}]\");");
                                sourceWriter.WriteLine("return _{entitySetName};");
                            }
                        }
                    }
                }
            }
            if (withNamespace)
            {
                sourceWriter.Unindent();
            }
        }

        private void WriteComplexTypes(string outputDirectory)
        {
            foreach (ComplexType complexType in Metadata.GetItems<ComplexType>())
            {
                string fileName = GetFileNameFor(outputDirectory, GetAdapterClrTypeName(complexType));
                using (StreamWriter output = new StreamWriter(fileName))
                {
                    SourceWriter sourceWriter = new SourceWriter(output);
                    WriteComplexType(sourceWriter, complexType, true);
                }
            }
        }

        private void WriteComplexType(SourceWriter sourceWriter, ComplexType complexType, bool withNamespace)
        {
            sourceWriter.Variables["codeNamespace"] = GetAdapterClrTypeNamespace(complexType);
            sourceWriter.Variables["pocoClassName"] = GetPocoClrTypeName(complexType);
            sourceWriter.Variables["className"] = GetBaseTypeName(GetAdapterClrTypeName(complexType));
            sourceWriter.Variables["complexTypeName"] = complexType.Name;
            sourceWriter.Variables["complexTypeNamespace"] = complexType.NamespaceName;

            if (withNamespace)
            {
                sourceWriter.WriteLine("namespace {codeNamespace}");
                sourceWriter.Indent();
                WriteCommonUsings(sourceWriter);
            }
            sourceWriter.WriteLine();
            sourceWriter.WriteLine("[EdmComplexType(NamespaceName=\"{complexTypeNamespace}\", Name=\"{complexTypeName}\")]");
            sourceWriter.WriteLine("public partial class {className} : ComplexObject, IComplexTypeAdapter<{pocoClassName}>");

            using (sourceWriter.Indent())
            {
                foreach (EdmProperty prop in complexType.Properties)
                {
                    sourceWriter.Variables["propertyName"] = prop.Name;
                    sourceWriter.Variables["propertyType"] = GetPocoClrType(prop);
                    sourceWriter.WriteLine("private {propertyType} _{propertyName};");
                }
                sourceWriter.WriteLine();
                sourceWriter.WriteLine("public {pocoClassName} CreatePocoStructure()");
                using (sourceWriter.Indent())
                {
                    EdmMember[] constructorArguments = GetConstructorArguments(complexType);
                    string constructorActualArguments = String.Join(", ", constructorArguments.Select(c => "this." + c.Name).ToArray());

                    sourceWriter.Variables["constructorActualArguments"] = constructorActualArguments;
                    sourceWriter.WriteLine("return new {pocoClassName}({constructorActualArguments}) {");
                    foreach (EdmProperty prop in complexType.Properties.Where(c=>!constructorArguments.Contains(c)))
                    {
                        sourceWriter.Variables["propertyName"] = prop.Name;
                        sourceWriter.WriteLine("    {propertyName} = this.{propertyName},");
                    }
                    sourceWriter.WriteLine("};");
                }
                sourceWriter.WriteLine("public void DetectChangesFrom({pocoClassName} pocoObject, IPocoAdapter parentObject, string propertyName)");
                using (sourceWriter.Indent())
                {
                    sourceWriter.WriteLine("bool different = false;");
                    foreach (EdmProperty prop in complexType.Properties)
                    {
                        sourceWriter.Variables["propertyName"] = prop.Name;
                        sourceWriter.WriteLine("if (this.{propertyName} != pocoObject.{propertyName}) different = true;");
                    }

                    sourceWriter.WriteLine("if (!different) return;");
                    sourceWriter.WriteLine("{pocoClassName} oldValue = CreatePocoStructure();");

                    foreach (EdmProperty prop in complexType.Properties)
                    {
                        sourceWriter.Variables["propertyName"] = prop.Name;
                        sourceWriter.WriteLine("this.{propertyName} = pocoObject.{propertyName};");
                    }
                    sourceWriter.WriteLine("{pocoClassName} newValue = CreatePocoStructure();");
                    sourceWriter.WriteLine("if (!Object.Equals(oldValue, newValue))");
                    using (sourceWriter.Indent())
                    {
                        //sourceWriter.WriteLine("parentObject.ReportPropertyChanging(propertyName, oldValue, newValue);");
                        sourceWriter.WriteLine("parentObject.RaiseChangeDetected(propertyName, oldValue, newValue);");
                    }
                }
                sourceWriter.WriteLine();
                foreach (EdmProperty prop in complexType.Properties)
                {
                    sourceWriter.Variables["propertyName"] = prop.Name;
                    sourceWriter.Variables["isNullable"] = prop.Nullable ? "true" : "false";
                    sourceWriter.Variables["propertyType"] = GetAdapterClrType(prop);
                    sourceWriter.WriteLine("[EdmScalarProperty(IsNullable={isNullable})]");
                    sourceWriter.WriteLine("public {propertyType} {propertyName}");
                    using (sourceWriter.Indent())
                    {
                        sourceWriter.WriteLine("get { return _{propertyName}; }");
                        sourceWriter.WriteLine("set");
                        using (sourceWriter.Indent())
                        {
                            sourceWriter.WriteLine("ReportPropertyChanging(\"{propertyName}\");");
                            sourceWriter.WriteLine("_{propertyName} = value;");
                            sourceWriter.WriteLine("ReportPropertyChanged(\"{propertyName}\");");
                        }
                    }
                }
                if (withNamespace)
                {
                    sourceWriter.Unindent();
                }
            }
        }

        private void WriteEntityTypeProxies(string outputDirectory)
        {
            foreach (EntityType entityType in Metadata.GetItems<EntityType>())
            {
                if (entityType.Abstract)
                    continue;

                string fileName = GetFileNameFor(outputDirectory, GetProxyClrTypeName(entityType));
                using (StreamWriter output = new StreamWriter(fileName))
                {
                    SourceWriter sourceWriter = new SourceWriter(output);

                    WriteEntityTypeProxy(sourceWriter, entityType, true);
                }
            }
        }

        private void WriteEntityTypes(string outputDirectory)
        {
            foreach (EntityType entityType in Metadata.GetItems<EntityType>())
            {
                string fileName = GetFileNameFor(outputDirectory, GetAdapterClrTypeName(entityType));
                using (StreamWriter output = new StreamWriter(fileName))
                {
                    SourceWriter sourceWriter = new SourceWriter(output);

                    WriteEntityType(sourceWriter, entityType, true);
                }
            }
        }

        private void WriteEntityTypeProxy(SourceWriter sourceWriter, EntityType entityType, bool withNamespace)
        {
            sourceWriter.Variables["entityTypeName"] = GetBaseTypeName(GetProxyClrTypeName(entityType));
            sourceWriter.Variables["codeNamespace"] = GetProxyClrTypeNamespace(entityType);
            sourceWriter.Variables["pocoClassName"] = GetPocoClrTypeName(entityType);
            sourceWriter.Variables["adapterClassName"] = GetAdapterClrTypeName(entityType);

            if (withNamespace)
            {
                sourceWriter.WriteLine("namespace {codeNamespace}");
                sourceWriter.Indent();
                WriteCommonUsings(sourceWriter);
            }
            sourceWriter.WriteLine();
            sourceWriter.WriteLine("public partial class {entityTypeName} : {pocoClassName}, IEntityProxy");
            using (sourceWriter.Indent())
            {
                sourceWriter.WriteLine("{adapterClassName} _adapter;");
                sourceWriter.WriteLine("IPocoAdapter IEntityProxy.Adapter");
                using (sourceWriter.Indent())
                {
                    sourceWriter.WriteLine("get { return _adapter; }");
                    sourceWriter.WriteLine("set { _adapter = ({adapterClassName})value; }");
                }

                StringBuilder formalParameters = new StringBuilder();
                StringBuilder actualParameters = new StringBuilder();

                foreach (EdmMember member in GetConstructorArguments(entityType))
                {
                    EdmProperty prop = member as EdmProperty;
                    NavigationProperty navprop = member as NavigationProperty;
                    if (prop != null)
                        formalParameters.AppendFormat("{0} {1}, ", GetPocoClrType(prop), "p" + member.Name);
                    if (navprop != null)
                        formalParameters.AppendFormat("{0} {1}, ", GetPocoClrType(navprop), "p" + member.Name);

                    if (actualParameters.Length > 0)
                        actualParameters.Append(", ");
                    actualParameters.AppendFormat("p{0}", member.Name);
                }
                sourceWriter.Variables["constructorFormalParameters"] = formalParameters.ToString();
                sourceWriter.Variables["baseConstructorActualParameters"] = actualParameters.ToString();

                sourceWriter.WriteLine("public {entityTypeName}({constructorFormalParameters}{adapterClassName} adapter) : base({baseConstructorActualParameters}) { _adapter = adapter; OnProxyCreated(); }");
                sourceWriter.WriteLine("partial void OnProxyCreated();");

                foreach (EdmProperty prop in entityType.Properties.Where(c => !IsHidden(c)))
                {
                    WriteProxyPropertyOverride(sourceWriter, prop);
                }
                foreach (NavigationProperty navprop in entityType.NavigationProperties.Where(c => !IsHidden(c)))
                {
                    WriteProxyNavigationPropertyOverride(sourceWriter, navprop);
                }
            }
            if (withNamespace)
            {
                sourceWriter.Unindent();
            }
        }

        private void WriteProxyNavigationPropertyOverride(SourceWriter sourceWriter, NavigationProperty navprop)
        {
            if (GetChangeTrackingMode(navprop) == ChangeTrackingMode.Proxy && navprop.ToEndMember.RelationshipMultiplicity != RelationshipMultiplicity.Many)
            {
                sourceWriter.Variables["propertyName"] = navprop.Name;
                sourceWriter.Variables["associationNamespace"] = navprop.RelationshipType.NamespaceName;
                sourceWriter.Variables["associationName"] = navprop.RelationshipType.Name;
                sourceWriter.Variables["relatedEndName"] = navprop.ToEndMember.Name;
                sourceWriter.Variables["pocoItemType"] = GetPocoClrTypeName(((RefType)navprop.ToEndMember.TypeUsage.EdmType).ElementType);
                sourceWriter.Variables["adapterItemType"] = GetAdapterClrTypeName(((RefType)navprop.ToEndMember.TypeUsage.EdmType).ElementType);

                sourceWriter.WriteLine("public override {pocoItemType} {propertyName}");
                using (sourceWriter.Indent())
                {
                    //LoadOnDemand
                    sourceWriter.WriteLine("get");
                    using (sourceWriter.Indent())
                    {
                        sourceWriter.WriteLine("IEntityWithRelationships iewr = _adapter as IEntityWithRelationships;");
                        sourceWriter.WriteLine("EntityReference<{adapterItemType}> reference = iewr.RelationshipManager.GetRelatedReference<{adapterItemType}>(\"{associationName}\",\"{relatedEndName}\");");
                        sourceWriter.WriteLine("if (!reference.IsLoaded && _adapter.CanLoadProperty(\"{propertyName}\"))");
                        using (sourceWriter.Indent())
                        {
                            sourceWriter.WriteLine("using (ThreadLocalContext.Set(_adapter.Context)) reference.Load();");
                        }
                        sourceWriter.WriteLine("return this._adapter.{propertyName}.GetPocoEntityOrNull();");
                    }

                    sourceWriter.WriteLine("set");
                    using (sourceWriter.Indent())
                    {
                        sourceWriter.WriteLine("IEntityWithRelationships iewr = _adapter as IEntityWithRelationships;");
                        sourceWriter.WriteLine("EntityReference<{adapterItemType}> reference = iewr.RelationshipManager.GetRelatedReference<{adapterItemType}>(\"{associationName}\",\"{relatedEndName}\");");
                        sourceWriter.WriteLine("var newValue = _adapter.Context.GetAdapterObject<{adapterItemType}>(value);");
                        sourceWriter.WriteLine("if (!Object.ReferenceEquals(reference.Value, newValue))");
                        using (sourceWriter.Indent())
                        {
                            sourceWriter.WriteLine("_adapter.Context.RaiseChangeDetected(this, \"{propertyName}\", reference.Value.GetPocoEntityOrNull(), newValue.GetPocoEntityOrNull());");
                            sourceWriter.WriteLine("reference.Value = newValue;");
                        }
                    }
                }
            }
        }

        private void WriteProxyPropertyOverride(SourceWriter sourceWriter, EdmProperty prop)
        {
            if (GetChangeTrackingMode(prop) == ChangeTrackingMode.Proxy)
            {
                sourceWriter.Variables["propertyType"] = GetPocoClrType(prop);
                sourceWriter.Variables["propertyName"] = prop.Name;
                if (prop.TypeUsage.EdmType is ComplexType)
                {
                    sourceWriter.Variables["complexTypePocoClass"] = GetPocoClrTypeName((ComplexType)prop.TypeUsage.EdmType);
                    sourceWriter.WriteLine("public override {complexTypePocoClass} {propertyName}");
                    using (sourceWriter.Indent())
                    {
                        sourceWriter.WriteLine("get { return this._adapter.{propertyName}.CreatePocoStructure(); }");
                        sourceWriter.WriteLine("set { base.{propertyName} = value; this._adapter.{propertyName}.DetectChangesFrom(value, _adapter, \"{propertyName}\"); }");
                    }
                }
                else
                {
                    sourceWriter.WriteLine("public override {propertyType} {propertyName}");
                    using (sourceWriter.Indent())
                    {
                        sourceWriter.WriteLine("get { return this._adapter.{propertyName}; }");
                        sourceWriter.WriteLine("set");
                        using (sourceWriter.Indent())
                        {
                            sourceWriter.WriteLine("base.{propertyName} = value;");
                            sourceWriter.WriteLine("if (this._adapter.Context != null && value != this._adapter.{propertyName})");
                            using (sourceWriter.Indent())
                            {
                                sourceWriter.WriteLine("_adapter.Context.RaiseChangeDetected(this, \"{propertyName}\", this._adapter.{propertyName}, value);");
                            }
                            sourceWriter.WriteLine("this._adapter.{propertyName} = value;");
                        }
                    }
                }
            }
        }

        private void WriteEntityType(SourceWriter sourceWriter, EntityType entityType, bool withNamespace)
        {
            sourceWriter.Variables["codeNamespace"] = GetAdapterClrTypeNamespace(entityType);
            sourceWriter.Variables["entityTypeName"] = entityType.Name;
            sourceWriter.Variables["entityTypeNamespace"] = entityType.NamespaceName;
            sourceWriter.Variables["className"] = GetBaseTypeName(GetAdapterClrTypeName(entityType));
            sourceWriter.Variables["baseClassName"] = " : " + ((entityType.BaseType != null ? GetAdapterClrTypeName((StructuralType)entityType.BaseType) : "PocoAdapterBase<" + GetPocoClrTypeName(entityType) + ">"));
            sourceWriter.Variables["optionalAbstract"] = entityType.Abstract ? "abstract " : "";
            sourceWriter.Variables["optionalNew"] = entityType.BaseType != null ? "new " : "";
            sourceWriter.Variables["pocoClassName"] = GetPocoClrTypeName(entityType);
            sourceWriter.Variables["proxyClassName"] = GetProxyClrTypeName(entityType);

            if (withNamespace)
            {
                sourceWriter.WriteLine("namespace {codeNamespace}");
                sourceWriter.Indent();
                WriteCommonUsings(sourceWriter);
            }
            sourceWriter.WriteLine();
            sourceWriter.WriteLine(@"[EdmEntityType(NamespaceName=""{entityTypeNamespace}"", Name=""{entityTypeName}"")]");
            sourceWriter.WriteLine("public {optionalAbstract}partial class {className}{baseClassName}, IPocoAdapter<{pocoClassName}>");
            using (sourceWriter.Indent())
            {
                WriteDataProperties(sourceWriter, entityType);
                WriteConstructors(sourceWriter, entityType);
                WriteInit(sourceWriter, entityType);
                WriteInitCollections(sourceWriter, entityType);
                WriteAssociationChangedMethods(sourceWriter, entityType);
                WriteDetectChangesMethod(sourceWriter, entityType);
                WritePopulatePocoEntityMethod(sourceWriter, entityType);
                WritePrivateFields(sourceWriter, entityType);
                WriteProperties(sourceWriter, entityType);
                WriteNavigationProperties(sourceWriter, entityType);
            }
            if (withNamespace)
            {
                sourceWriter.Unindent();
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
                    string itemType = GetAdapterClrTypeName(((RefType)navprop.ToEndMember.TypeUsage.EdmType).ElementType);
                    sourceWriter.Variables["itemType"] = itemType;
                    if (navprop.ToEndMember.RelationshipMultiplicity == RelationshipMultiplicity.Many)
                    {
                        itemType = "EntityCollection<" + itemType + ">";
                    }
                    sourceWriter.Variables["propertyType"] = itemType;
                    sourceWriter.WriteLine();
                    sourceWriter.WriteLine("[EdmRelationshipNavigationProperty(\"{associationNamespace}\", \"{associationName}\", \"{relatedEndName}\")]");

                    sourceWriter.WriteLine("public {propertyType} {propertyName}");
                    using (sourceWriter.Indent())
                    {
                        if (navprop.ToEndMember.RelationshipMultiplicity != RelationshipMultiplicity.Many)
                        {
                            sourceWriter.WriteLine("get { return this.{propertyName}Reference.Value; }");
                            sourceWriter.WriteLine("set { this.{propertyName}Reference.Value = value; }");
                        }
                        else
                        {
                            sourceWriter.WriteLine("get");
                            using (sourceWriter.Indent())
                            {
                                sourceWriter.WriteLine("if (_{propertyName}CollectionCache == null)");
                                sourceWriter.WriteLine("    _{propertyName}CollectionCache = GetRelatedCollection<{itemType}>(\"{associationName}\", \"{relatedEndName}\");");
                                sourceWriter.WriteLine(" return _{propertyName}CollectionCache;");
                            }
                        }
                    }

                    if (navprop.ToEndMember.RelationshipMultiplicity != RelationshipMultiplicity.Many)
                    {
                        sourceWriter.WriteLine();
                        sourceWriter.WriteLine("private EntityReference<{propertyType}> _{propertyName}ReferenceCache = null;");
                        sourceWriter.WriteLine("public EntityReference<{propertyType}> {propertyName}Reference");
                        using (sourceWriter.Indent())
                        {
                            sourceWriter.WriteLine("get");
                            using (sourceWriter.Indent())
                            {
                                sourceWriter.WriteLine("if (_{propertyName}ReferenceCache == null)");
                                sourceWriter.WriteLine("    _{propertyName}ReferenceCache = GetRelatedReference<{itemType}>(\"{associationName}\", \"{relatedEndName}\");");
                                sourceWriter.WriteLine("return _{propertyName}ReferenceCache;");
                            }
                        }
                    }
                    else
                    {
                        sourceWriter.WriteLine("private {propertyType} _{propertyName}CollectionCache = null;");
                    }
                }
            }
        }

        private void WriteProperties(SourceWriter sourceWriter, EntityType entityType)
        {
            foreach (EdmProperty prop in entityType.Properties.Where(c => c.DeclaringType == entityType))
            {
                using (sourceWriter.NewVariableScope())
                {
                    WriteProperty(sourceWriter, entityType, prop);
                }
            }
        }

        private void WriteProperty(SourceWriter sourceWriter, EntityType entityType, EdmProperty prop)
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

            sourceWriter.WriteLine();
            if (prop.TypeUsage.EdmType is PrimitiveType)
            {
                sourceWriter.WriteLine("[EdmScalarProperty(EntityKeyProperty={isKey}, IsNullable={isNullable})]");
            }
            else if (prop.TypeUsage.EdmType is ComplexType)
            {
                sourceWriter.WriteLine("[EdmComplexProperty]");
            }
            else
                throw new NotSupportedException("Invalid property type");

            sourceWriter.Variables["propertyType"] = GetAdapterClrType(prop);
            sourceWriter.WriteLine("public {propertyType} {propertyName}");
            using (sourceWriter.Indent())
            {
                sourceWriter.WriteLine("get");
                using (sourceWriter.Indent())
                {
                    if (prop.TypeUsage.EdmType is ComplexType)
                    {
                        sourceWriter.WriteLine("this._{propertyName} = this.GetValidValue(this._{propertyName}, \"{propertyName}\", false, this._{propertyName}_Initialized);");
                        sourceWriter.WriteLine("this._{propertyName}_Initialized = true;");
                        sourceWriter.WriteLine("this._{propertyName}.PropertyChanged += new PropertyChangedEventHandler(_{propertyName}_PropertyChanged);");
                    }
                    sourceWriter.WriteLine("return _{propertyName};");
                }
                sourceWriter.WriteLine("set");
                using (sourceWriter.Indent())
                {
                    if (GetChangeTrackingMode(prop) == ChangeTrackingMode.Proxy)
                    {
                        sourceWriter.WriteLine("if (_pocoEntity != null && !(_pocoEntity is IEntityProxy))");
                    }
                    else
                    {
                        sourceWriter.WriteLine("if (_pocoEntity != null)");
                    }
                    using (sourceWriter.Indent())
                    {
                        if (prop.TypeUsage.EdmType is ComplexType)
                        {
                            sourceWriter.WriteLine("PocoEntity.{propertyName} = value.CreatePocoStructure();");
                        }
                        else
                        {
                            if (IsSettable(prop))
                                sourceWriter.WriteLine("PocoEntity.{propertyName} = value;");
                        }
                    }
                    sourceWriter.WriteLine("ReportPropertyChanging(\"{propertyName}\");");
                    if (prop.TypeUsage.EdmType is ComplexType)
                    {
                        sourceWriter.WriteLine("this._{propertyName} = this.SetValidValue(this._{propertyName}, value, \"{propertyName}\");");
                        sourceWriter.WriteLine("this._{propertyName}_Initialized = true;");
                        sourceWriter.WriteLine("this._{propertyName}.PropertyChanged += new PropertyChangedEventHandler(_{propertyName}_PropertyChanged);");
                    }
                    else
                    {
                        sourceWriter.WriteLine("_{propertyName} = value;");
                    }
                    sourceWriter.WriteLine("ReportPropertyChanged(\"{propertyName}\");");
                }
            };
            if (prop.TypeUsage.EdmType is ComplexType)
            {
                sourceWriter.WriteLine("void _{propertyName}_PropertyChanged(object sender, PropertyChangedEventArgs e)");
                using (sourceWriter.Indent())
                {
                    sourceWriter.WriteLine("if (_pocoEntity != null)");
                    using (sourceWriter.Indent())
                    {
                        sourceWriter.WriteLine("PocoEntity.{propertyName} = _{propertyName}.CreatePocoStructure();");
                    }
                }
            }
        }

        private void WritePrivateFields(SourceWriter sourceWriter, EntityType entityType)
        {
            foreach (EdmProperty prop in entityType.Properties.Where(c => c.DeclaringType == entityType).Where(prop => entityType.KeyMembers.Contains(prop)))
            {
                using (sourceWriter.NewVariableScope())
                {
                    sourceWriter.Variables["propertyType"] = GetAdapterClrType(prop);
                    sourceWriter.Variables["propertyName"] = prop.Name;
                    sourceWriter.WriteLine("private {propertyType} _{propertyName};");
                }
            }
        }

        #region Adapter object DetectChanges()

        private void WriteDetectChangesMethod(SourceWriter sourceWriter, EntityType entityType)
        {
            if (entityType.Properties.Where(c => c.DeclaringType == entityType).Count() + entityType.NavigationProperties.Where(c => c.DeclaringType == entityType).Count() > 0)
            {
                sourceWriter.WriteLine("public override void DetectChanges()");
                using (sourceWriter.Indent())
                {
                    sourceWriter.WriteLine("base.DetectChanges();");
                    WriteDetectChangesForScalarProperties(sourceWriter, entityType, c => GetChangeTrackingMode(c) != ChangeTrackingMode.Proxy);
                    WriteDetectChangesForNavigationProperties(sourceWriter, entityType, c => GetChangeTrackingMode(c) != ChangeTrackingMode.Proxy);
                    sourceWriter.WriteLine("if (!(PocoEntity is IEntityProxy))");
                    using (sourceWriter.Indent())
                    {
                        WriteDetectChangesForScalarProperties(sourceWriter, entityType, c => GetChangeTrackingMode(c) == ChangeTrackingMode.Proxy);
                        WriteDetectChangesForNavigationProperties(sourceWriter, entityType, c => GetChangeTrackingMode(c) == ChangeTrackingMode.Proxy);
                    }
                }
            }
        }

        private void WriteDetectChangesForScalarProperties(SourceWriter sourceWriter, EntityType entityType, Func<EdmMember,bool> filter)
        {
            foreach (EdmProperty prop in entityType.Properties.Where(c => c.DeclaringType == entityType && !IsHidden(c) && filter(c)))
            {
                sourceWriter.Variables["propertyName"] = prop.Name;
                if (prop.TypeUsage.EdmType is ComplexType)
                {
                    sourceWriter.WriteLine("this.{propertyName}.DetectChangesFrom(PocoEntity.{propertyName}, this, \"{propertyName}\");");
                }
                else
                {
                    sourceWriter.WriteLine("DetectChanges(PocoEntity.{propertyName}, ref _{propertyName}, \"{propertyName}\");");
                }
            }
        }

        private void WriteDetectChangesForNavigationProperties(SourceWriter sourceWriter, EntityType entityType, Func<EdmMember, bool> filter)
        {
            foreach (NavigationProperty navprop in entityType.NavigationProperties.Where(c => c.DeclaringType == entityType && !IsHidden(c) && filter(c)))
            {
                sourceWriter.Variables["propertyName"] = navprop.Name;
                sourceWriter.Variables["associationNamespace"] = navprop.RelationshipType.NamespaceName;
                sourceWriter.Variables["associationName"] = navprop.RelationshipType.Name;
                sourceWriter.Variables["relatedEndName"] = navprop.ToEndMember.Name;
                sourceWriter.Variables["itemType"] = GetAdapterClrTypeName(((RefType)navprop.ToEndMember.TypeUsage.EdmType).ElementType);
                if (navprop.ToEndMember.RelationshipMultiplicity == RelationshipMultiplicity.Many)
                {
                    sourceWriter.WriteLine("DetectChanges(PocoEntity.{propertyName}, this.{propertyName}, \"{propertyName}\");");
                }
                else
                {
                    if (IsSettable(navprop))
                        sourceWriter.WriteLine("DetectChanges(PocoEntity.{propertyName}, this.{propertyName}Reference, \"{propertyName}\");");
                }
            }
        }

        #endregion

        private void WritePopulatePocoEntityMethod(SourceWriter sourceWriter, EntityType entityType)
        {
            if (entityType.Properties.Where(c => c.DeclaringType == entityType).Count() + entityType.NavigationProperties.Where(c => c.DeclaringType == entityType).Count() > 0)
            {
                sourceWriter.WriteLine("public override void PopulatePocoEntity(bool enableProxies)");
                using (sourceWriter.Indent())
                {
                    WritePopulatePocoEntityInit(sourceWriter, entityType);
                    sourceWriter.WriteLine("base.PopulatePocoEntity(enableProxies);");
                    WritePopulatePocoEntityForScalarProperties(sourceWriter, entityType, c => GetChangeTrackingMode(c) != ChangeTrackingMode.Proxy && IsSettable(c));
                    WritePopulatePocoEntityForNavigationProperties(sourceWriter, entityType, c => GetChangeTrackingMode(c) != ChangeTrackingMode.Proxy && IsSettable(c));
                    sourceWriter.WriteLine("if (!(PocoEntity is IEntityProxy))");
                    using (sourceWriter.Indent())
                    {
                        WritePopulatePocoEntityForScalarProperties(sourceWriter, entityType, c => GetChangeTrackingMode(c) == ChangeTrackingMode.Proxy && IsSettable(c));
                        WritePopulatePocoEntityForNavigationProperties(sourceWriter, entityType, c => GetChangeTrackingMode(c) == ChangeTrackingMode.Proxy && IsSettable(c));
                    }
                }
            }
        }

        private void WritePopulatePocoEntityInit(SourceWriter sourceWriter, EntityType entityType)
        {
            EdmMember[] constructorArguments = GetConstructorArguments(entityType);
            if (!entityType.Abstract)
            {
                StringBuilder sb = new StringBuilder();
                foreach (EdmMember mem in constructorArguments)
                {
                    if (sb.Length > 0)
                        sb.Append(", ");
                    if (mem.TypeUsage.EdmType is ComplexType)
                    {
                        sb.AppendFormat("{0}.CreatePocoStructure()", mem.Name);
                    }
                    else if (mem is NavigationProperty)
                    {
                        if (((NavigationProperty)mem).ToEndMember.RelationshipMultiplicity == RelationshipMultiplicity.Many)
                        {
                            sb.AppendFormat("ConvertTo<{0},{1}>({2})", GetCollectionTypeName((NavigationProperty)mem), GetPocoClrTypeName(((RefType)((NavigationProperty)mem).ToEndMember.TypeUsage.EdmType).ElementType), mem.Name);
                        }
                        else
                        {
                            sb.AppendFormat("{0}.GetPocoEntityOrNull()", mem.Name);
                        }
                    }
                    else
                    {
                        sb.AppendFormat("{0}", mem.Name);
                    }
                }
                sourceWriter.Variables["actualConstructorArguments"] = sb.ToString();

                sourceWriter.WriteLine("if (_pocoEntity == null)");
                using (sourceWriter.Indent())
                {
                    sourceWriter.WriteLine("if (!enableProxies)");
                    using (sourceWriter.Indent())
                    {
                        sourceWriter.WriteLine("_pocoEntity = new {pocoClassName}({actualConstructorArguments}); // poco");
                    }
                    sourceWriter.WriteLine("else");
                    using (sourceWriter.Indent())
                    {
                        if (sourceWriter.Variables["actualConstructorArguments"].Length > 0)
                            sourceWriter.Variables["actualConstructorArguments"] += ", ";

                        sourceWriter.WriteLine("_pocoEntity = new {proxyClassName}({actualConstructorArguments}this); // proxy");
                    }
                    sourceWriter.WriteLine("RegisterAdapterInContext();");
                    sourceWriter.WriteLine("InitCollections(enableProxies);");
                }
            }
        }

        private void WritePopulatePocoEntityForScalarProperties(SourceWriter sourceWriter, EntityType entityType, Func<EdmMember, bool> filter)
        {
            foreach (EdmProperty prop in entityType.Properties.Where(c => c.DeclaringType == entityType && !IsHidden(c) && filter(c)))
            {
                sourceWriter.Variables["propertyName"] = prop.Name;
                if (prop.TypeUsage.EdmType is ComplexType)
                {
                    sourceWriter.WriteLine("PocoEntity.{propertyName} = _{propertyName}.CreatePocoStructure();");
                }
                else
                {
                    sourceWriter.WriteLine("PocoEntity.{propertyName} = _{propertyName};");
                }
            }
        }

        private void WritePopulatePocoEntityForNavigationProperties(SourceWriter sourceWriter, EntityType entityType, Func<EdmMember, bool> filter)
        {
            foreach (NavigationProperty navprop in entityType.NavigationProperties.Where(c => c.DeclaringType == entityType && !IsHidden(c) && filter(c)))
            {
                sourceWriter.Variables["propertyName"] = navprop.Name;
                sourceWriter.Variables["associationNamespace"] = navprop.RelationshipType.NamespaceName;
                sourceWriter.Variables["associationName"] = navprop.RelationshipType.Name;
                sourceWriter.Variables["relatedEndName"] = navprop.ToEndMember.Name;
                sourceWriter.Variables["itemType"] = GetAdapterClrTypeName(((RefType)navprop.ToEndMember.TypeUsage.EdmType).ElementType);
                sourceWriter.Variables["pocoItemType"] = GetPocoClrTypeName(((RefType)navprop.ToEndMember.TypeUsage.EdmType).ElementType);
                sourceWriter.Variables["adapterItemType"] = GetAdapterClrTypeName(((RefType)navprop.ToEndMember.TypeUsage.EdmType).ElementType);
                if (navprop.ToEndMember.RelationshipMultiplicity == RelationshipMultiplicity.Many)
                {
                    sourceWriter.WriteLine("UpdateCollection(PocoEntity.{propertyName}, this.{propertyName});");
                }
                else
                {
                    sourceWriter.WriteLine("PocoEntity.{propertyName} = this.{propertyName}Reference.Value.GetPocoEntityOrNull();");
                }
            }
        }

        private void WriteInit(SourceWriter sourceWriter, EntityType entityType)
        {
            if (entityType.Properties.Where(c => c.DeclaringType == entityType).Count() + entityType.NavigationProperties.Where(c => c.DeclaringType == entityType).Count() > 0)
            {
                sourceWriter.WriteLine("public override void Init()");
                using (sourceWriter.Indent())
                {
                    sourceWriter.WriteLine("base.Init();");

                    foreach (NavigationProperty navprop in entityType.NavigationProperties.Where(c => c.DeclaringType == entityType && !IsHidden(c)))
                    {
                        using (sourceWriter.NewVariableScope())
                        {
                            sourceWriter.Variables["propertyName"] = navprop.Name;
                            sourceWriter.Variables["associationNamespace"] = navprop.RelationshipType.NamespaceName;
                            sourceWriter.Variables["associationName"] = navprop.RelationshipType.Name;
                            sourceWriter.Variables["relatedEndName"] = navprop.ToEndMember.Name;
                            sourceWriter.Variables["pocoItemType"] = GetPocoClrTypeName(((RefType)navprop.ToEndMember.TypeUsage.EdmType).ElementType);
                            sourceWriter.Variables["adapterItemType"] = GetAdapterClrTypeName(((RefType)navprop.ToEndMember.TypeUsage.EdmType).ElementType);

                            if (navprop.ToEndMember.RelationshipMultiplicity == RelationshipMultiplicity.Many)
                            {
                                sourceWriter.WriteLine("this.{propertyName}.AssociationChanged += {propertyName}_Changed;");
                            }
                            else
                            {
                                if (IsSettable(navprop))
                                    sourceWriter.WriteLine("this.{propertyName}Reference.AssociationChanged += {propertyName}_Changed;");
                            }
                        }
                    }
                }
            }
        }

        private void WriteInitCollections(SourceWriter sourceWriter, EntityType entityType)
        {
            if (entityType.Properties.Where(c => c.DeclaringType == entityType).Count() + entityType.NavigationProperties.Where(c => c.DeclaringType == entityType).Count() > 0)
            {
                sourceWriter.WriteLine("public override void InitCollections(bool enableProxies)");
                using (sourceWriter.Indent())
                {
                    sourceWriter.WriteLine("if (_pocoEntity == null) return;");
                    sourceWriter.WriteLine("base.InitCollections(enableProxies);");
                    foreach (NavigationProperty navprop in entityType.NavigationProperties.Where(c => c.DeclaringType == entityType && !IsHidden(c) && IsSettable(c)))
                    {
                        using (sourceWriter.NewVariableScope())
                        {
                            sourceWriter.Variables["propertyName"] = navprop.Name;
                            sourceWriter.Variables["associationNamespace"] = navprop.RelationshipType.NamespaceName;
                            sourceWriter.Variables["associationName"] = navprop.RelationshipType.Name;
                            sourceWriter.Variables["relatedEndName"] = navprop.ToEndMember.Name;
                            sourceWriter.Variables["pocoItemType"] = GetPocoClrTypeName(((RefType)navprop.ToEndMember.TypeUsage.EdmType).ElementType);
                            sourceWriter.Variables["adapterItemType"] = GetAdapterClrTypeName(((RefType)navprop.ToEndMember.TypeUsage.EdmType).ElementType);
                            sourceWriter.Variables["collectionConcreteType"] = GetCollectionTypeName(navprop);

                            if (navprop.ToEndMember.RelationshipMultiplicity == RelationshipMultiplicity.Many)
                            {
                                sourceWriter.WriteLine("if (PocoEntity.{propertyName} == null)");
                                using (sourceWriter.Indent())
                                {
                                    if (GetChangeTrackingMode(navprop) == ChangeTrackingMode.Proxy)
                                    {
                                        sourceWriter.WriteLine("if (enableProxies)");
                                        using (sourceWriter.Indent())
                                        {
                                            sourceWriter.WriteLine("PocoEntity.{propertyName} = new IListEntityCollectionAdapter<{pocoItemType},{adapterItemType}>(this, \"{propertyName}\", this.{propertyName}, this.Context);");
                                        }
                                        sourceWriter.WriteLine("else");
                                        using (sourceWriter.Indent())
                                        {
                                            sourceWriter.WriteLine("PocoEntity.{propertyName} = new {collectionConcreteType}();");
                                        }
                                    }
                                    else
                                        sourceWriter.WriteLine("PocoEntity.{propertyName} = new {collectionConcreteType}();");
                                }
                            }
                        }
                    }
                }
            }
        }

        private void WriteAssociationChangedMethods(SourceWriter sourceWriter, EntityType entityType)
        {
            foreach (NavigationProperty navprop in entityType.NavigationProperties.Where(c => c.DeclaringType == entityType && !IsHidden(c)))
            {
                using (sourceWriter.NewVariableScope())
                {
                    sourceWriter.Variables["propertyName"] = navprop.Name;
                    //sourceWriter.WriteLine("GetRelationshipManager().");
                    sourceWriter.Variables["associationNamespace"] = navprop.RelationshipType.NamespaceName;
                    sourceWriter.Variables["associationName"] = navprop.RelationshipType.Name;
                    sourceWriter.Variables["relatedEndName"] = navprop.ToEndMember.Name;
                    sourceWriter.Variables["pocoItemType"] = GetPocoClrTypeName(((RefType)navprop.ToEndMember.TypeUsage.EdmType).ElementType);
                    sourceWriter.Variables["adapterItemType"] = GetAdapterClrTypeName(((RefType)navprop.ToEndMember.TypeUsage.EdmType).ElementType);

                    if (navprop.ToEndMember.RelationshipMultiplicity != RelationshipMultiplicity.Many && !IsSettable(navprop))
                        continue;

                    sourceWriter.WriteLine("void {propertyName}_Changed(object sender, System.ComponentModel.CollectionChangeEventArgs e)");
                    using (sourceWriter.Indent())
                    {
                        sourceWriter.WriteLine("if (IsDetectingChanges) return;");
                        sourceWriter.WriteLine("if (_pocoEntity == null) return;");
                        if (navprop.ToEndMember.RelationshipMultiplicity == RelationshipMultiplicity.Many)
                        {
                            if (GetChangeTrackingMode(navprop) == ChangeTrackingMode.Proxy)
                            {
                                sourceWriter.WriteLine("if (PocoEntity.{propertyName} is IListEntityCollectionAdapter<{pocoItemType},{adapterItemType}>) return;");
                            }
                            sourceWriter.WriteLine("switch (e.Action)");
                            using (sourceWriter.Indent())
                            {
                                sourceWriter.WriteLine("case CollectionChangeAction.Add:");
                                sourceWriter.WriteLine("    PocoEntity.{propertyName}.Add((({adapterItemType})e.Element).PocoEntity);");
                                sourceWriter.WriteLine("    break;");

                                sourceWriter.WriteLine("case CollectionChangeAction.Remove:");
                                sourceWriter.WriteLine("    PocoEntity.{propertyName}.Remove((({adapterItemType})e.Element).PocoEntity);");
                                sourceWriter.WriteLine("    break;");

                                sourceWriter.WriteLine("case CollectionChangeAction.Refresh:");
                                sourceWriter.WriteLine("default:");
                                sourceWriter.WriteLine("    UpdateCollection(PocoEntity.{propertyName}, this.{propertyName});");
                                sourceWriter.WriteLine("    break;");
                            }
                        }
                        else
                        {
                            if (GetChangeTrackingMode(navprop) == ChangeTrackingMode.Proxy)
                            {
                                sourceWriter.WriteLine("if (PocoEntity is IEntityProxy) return;");
                            }
                            sourceWriter.WriteLine("PocoEntity.{propertyName} = this.{propertyName}.GetPocoEntityOrNull();");
                        }

                    }
                }
            }
        }
        private void WriteConstructors(SourceWriter sourceWriter, EntityType entityType)
        {
            sourceWriter.WriteLine("public {className}() { OnAdapterCreated(); }");
            sourceWriter.WriteLine("public {className}({pocoClassName} pocoObject) : base(pocoObject) { OnAdapterCreated(); }");
            sourceWriter.WriteLine();
            sourceWriter.WriteLine("partial void OnAdapterCreated();");

            if (!entityType.Abstract)
            {
                if (HasDefaultConstructor(entityType))
                {
                    sourceWriter.WriteLine("public override object CreatePocoEntity() { return new {pocoClassName}(); }");
                    sourceWriter.WriteLine("public override object CreatePocoEntityProxy() { return new {proxyClassName}(this); }");
                }
                else
                {
                    sourceWriter.WriteLine("public override object CreatePocoEntity() { return null; }");
                    sourceWriter.WriteLine("public override object CreatePocoEntityProxy() { return null; }");
                }
            }

            if (entityType.BaseType != null)
            {
                sourceWriter.WriteLine("public new {pocoClassName} PocoEntity { get { return ({pocoClassName})base.PocoEntity; } }");
            }
        }

        private void WriteDataProperties(SourceWriter sourceWriter, EntityType entityType)
        {
            foreach (EdmProperty prop in entityType.Properties.Where(c => c.DeclaringType == entityType).Where(prop => !entityType.KeyMembers.Contains(prop)))
            {
                using (sourceWriter.NewVariableScope())
                {
                    sourceWriter.Variables["propertyType"] = GetAdapterClrType(prop);
                    sourceWriter.Variables["propertyName"] = "_" + prop.Name;
                    sourceWriter.WriteLine("private {propertyType} {propertyName};");
                    if (prop.TypeUsage.EdmType is ComplexType)
                    {
                        sourceWriter.WriteLine("private bool {propertyName}_Initialized = false;");
                    }
                }
            }
        }

        private void WriteAssociationTypes(SourceWriter sourceWriter)
        {
            sourceWriter.WriteLine("// Association Types");
            sourceWriter.WriteLine();
            foreach (AssociationType associationType in Metadata.GetItems<AssociationType>())
            {
                using (sourceWriter.NewVariableScope())
                {
                    sourceWriter.Variables["associationName"] = associationType.Name;
                    sourceWriter.Variables["associationNamespace"] = associationType.NamespaceName;

                    sourceWriter.Variables["end1Name"] = associationType.AssociationEndMembers[0].Name;
                    sourceWriter.Variables["end1Multiplicity"] = associationType.AssociationEndMembers[0].RelationshipMultiplicity.ToString();
                    sourceWriter.Variables["end1Type"] = GetAdapterClrTypeName(((RefType)associationType.AssociationEndMembers[0].TypeUsage.EdmType).ElementType);

                    sourceWriter.Variables["end2Name"] = associationType.AssociationEndMembers[1].Name;
                    sourceWriter.Variables["end2Multiplicity"] = associationType.AssociationEndMembers[1].RelationshipMultiplicity.ToString();
                    sourceWriter.Variables["end2Type"] = GetAdapterClrTypeName(((RefType)associationType.AssociationEndMembers[1].TypeUsage.EdmType).ElementType);

                    sourceWriter.WriteLine("[assembly: global::System.Data.Objects.DataClasses.EdmRelationshipAttribute(\"{associationNamespace}\", \"{associationName}\", \"{end1Name}\", global::System.Data.Metadata.Edm.RelationshipMultiplicity.{end1Multiplicity}, typeof({end1Type}), \"{end2Name}\", global::System.Data.Metadata.Edm.RelationshipMultiplicity.{end2Multiplicity}, typeof({end2Type}))]");
                }
            }
        }
        public override void Generate(SourceWriter sourceWriter)
        {
            WriteGlobalAttributes(sourceWriter);
            bool first = true;

            foreach (var ns in Metadata.GetItems<StructuralType>().Select(c=>GetAdapterClrTypeNamespace(c)).Distinct())
            {
                sourceWriter.Variables["namespaceName"] = ns;
                sourceWriter.WriteLine("namespace {namespaceName}");
                using (sourceWriter.Indent())
                {
                    WriteCommonUsings(sourceWriter);
                    WriteEntityContainers(sourceWriter, ref first);
                    WriteComplexTypeAdapters(sourceWriter, ns);
                    WriteEntityTypeAdapters(sourceWriter, ns);
                }
            }

            foreach (var ns in Metadata.GetItems<StructuralType>().Select(c => GetProxyClrTypeNamespace(c)).Distinct())
            {
                sourceWriter.Variables["namespaceName"] = ns;
                sourceWriter.WriteLine("namespace {namespaceName}");
                using (sourceWriter.Indent())
                {
                    WriteCommonUsings(sourceWriter);
                    foreach (EntityType et in Metadata.GetItems<EntityType>().Where(c => GetProxyClrTypeNamespace(c) == ns && !c.Abstract))
                    {
                        WriteEntityTypeProxy(sourceWriter, et, false);
                    }
                }
            }
        }

        private void WriteEntityTypeAdapters(SourceWriter sourceWriter, string ns)
        {
            bool any = false;
            foreach (EntityType et in Metadata.GetItems<EntityType>().Where(c => GetAdapterClrTypeNamespace(c) == ns))
            {
                if (!any)
                {
                    sourceWriter.WriteLine();
                    sourceWriter.WriteLine("// POCO Adapters for Entity Types");
                }
                any = true;
                WriteEntityType(sourceWriter, et, false);
            }
        }

        private void WriteComplexTypeAdapters(SourceWriter sourceWriter, string ns)
        {
            bool any = false;
            foreach (ComplexType ct in Metadata.GetItems<ComplexType>().Where(c => GetAdapterClrTypeNamespace(c) == ns))
            {
                if (!any)
                {
                    sourceWriter.WriteLine();
                    sourceWriter.WriteLine("// POCO Adapters for Complex Types");
                }
                any = true;
                WriteComplexType(sourceWriter, ct, false);
            }
        }

        private void WriteEntityContainers(SourceWriter sourceWriter, ref bool first)
        {
            bool any = false;
            if (first)
            {
                first = false;
                foreach (EntityContainer container in Metadata.GetItems<EntityContainer>())
                {
                    if (!any)
                    {
                        sourceWriter.WriteLine();
                        sourceWriter.WriteLine("// Entity Containers");
                    }
                    any = true;
                    WriteEntityContainer(sourceWriter, container, false);
                }
            }
        }

        public override void Generate(string outputDirectory)
        {
            WriteGlobalAttributes(outputDirectory);
            WriteAdapterEntityContainers(outputDirectory);
            WriteEntityTypes(outputDirectory);
            WriteComplexTypes(outputDirectory);
            WriteEntityTypeProxies(outputDirectory);
        }

        private void WriteGlobalAttributes(string outputDirectory)
        {
            if (!Directory.Exists(outputDirectory))
                Directory.CreateDirectory(outputDirectory);
            using (StreamWriter output = new StreamWriter(Path.Combine(outputDirectory, "Global.cs")))
            {
                SourceWriter sourceWriter = new SourceWriter(output);
                WriteGlobalAttributes(sourceWriter);
            }
        }

        private void WriteGlobalAttributes(SourceWriter sourceWriter)
        {
            // detect schema namespace from the first entity type (containers don't have namespaces!)
            EntityType firstEntityType = Metadata.GetItems<EntityType>().FirstOrDefault();
            if (firstEntityType == null)
                throw new InvalidOperationException("Schema must have at least one EntityType");

            sourceWriter.Variables["schemaNamespace"] = firstEntityType.NamespaceName;

            sourceWriter.WriteLine("// Global Attributes");
            sourceWriter.WriteLine();
            sourceWriter.WriteLine(@"[assembly: global::System.Data.Objects.DataClasses.EdmSchemaAttribute()]");
            sourceWriter.WriteLine();
            WriteAssociationTypes(sourceWriter);
            sourceWriter.WriteLine();
        }

    }
}