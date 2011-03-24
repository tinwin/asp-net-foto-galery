// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Data.Metadata.Edm;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CSharp;
using System.Globalization;

namespace EFPocoAdapter.CodeGeneration
{
    public abstract class CodeGeneratorBase
    {
        const string XmlNamespaceName = "http://code.msdn.microsoft.com/EFPocoAdapter/ObjectMapping.xsd";

        public string ConnectionString { get; set; }
        public EdmItemCollection Metadata { get; set; }
        public bool AutoLazyLoading { get; set; }
        public bool GenerateProxies { get; set; }
        public TextWriter VerboseOutput { get; set; }
        public Dictionary<string, string> EdmToClrNamespaceMapping { get; private set; }
        public ICollection<Assembly> Assemblies { get; private set; }

        private HashSet<object> _notifications = new HashSet<object>();
        
        protected CodeGeneratorBase()
        {
            Assemblies = new List<Assembly>();
            EdmToClrNamespaceMapping = new Dictionary<string, string>();
        }

        protected static string GetFileNameFor(string outputDirectory, string className)
        {
            string ns = GetNamespaceName(className);
            string baseName = GetBaseTypeName(className);
            string outDir = Path.Combine(outputDirectory, ns);
            if (!Directory.Exists(outDir))
                Directory.CreateDirectory(outDir);
            return Path.Combine(outDir, baseName + ".cs");
        }

        public string GetPocoClrTypeName(EdmType edmType)
        {
            MetadataProperty mp;

            if (edmType.MetadataProperties.TryGetValue(XmlNamespaceName + ":ClrType", false, out mp))
            {
                return (string)mp.Value;
            }
            return MapNamespace(edmType.NamespaceName) + "." + edmType.Name;
        }

        public string GetProxyClrTypeName(EdmType edmType)
        {
            return Inject(GetPocoClrTypeName(edmType), "PocoProxies", "Proxy");
        }

        public string GetAdapterClrTypeName(EdmType edmType)
        {
            return Inject(GetPocoClrTypeName(edmType), "PocoAdapters", "Adapter");
        }

        protected string GetPocoClrTypeNamespace(EdmType edmType)
        {
            return GetNamespaceName(GetPocoClrTypeName(edmType));
        }

        protected string GetProxyClrTypeNamespace(EdmType edmType)
        {
            return GetNamespaceName(GetProxyClrTypeName(edmType));
        }

        protected string GetAdapterClrTypeNamespace(EdmType edmType)
        {
            return GetNamespaceName(GetAdapterClrTypeName(edmType));
        }

        protected static string Inject(string typeName, string namespaceSuffix, string typeSuffix)
        {
            int p = typeName.LastIndexOf('.');
            if (p >= 0)
                return typeName.Substring(0, p) + "." + namespaceSuffix + "." + typeName.Substring(p + 1) + typeSuffix;
            else
                return namespaceSuffix + "." + typeName + typeSuffix;
        }

        protected string MapNamespace(string ns)
        {
            string ns2;
            if (EdmToClrNamespaceMapping.TryGetValue(ns, out ns2))
                return ns2;
            else
                return ns;
        }

        protected static string GetNamespaceName(string typeName)
        {
            int p = typeName.LastIndexOf('.');
            if (p >= 0)
                return typeName.Substring(0, p);
            else
                return "";
        }

        protected static string GetBaseTypeName(string typeName)
        {
            int p = typeName.LastIndexOf('.');
            if (p >= 0)
                return typeName.Substring(p + 1);
            else
                return typeName;
        }

        protected string GetPocoClrType(EdmProperty prop)
        {
            PrimitiveType pt = prop.TypeUsage.EdmType as PrimitiveType;
            if (pt != null)
            {
                string nullableSuffix = (prop.Nullable && pt.ClrEquivalentType.IsValueType) ? "?" : "";

                return pt.ClrEquivalentType.Name + nullableSuffix;
            }

            Debug.Assert(prop.TypeUsage.EdmType is StructuralType);
            return GetPocoClrTypeName((StructuralType)prop.TypeUsage.EdmType);
        }

        protected string GetPocoClrType(NavigationProperty navprop)
        {
            if (navprop.ToEndMember.RelationshipMultiplicity == RelationshipMultiplicity.Many)
            {
                return GetCollectionTypeName(navprop);
            }
            else
            {
                return GetPocoClrTypeName(((RefType)navprop.ToEndMember.TypeUsage.EdmType).ElementType);                
            }
        }

        protected string GetAdapterClrType(EdmProperty prop)
        {
            PrimitiveType pt = prop.TypeUsage.EdmType as PrimitiveType;
            if (pt != null)
            {
                string nullableSuffix = (prop.Nullable && pt.ClrEquivalentType.IsValueType) ? "?" : "";

                return pt.ClrEquivalentType.Name + nullableSuffix;
            }

            Debug.Assert(prop.TypeUsage.EdmType is StructuralType);
            return GetAdapterClrTypeName((StructuralType)prop.TypeUsage.EdmType);
        }

        protected bool IsHidden(EdmMember prop)
        {
            return GetChangeTrackingMode(prop) == ChangeTrackingMode.Hidden;
        }

        protected EdmMember[] GetConstructorArguments(StructuralType structuralType)
        {
            MetadataProperty mp;

            if (structuralType.MetadataProperties.TryGetValue(XmlNamespaceName + ":ConstructorArguments", false, out mp))
            {
                // string names = (string)mp.Value;
            }

            Type clrType;

            if (!TryGetClrType(structuralType, out clrType))
            {
                // no such type - we don't have assemblies loaded
                // assume we have a default constructor
                return new EdmMember[0];
            }

            ConstructorInfo[] constructors = clrType
                .GetConstructors(BindingFlags.Public | BindingFlags.Instance);
            ConstructorInfo constructor = null;
            if (clrType.IsValueType)
            {
                constructor = constructors.Where(c => c.GetParameters().Length > 0).SingleOrDefault();
            }
            else
            {
                constructor = constructors.Where(c => c.GetParameters().Length == 0).SingleOrDefault();
                if (constructor == null)
                    constructor = constructors.SingleOrDefault();
            }
            List<EdmMember> results = new List<EdmMember>();
            if (constructor != null)
            {
                foreach (ParameterInfo pi in constructor.GetParameters())
                {
                    string paramName = pi.Name;
                    var edmMember = structuralType.Members.Where(c => 0 == String.Compare(c.Name, paramName, StringComparison.InvariantCultureIgnoreCase)).SingleOrDefault();
                    results.Add(edmMember);
                }
            }
            return results.ToArray();
        }

        protected bool HasDefaultConstructor(StructuralType structuralType)
        {
            if (GetConstructorArguments(structuralType).Length != 0)
                return false;

            Type clrType;

            if (!TryGetClrType(structuralType, out clrType))
                return true;

            return clrType.GetConstructor(Type.EmptyTypes) != null;
        }

        protected bool IsSettable(EdmMember prop)
        {
            ChangeTrackingMode ctm = GetChangeTrackingMode(prop);
            
            return ctm != ChangeTrackingMode.Hidden && ctm != ChangeTrackingMode.ConstructOnly;
        }

        protected ChangeTrackingMode GetChangeTrackingMode(EdmMember prop)
        {
            MetadataProperty mp;

            EntityType et = prop.DeclaringType as EntityType;

            if (prop.MetadataProperties.TryGetValue(XmlNamespaceName + ":ChangeTracking", false, out mp))
                return (ChangeTrackingMode)Enum.Parse(typeof(ChangeTrackingMode), (string)mp.Value, true);

            if (Assemblies.Count > 0)
            {
                MemberInfo clrMember;

                if (!TryGetClrMember(prop, out clrMember))
                {
                    if (VerboseOutput != null && !_notifications.Contains(clrMember))
                    {
                        _notifications.Add(clrMember);
                        VerboseOutput.WriteLine("Property is hidden: '{0}.{1}'", prop.DeclaringType.FullName, prop.Name);
                    }
                    return ChangeTrackingMode.Hidden;
                }

                if (clrMember.MemberType == MemberTypes.Property)
                {
                    PropertyInfo pi = (PropertyInfo)clrMember;

                    if (!pi.CanWrite || pi.GetSetMethod() == null || !pi.GetSetMethod().IsPublic)
                    {
                        return ChangeTrackingMode.ConstructOnly;
                    }

                    if (GenerateProxies)
                    {
                        bool useProxy = false;
                        if (pi.PropertyType.IsInterface && pi.PropertyType.IsGenericType)
                        {
                            Type definition = pi.PropertyType.GetGenericTypeDefinition();
                            if (definition == typeof(ICollection<>) || definition == typeof(IList<>))
                            {
                                useProxy = true;
                            }
                        }
                        if (pi.GetGetMethod().IsVirtual && pi.GetSetMethod().IsVirtual && pi.GetSetMethod().IsPublic)
                        {
                            useProxy = true;
                        }

                        if (useProxy)
                        {
                            if (VerboseOutput != null && !_notifications.Contains(clrMember))
                            {
                                _notifications.Add(clrMember);
                                VerboseOutput.WriteLine("Proxy-based change tracking: '{0}.{1}'", clrMember.DeclaringType, clrMember.Name);
                            }
                            return ChangeTrackingMode.Proxy;
                        }
                    }
                }
            }

            if (et != null && et.KeyMembers.Contains(prop))
                return ChangeTrackingMode.Snapshot;

            for (EdmType type = prop.DeclaringType; type != null; type = type.BaseType)
            {
                if (type.MetadataProperties.TryGetValue(XmlNamespaceName + ":ChangeTracking", false, out mp))
                    return (ChangeTrackingMode)Enum.Parse(typeof(ChangeTrackingMode), (string)mp.Value, true);
            }

            return ChangeTrackingMode.Snapshot;
        }

        private bool TryGetClrMember(EdmMember edmMember, out MemberInfo clrMember)
        {
            Type clrType;

            if (!TryGetClrType(edmMember.DeclaringType, out clrType))
            {
                clrMember = null;
                return false;
            }
            MemberInfo[] clrMembers = clrType.GetMember(edmMember.Name);
            if (clrMembers.Length == 1)
            {
                clrMember = clrMembers[0];
                return true;
            }
            else
            {
                clrMember = null;
                return false;
            }
        }

        private bool TryGetClrType(StructuralType structuralType, out Type type)
        {
            MetadataProperty mp;

            if (structuralType.MetadataProperties.TryGetValue(XmlNamespaceName + ":ClrType", false, out mp))
            {
                return TryGetClrTypeByName((string)mp.Value, out type);
            }
            string ns;
            if (!EdmToClrNamespaceMapping.TryGetValue(structuralType.NamespaceName, out ns))
                ns = structuralType.NamespaceName;

            return TryGetClrTypeByName(ns + "." + structuralType.Name, out type);
        }

        private bool TryGetClrTypeByName(string typeName, out Type type)
        {
            foreach (Assembly a in Assemblies)
            {
                Type t = a.GetType(typeName, false, false);
                if (t != null)
                {
                    type = t;
                    return true;
                }
            }
            type = null;
            return false;
        }

        public abstract void Generate(SourceWriter sourceWriter);
        public abstract void Generate(string outputDirectory);

        public virtual Assembly GenerateAssembly(string outputAssemblyName, bool inMemory)
        {
            StringWriter sw = new StringWriter(CultureInfo.InvariantCulture);
            SourceWriter sourceWriter = new SourceWriter(sw);
            Generate(sourceWriter);
            string allSourceCode = sw.ToString();

            var providerOptions = new Dictionary<string, string>();
            providerOptions.Add("CompilerVersion", "v3.5");
            CSharpCodeProvider codeProvider = new CSharpCodeProvider(providerOptions);
            CompilerParameters parameters = new CompilerParameters();
            if (outputAssemblyName != null)
                parameters.OutputAssembly = outputAssemblyName;
            parameters.GenerateInMemory = inMemory;

            parameters.IncludeDebugInformation = true;
            parameters.ReferencedAssemblies.Add("System.dll");
            parameters.ReferencedAssemblies.Add("System.Data.dll");
            parameters.ReferencedAssemblies.Add("System.Core.dll");
            foreach (Assembly a in Assemblies)
            {
                parameters.ReferencedAssemblies.Add(a.Location);
            }
            parameters.ReferencedAssemblies.Add(typeof(EFPocoContext).Assembly.Location);
            parameters.ReferencedAssemblies.Add(typeof(EntityConnection).Assembly.Location);
            CompilerResults results = codeProvider.CompileAssemblyFromSource(parameters, allSourceCode);
            if (results.Errors.HasErrors)
            {
                throw new InvalidOperationException("Generated code failed to compile\r\nErrors (" + results.Errors.Count + "):\r\n" +
                    String.Join("\r\n", results.Errors.Cast<CompilerError>()
                    .Take(10)
                    .Select(c => "(" + c.Line + "," + c.Column + "): " + c.ErrorNumber + " " + c.ErrorText).ToArray()));
            }
            return results.CompiledAssembly;
        }

        protected Type GetCollectionType(NavigationProperty prop)
        {
            Type result;

            if (!TryGetCollectionType(prop, out result))
                throw new InvalidOperationException("Unknown member: " + prop.Name + " in " + prop.DeclaringType.FullName);

            return result;
        }

        protected bool TryGetCollectionType(NavigationProperty prop, out Type result)
        {
            MemberInfo clrMember;

            if (!TryGetClrMember(prop, out clrMember))
            {
                result = null;
                return false;
            }

            PropertyInfo pi = clrMember as PropertyInfo;
            if (pi != null)
            {
                result = pi.PropertyType;
                return true;
            }

            FieldInfo fi = clrMember as FieldInfo;
            if (fi != null)
            {
                result = fi.FieldType;
                return true;
            }

            result = null;
            return false;
        }

        protected string GetCollectionTypeName(NavigationProperty navprop)
        {
            MetadataProperty mp;

            if (navprop.MetadataProperties.TryGetValue(XmlNamespaceName + ":CollectionType", false, out mp))
            {
                return (string)mp.Value;
            }

            Type t;

            if (TryGetCollectionType(navprop, out t))
            {
                if (t.IsInterface && t.IsGenericType)
                    t = typeof(List<>).MakeGenericType(t.GetGenericArguments());
                return GetCSharpTypeName(t);
            }
            else
            {
                return "List<" + GetPocoClrTypeName(((RefType)navprop.ToEndMember.TypeUsage.EdmType).ElementType) + ">";
            }
        }

        private string GetCSharpTypeName(Type t)
        {
            if (t.IsGenericType)
            {
                Type def = t.GetGenericTypeDefinition();
                int p = def.Name.IndexOf('`');
                string baseName = def.Name.Substring(0, p);
                return baseName + "<" + String.Join(",", t.GetGenericArguments().Select(c => GetCSharpTypeName(c)).ToArray()) + ">";
            }
            else
            {
                return t.FullName;
            }
        }
    }
}
