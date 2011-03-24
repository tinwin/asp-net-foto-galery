// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Data.Metadata.Edm;
using System.IO;
using System.Linq;
using System.Reflection;
using EFPocoAdapter.CodeGeneration;
using EFPocoAdapter.DataClasses;

namespace EFPocoAdapter
{
    public class EFPocoAdapterGenerator<T>
        where T : EFPocoContext, new()
    {
        public string EntityConnectionString { get; set; }
        public IDictionary<string, string> EdmToClrNamespaceMapping { get; private set; }
        public ICollection<Assembly> ObjectAssemblies { get; private set; }
        public bool GenerateProxies { get; set; }
        public bool AutoLazyLoading { get; set; }
        public string SaveGeneratedCodeIn { get; set; }
        public TextWriter VerboseOutput { get; set; }

        public EFPocoAdapterGenerator()
        {
            EdmToClrNamespaceMapping = new Dictionary<string, string>();
            ObjectAssemblies = new List<Assembly>();
        }

        public EFPocoContextFactory<T> CreateContextFactory()
        {
            return new EFPocoContextFactory<T>(CreateAdapterContainerType());
        }

        private Type CreateAdapterContainerType()
        {
            EntityConnection conn = new EntityConnection(EntityConnectionString);
            var mw = conn.GetMetadataWorkspace();

            PocoAdapterCodeGenerator pag = new PocoAdapterCodeGenerator();
            pag.Metadata = (EdmItemCollection)mw.GetItemCollection(System.Data.Metadata.Edm.DataSpace.CSpace);
            foreach (var mapping in this.EdmToClrNamespaceMapping)
            {
                pag.EdmToClrNamespaceMapping[mapping.Key] = mapping.Value;
            }
            pag.GenerateProxies = this.GenerateProxies;
            foreach (var objectAssembly in this.ObjectAssemblies)
                pag.Assemblies.Add(objectAssembly);
            pag.VerboseOutput = this.VerboseOutput;
            if (SaveGeneratedCodeIn != null)
            {
                using (StreamWriter sw = new StreamWriter(SaveGeneratedCodeIn))
                {
                    pag.Generate(new SourceWriter(sw));
                }
            }
            Assembly assm = pag.GenerateAssembly(null, true);
            Type contextType = assm.GetExportedTypes().Where(c => typeof(IPocoAdapterObjectContext).IsAssignableFrom(c)).Single();
            return contextType;
        }
    }
}
