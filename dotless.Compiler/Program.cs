namespace dotless.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using nless.Core;
    using nless.Core.configuration;

    public class Program
    {
        public static void Main(string[] args)
        {
            var arguments = new List<string>();
            arguments.AddRange(args);

            DotlessConfiguration configuration;
            try
            {
                configuration = GetConfigurationFromArguments(arguments);
            }
            catch (HelpRequestedException)
            {
                return;
            }

            var inputFilePath = arguments[0];
            string outputFilePath;
            if (arguments.Count > 1)
            {
                outputFilePath = arguments[1];
            }
            else
            {
                outputFilePath = String.Format("{0}.css", inputFilePath);
            }
            if (File.Exists(inputFilePath))
            {
                var factory = new EngineFactory();
                ILessEngine engine = factory.GetEngine(configuration);
                Console.Write("Compiling {0} -> {1} ", inputFilePath, outputFilePath);
                string css = engine.TransformToCss(inputFilePath);
                File.WriteAllText(outputFilePath, css);
                Console.WriteLine("[Done]");
            }
            else
            {
                Console.WriteLine("Input file {0} does not exist", inputFilePath);
            }
        }

        private static string GetAssemblyVersion()
        {
            var attributes = Assembly.GetEntryAssembly().GetCustomAttributes(typeof (AssemblyVersionAttribute), false) as
                             AssemblyVersionAttribute[];
            if (attributes != null && attributes.Length == 1)
                return attributes[0].Version;
            return "v.Unknown";
        }

        private static void WriteHelp()
        {
            Console.WriteLine("dotless Compiler {0}", GetAssemblyVersion());
            Console.WriteLine("Compiles .less files to css files.");
            Console.WriteLine("Usage: dotless.Compiler.exe [-switches] <inputfile> [outputfile]");
            Console.WriteLine("\tSwitches:");
            Console.WriteLine("\t\t-m --minify - Output CSS will be compressed");
            Console.WriteLine("\t\t-h --help - Displays this dialog");
            Console.WriteLine("\tinputfile: .less file dotless should compile to CSS");
            Console.WriteLine("\toutputfile: (optional) desired filename for .css output");
            Console.WriteLine("\t\t Defaults to inputfile.css");
        }

        private static DotlessConfiguration GetConfigurationFromArguments(List<string> arguments)
        {
            var configuration = DotlessConfiguration.Default;
            foreach (var arg in arguments)
            {
                if (arg.StartsWith("-"))
                {
                    if (arg == "-m" || arg == "--minify")
                    {
                        configuration.MinifyOutput = true;
                    }
                    else if (arg == "-h" || arg == "--help")
                    {
                        WriteHelp();
                        throw new HelpRequestedException();
                    }
                    else
                    {
                        Console.WriteLine("Unknown command switch {0}.", arg);
                    }
                }
            }
            arguments.RemoveAll(p => p.StartsWith("-"));
            return configuration;
        }

        private class HelpRequestedException : ApplicationException
        {
        }
    }
}