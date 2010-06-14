namespace dotless.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using Core;
    using Core.configuration;
    using System.Linq;
    using Core.Input;
    using Core.Parameters;

    public class Program
    {
        public static void Main(string[] args)
        {
            var arguments = new List<string>();

            arguments.AddRange(args);

            var configuration = GetConfigurationFromArguments(arguments);

            if(configuration.Help)
                return;

            if (arguments.Count == 0)
            {
                WriteHelp();
                return;
            }

            var inputFile = new FileInfo(arguments[0]);

            if (!inputFile.Exists && inputFile.Extension != ".less")
                inputFile = new FileInfo(inputFile.FullName + ".less");

            string outputFilePath;
            if (arguments.Count > 1)
            {
                outputFilePath = arguments[1] + (Path.HasExtension(arguments[1]) ? "" : ".css");
                outputFilePath = Path.GetFullPath(outputFilePath);
            }
            else
                outputFilePath = Path.ChangeExtension(inputFile.Name, ".css");

            var currentDir = Directory.GetCurrentDirectory();
            if (inputFile.Directory != null)
                Directory.SetCurrentDirectory(inputFile.Directory.FullName);

            var engine = new EngineFactory(configuration).GetEngine();
            Func<IEnumerable<string>> compilationDelegate = () => Compile(engine, inputFile.Name, outputFilePath);

            var files = compilationDelegate();

            if (configuration.Watch)
            {
                WriteAbortInstructions();

                var watcher = new Watcher(files, compilationDelegate);

                while (Console.ReadLine() != "")
                {
                    WriteAbortInstructions();
                }

                watcher.RemoveWatchers();
            }

            Directory.SetCurrentDirectory(currentDir);
        }

        private static IEnumerable<string> Compile(ILessEngine engine, string inputFilePath, string outputFilePath)
        {
            Console.Write("Compiling {0} -> {1} ", inputFilePath, outputFilePath);
            try
            {
                var source = new FileReader().GetFileContents(inputFilePath);
                var css = engine.TransformToCss(source, inputFilePath);

                File.WriteAllText(outputFilePath, css);
                Console.WriteLine("[Done]");
                
                return new[] { inputFilePath }.Concat(engine.GetImports());
            }
            catch(IOException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("[FAILED]");
                Console.WriteLine("Compilation failed: {0}", ex.Message);
                Console.WriteLine(ex.StackTrace);
                return null;
            }
        }

        private static void WriteAbortInstructions()
        {
            Console.WriteLine("Hit Enter to stop watching");
        }

        private static string GetAssemblyVersion()
        {
            Assembly assembly = typeof(EngineFactory).Assembly;
            var attributes = assembly.GetCustomAttributes(typeof (AssemblyFileVersionAttribute), true) as
                             AssemblyFileVersionAttribute[];
            if (attributes != null && attributes.Length == 1)
                return attributes[0].Version;
            return "v.Unknown";
        }

        private static void WriteHelp()
        {
            Console.WriteLine("dotless Compiler {0}", GetAssemblyVersion());
            Console.WriteLine("\tCompiles .less files to css files.");
            Console.WriteLine();
            Console.WriteLine("Usage: dotless.Compiler.exe [-switches] <inputfile> [outputfile]");
            Console.WriteLine("\tSwitches:");
            Console.WriteLine("\t\t-m --minify - Output CSS will be compressed");
            Console.WriteLine("\t\t-w --watch - Watches .less file for changes");
            Console.WriteLine("\t\t-h --help - Displays this dialog");
            Console.WriteLine("\tinputfile: .less file dotless should compile to CSS");
            Console.WriteLine("\toutputfile: (optional) desired filename for .css output");
            Console.WriteLine("\t\t Defaults to inputfile.css");
        }

        private static CompilerConfiguration GetConfigurationFromArguments(List<string> arguments)
        {
            var configuration = new CompilerConfiguration(DotlessConfiguration.Default);

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
                        configuration.Help = true;
                        return configuration;
                    }
                    else if (arg == "-w" || arg == "--watch")
                    {
                        configuration.Watch = true;
                    }
                    else if (arg.StartsWith("-D") && arg.Contains("="))
                    {
                        var split = arg.Substring(2).Split('=');
                        var key = split[0];
                        var value = split[1];
                        ConsoleArgumentParameterSource.ConsoleArguments.Add(key, value);
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
    }
}