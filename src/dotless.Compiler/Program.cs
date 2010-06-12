/* Copyright 2009 dotless project, http://www.dotlesscss.com
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 *     
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License. */

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

    public class Program
    {
        public static void Main(string[] args)
        {
            var arguments = new List<string>();

            arguments.AddRange(args);
            var watch = arguments.Any(p => p == "-w" || p == "--watch");

            DotlessConfiguration configuration;
            try
            {
                configuration = GetConfigurationFromArguments(arguments);                
            }
            catch (HelpRequestedException)
            {
                return;
            }

            if (arguments.Count == 0)
            {
                WriteHelp();
                return;
            }

            var inputFile = new FileInfo(arguments[0]);

            if (!inputFile.Exists && inputFile.Extension != ".less")
                inputFile = new FileInfo(inputFile.FullName + ".less");

            var outputFilePath = arguments.Count > 1 ? arguments[1] : Path.ChangeExtension(inputFile.Name, ".css");


            var currentDir = Directory.GetCurrentDirectory();
            if (inputFile.Directory != null)
                Directory.SetCurrentDirectory(inputFile.Directory.FullName);

            var engine = new EngineFactory(configuration).GetEngine();
            Func<IEnumerable<string>> compilationDelegate = () => Compile(engine, inputFile.Name, outputFilePath);

            var files = compilationDelegate();

            if (watch)
            {
                WriteAbortInstructions();

                new Watcher(files, compilationDelegate);
                
                while (Console.ReadLine() != "")
                {
                    WriteAbortInstructions();
                }
                Console.WriteLine("Stopped watching file. Exiting");
            }

            Directory.SetCurrentDirectory(currentDir);
        }

        private static IEnumerable<string> Compile(ILessEngine engine, string inputFilePath, string outputFilePath)
        {
            Console.Write("Compiling {0} -> {1} ", inputFilePath, outputFilePath);
            try
            {
                var source = new FileReader().GetFileContents(inputFilePath);
                string css = engine.TransformToCss(source, inputFilePath);
                File.WriteAllText(outputFilePath, css);
                Console.WriteLine("[Done]");
                return new[] { inputFilePath }.Concat(engine.GetImports());
            }
            catch (Exception ex)
            {
                if (ex is IOException) throw; //Rethrow

                Console.WriteLine("[FAILED]");
                Console.WriteLine("Compilation failed: {0}", ex.Message);
                Console.WriteLine(ex.StackTrace);
                return new string[]{};
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

        private static DotlessConfiguration GetConfigurationFromArguments(List<string> arguments)
        {
            var configuration = DotlessConfiguration.Default;
            configuration.CacheEnabled = false;

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
                    else if (arg == "-w" || arg == "--watch")
                    {
                        //Ignore this since it already gets handled. 
                        //TODO: Introduce a run-configuration class for Compiler only (sublcassing DotlessConfiguration?)
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