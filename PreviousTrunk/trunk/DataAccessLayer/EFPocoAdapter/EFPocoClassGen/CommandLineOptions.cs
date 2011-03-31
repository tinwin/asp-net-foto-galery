// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;

namespace EFPocoClassGen
{
    class CommandLineOptions
    {
        public List<string> InputCsdlFiles = new List<string>();
        public List<string> ReferencedAssemblies = new List<string>();
        public Dictionary<string, string> EdmToClrNamespaceMapping = new Dictionary<string, string>();
        public string InputEdmxFile = null;
        public string OutputDirectory = null;
        public string OutputFile = null;
        public GenerationMode Mode = GenerationMode.PocoAdapter;
        public bool GenerateProxies = true;
        public bool ConditionalRebuild = true;
        public bool AutoLazyLoading = false;
        public bool Verbose = false;

        public void ValidateArguments()
        {
            if (InputEdmxFile == null && InputCsdlFiles.Count == 0)
                throw new ArgumentException("Must specify either /indmx or /incsdl");

            if (InputEdmxFile != null && InputCsdlFiles.Count > 0)
                throw new ArgumentException("Must specify either /indmx or /incsdl");

            int haveOutputFile = (OutputFile != null) ? 1 : 0;
            int haveOutputDirectory = (OutputDirectory != null) ? 1 : 0;

            if (haveOutputDirectory + haveOutputFile == 0)
            {
                throw new ArgumentException("At least one of /outputdir or /outputfile must be specified.");
            }
        }
    }
}
