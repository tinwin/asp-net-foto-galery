// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Data.Metadata.Edm;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using EFPocoAdapter.CodeGeneration;

namespace EFPocoClassGen
{
    enum GenerationMode
    {
        Poco,
        PocoAdapter,
        PocoContainer,
    }

    class Program
    {
        static int Main(string[] args)
        {
            CommandLineOptions options;
            try
            {
                options = ParseCommandLineOptions(args);
                if (options == null)
                {
                    Usage();
                    return 1;
                }
                // validate arguments
                options.ValidateArguments();
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("Invalid argument: {0}", ex.Message);
                Usage();
                return 1;
            }

            try
            {
                CodeGeneratorBase generator;

                switch (options.Mode)
                {
                    case GenerationMode.Poco:
                        generator = new PocoClassCodeGenerator();
                        break;

                    case GenerationMode.PocoAdapter:
                        generator = new PocoAdapterCodeGenerator();
                        break;

                    case GenerationMode.PocoContainer:
                        generator = new PocoContainerCodeGenerator();
                        break;

                    default:
                        throw new NotSupportedException("Not supported mode: " + options.Mode);
                }

                EdmItemCollection itemCollection;

                if (options.InputEdmxFile != null)
                {
                    if (options.Verbose)
                        Console.WriteLine("Loading conceptual model from '{0}'...", options.InputEdmxFile);
                    XElement edmx = XElement.Load(options.InputEdmxFile);
                    XNamespace edmxNamespace = "http://schemas.microsoft.com/ado/2007/06/edmx";
                    var csdlNodes = edmx.Descendants(edmxNamespace + "ConceptualModels").First().Elements();
                    var readers = csdlNodes.Select(c=>c.CreateReader());
                    itemCollection = new EdmItemCollection(readers);
                }
                else
                {
                    if (options.Verbose)
                        Console.WriteLine("Loading conceptual model from '{0}'...", String.Join(", ", options.InputCsdlFiles.ToArray()));
                    itemCollection = new EdmItemCollection(options.InputCsdlFiles.ToArray());
                }

                foreach (string asm in options.ReferencedAssemblies)
                {
                    if (asm.EndsWith(".dll"))
                    {
                        if (options.Verbose)
                            Console.WriteLine("Loading assembly from file: '{0}'", asm);
                        generator.Assemblies.Add(Assembly.LoadFrom(asm));
                    }
                    else
                    {
                        if (options.Verbose)
                            Console.WriteLine("Loading assembly: '{0}'", asm);
                        generator.Assemblies.Add(Assembly.Load(asm));
                    }
                }
                foreach (var m in options.EdmToClrNamespaceMapping)
                {
                    if (options.Verbose)
                        Console.WriteLine("Adding namespace mapping from {0} to {1}", m.Key, m.Value);
                    generator.EdmToClrNamespaceMapping[m.Key] = m.Value;
                }
                if (options.Verbose)
                    generator.VerboseOutput = Console.Out;
                generator.Metadata = itemCollection;
                generator.GenerateProxies = options.GenerateProxies;
                generator.AutoLazyLoading = options.AutoLazyLoading;

                if (options.OutputFile != null)
                {
                    bool needToRebuild = false;

                    if (options.ConditionalRebuild == false)
                        needToRebuild = true;
                    if (!File.Exists(options.OutputFile) || GetInputTimeStamp(options) > File.GetLastWriteTime(options.OutputFile))
                        needToRebuild = true;

                    DateTime inputTimeStamp = GetInputTimeStamp(options);

                    if (needToRebuild)
                    {
                        if (options.Verbose)
                            Console.WriteLine("Writing '{0}'", options.OutputFile);
                        using (StreamWriter sw = File.CreateText(options.OutputFile))
                        {
                            generator.Generate(new SourceWriter(sw));
                        }
                    }
                    else
                    {
                        if (options.Verbose)
                            Console.WriteLine("File '{0}' is up to date.", options.OutputFile);
                    }
                }

                if (options.OutputDirectory != null)
                {
                    Console.WriteLine("Writing to output directory: '{0}'", options.OutputDirectory);
                    generator.Generate(options.OutputDirectory);
                }

                return 0;
            }
            catch (ArgumentException ex)
            {
                ConsoleColor oldColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid argument: {0}", ex.Message);
                Usage();
                Console.ForegroundColor = oldColor;
                return 1;
            }
            catch (Exception ex)
            {
                ConsoleColor oldColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR: {0}", ex);
                Console.ForegroundColor = oldColor;
                return 1;
            }
        }

        private static DateTime Max(DateTime dt1, DateTime dt2)
        {
            return (dt2 > dt1) ? dt2 : dt1;
        }

        private static DateTime GetInputTimeStamp(CommandLineOptions options)
        {
            DateTime timeStamp = File.GetLastWriteTime(typeof(PocoAdapterCodeGenerator).Assembly.Location);
            foreach (string csdl in options.InputCsdlFiles)
            {
                timeStamp = Max(File.GetLastWriteTime(csdl), timeStamp);
            }
            if (options.InputEdmxFile != null)
                timeStamp = Max(File.GetLastWriteTime(options.InputEdmxFile), timeStamp);

            return timeStamp;
        }

        private static CommandLineOptions ParseCommandLineOptions(string[] args)
        {
            CommandLineOptions clo = new CommandLineOptions();

            for (int i = 0; i < args.Length; ++i)
            {
                int p = args[i].IndexOf(':');
                string command;
                string argument;

                if (p >= 0)
                {
                    command = args[i].Substring(0, p);
                    argument = args[i].Substring(p + 1);
                }
                else
                {
                    command = args[i];
                    argument = null;
                }

                switch (command.ToLowerInvariant())
                {
                    case "/?":
                    case "/help":
                        Usage();
                        return null;

                    case "/incsdl":
                        clo.InputCsdlFiles.Add(argument);
                        break;

                    case "/inedmx":
                        if (clo.InputEdmxFile != null)
                            throw new ArgumentException("Multiple /inedmx arguments are not supported.");
                        clo.InputEdmxFile = argument;
                        break;

                    case "/ref":
                        clo.ReferencedAssemblies.Add(argument);
                        break;

                    case "/map":
                        string[] parts = argument.Split('=');
                        if (parts.Length != 2)
                            throw new ArgumentException("Invalid argument to /map. Must be /map:EdmNamespace=ClrNamespace");
                        clo.EdmToClrNamespaceMapping[parts[0]] = parts[1];
                        break;

                    case "/outputdir":
                        if (clo.OutputDirectory != null)
                            throw new ArgumentException("Duplicate /outputdir option");
                        clo.OutputDirectory = Path.GetFullPath(argument);
                        break;

                    case "/outputfile":
                        if (clo.OutputFile != null)
                            throw new ArgumentException("Duplicate /outputfile option");
                        clo.OutputFile = Path.GetFullPath(argument);
                        break;

                    case "/verbose":
                        clo.Verbose = Convert.ToBoolean(argument ?? "true", CultureInfo.InvariantCulture);
                        break;

                    case "/mode":
                        switch (argument.ToLowerInvariant())
                        {
                            case "pococlasses":
                                clo.Mode = GenerationMode.Poco;
                                break;

                            case "pocoadapter":
                                clo.Mode = GenerationMode.PocoAdapter;
                                break;

                            case "pococontainer":
                                clo.Mode = GenerationMode.PocoContainer;
                                break;
                        }
                        break;

                    case "/conditionalrebuild":
                        clo.ConditionalRebuild = Convert.ToBoolean(argument ?? "true", CultureInfo.InvariantCulture);
                        break;

                    case "/autolazyloading":
                        clo.AutoLazyLoading = Convert.ToBoolean(argument ?? "true", CultureInfo.InvariantCulture);
                        break;

                    case "/generateproxies":
                        clo.GenerateProxies = Convert.ToBoolean(argument ?? "true", CultureInfo.InvariantCulture);
                        break;

                    default:
                        throw new ArgumentException("Invalid argument: " + args[i]);
                }
            }
            return clo;
        }

        static void Usage()
        {
            Console.WriteLine("Usage: {0} options...", Process.GetCurrentProcess().ProcessName);
            Console.WriteLine(@"
Options:

MODE SELECTION

  /mode:PocoClasses                Generates POCO classes
  /mode:PocoContainer              Generates POCO container
  /mode:PocoAdapter                Generates POCO adapters

INPUT OPTIONS

  /incsdl:filename.csdl            Specifies CSDL file
  /inedmx:filename.edmx            Specifies EDMX file 
  /ref:assemblyName                Specifies referenced assembly

OUTPUT OPTIONS

  /outputDir:directory             Specifies output directory
  /outputFile:filename.cs          Specifies output file

OTHER OPTIONS:

  /map:EdmNamespace=ClrNamespace   Defines namespace mapping from EDM to CLR
  /conditionalrebuild:[true|false] Enables/disables conditional rebuild
                                   (will generate output file only if 
                                   it does not exist or is older than input)
  /generateProxies:[true|false]    Enables proxy change tracking for all 
                                   virtual properties in ref assemblies
                                   Requires at least one /ref: to be specified.
  /autoLazyLoading:[true|false]    Generate lazy-loading proxy code for 
                                   all virtual and ICollection<T> properties 
                                   in reference assemblies.
                                   Requires at least one /ref: to be specified.

");
        }

    }
}
