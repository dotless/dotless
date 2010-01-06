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

    public class Program
    {
        public static void Main(string[] args)
        {
            var arguments = new List<string>();
            arguments.AddRange(args);

            if (arguments.Count == 0)
            {
                WriteHelp();
                return;
            }

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
                string css = engine.TransformToCss(new FileSource().GetSource(inputFilePath));
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