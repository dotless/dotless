namespace dotless.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Linq;
    using Core;
    using Core.configuration;
    using Core.Parameters;
    using dotless.Core.Plugins;

    public class Program
    {
        static int returnCode = 0;
        public static int Main(string[] args)
        {
            var arguments = new List<string>();

            arguments.AddRange(args);

            var configuration = GetConfigurationFromArguments(arguments);

            if(configuration.Help)
                return -1;

            if (arguments.Count == 0)
            {
                WriteHelp();
                return -1;
            }

            var inputDirectoryPath = Path.GetDirectoryName(arguments[0]);
            if(string.IsNullOrEmpty(inputDirectoryPath)) inputDirectoryPath = "." + Path.DirectorySeparatorChar;
            var inputFilePattern = Path.GetFileName(arguments[0]);
            var outputDirectoryPath = string.Empty;
            var outputFilename = string.Empty;

            if (string.IsNullOrEmpty(inputFilePattern)) inputFilePattern = "*.less";
            if (!Path.HasExtension(inputFilePattern)) inputFilePattern = Path.ChangeExtension(inputFilePattern, "less");

            if (arguments.Count > 1)
            {
                outputDirectoryPath = Path.GetDirectoryName(arguments[1]);
                outputFilename = Path.GetFileName(arguments[1]);
                outputFilename = Path.ChangeExtension(outputFilename, "css");
            }
            
            if (string.IsNullOrEmpty(outputDirectoryPath))
            {
                outputDirectoryPath = inputDirectoryPath;
            }
            else
            {
                Directory.CreateDirectory(outputDirectoryPath);
            }

            if (HasWildcards(inputFilePattern)) 
                outputFilename = string.Empty;

            var filenames = Directory.GetFiles(inputDirectoryPath, inputFilePattern);
            var engine = new EngineFactory(configuration).GetEngine();

            using (var watcher = new Watcher() { Watch = configuration.Watch })
            {
                if (watcher.Watch && HasWildcards(inputFilePattern))
                {
                    CompilationFactoryDelegate factoryDelegate = (input) => CreationImpl(engine, input, Path.GetFullPath(outputDirectoryPath));
                    watcher.SetupDirectoryWatcher(Path.GetFullPath(inputDirectoryPath), inputFilePattern, factoryDelegate);
                }

                foreach (var filename in filenames)
                {
                    var inputFile = new FileInfo(filename);

                    var outputFile = 
                        string.IsNullOrEmpty(outputFilename) ? 
                            Path.Combine(outputDirectoryPath, Path.ChangeExtension(inputFile.Name, "css")) :
                            Path.Combine(outputDirectoryPath, outputFilename);

                    var outputFilePath = Path.GetFullPath(outputFile);

                    CompilationDelegate compilationDelegate = () => CompileImpl(engine, inputFile.FullName, outputFilePath);

                    Console.WriteLine("[Compile]");

                    var files = compilationDelegate();

                    if (watcher.Watch)
                        watcher.SetupWatchers(files, compilationDelegate);
                }

                if (configuration.Watch) 
                    WriteAbortInstructions();

                while (watcher.Watch && Console.ReadKey(true).Key != ConsoleKey.Enter) 
                {
                    System.Threading.Thread.Sleep(200);
                }
            }
            return returnCode;
        }
        private static CompilationDelegate CreationImpl(ILessEngine engine, string inputFilePath, string outputDirectoryPath)
        {
            var pathbuilder = new System.Text.StringBuilder(outputDirectoryPath + Path.DirectorySeparatorChar);
            pathbuilder.Append(Path.ChangeExtension(Path.GetFileName(inputFilePath), "css"));
            var outputFilePath = Path.GetFullPath(pathbuilder.ToString());
            return () => CompileImpl(engine, inputFilePath, outputFilePath);
        }

        private static IEnumerable<string> CompileImpl(ILessEngine engine, string inputFilePath, string outputFilePath)
        {
            engine = new FixImportPathDecorator(engine);
            var currentDir = Directory.GetCurrentDirectory();
            try
            {
                Console.WriteLine("{0} -> {1}", inputFilePath, outputFilePath);
                var directoryPath = Path.GetDirectoryName(inputFilePath);
                var source = new dotless.Core.Input.FileReader().GetFileContents(inputFilePath);
                Directory.SetCurrentDirectory(directoryPath);
                var css = engine.TransformToCss(source, inputFilePath);
                if (string.IsNullOrEmpty(css) && !string.IsNullOrEmpty(source))
                {
                    returnCode++;
                }
                File.WriteAllText(outputFilePath, css);
                Console.WriteLine("[Done]");

                var files = new List<string>();
                files.Add(inputFilePath);
                foreach (var file in engine.GetImports())
                    files.Add(Path.Combine(directoryPath, Path.ChangeExtension(file, "less")));
                engine.ResetImports();
                return files;
            }
            catch (IOException)
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
            finally
            {
                Directory.SetCurrentDirectory(currentDir);
            }
        }


        private static bool HasWildcards(string inputFilePattern)
        {
            return System.Text.RegularExpressions.Regex.Match(inputFilePattern, @"[\*\?]").Success;
        }

        private static void WriteAbortInstructions()
        {
            Console.WriteLine("Hit Enter to stop watching");
        }

        private static string GetAssemblyVersion()
        {
            Assembly assembly = typeof(EngineFactory).Assembly;
            var attributes = assembly.GetCustomAttributes(typeof(AssemblyFileVersionAttribute), true) as
                             AssemblyFileVersionAttribute[];
            if (attributes != null && attributes.Length == 1)
                return attributes[0].Version;
            return "v.Unknown";
        }

        private static IEnumerable<IPluginConfigurator> _pluginConfigurators = null;
        private static IEnumerable<IPluginConfigurator> GetPluginConfigurators()
        {
            if (_pluginConfigurators == null)
            {
                _pluginConfigurators = PluginFinder.GetConfigurators(true);
            }
            return _pluginConfigurators;
        }

        private static void WritePluginList()
        {
            Console.WriteLine("List of plugins");
            Console.WriteLine();
            foreach (IPluginConfigurator pluginConfigurator in GetPluginConfigurators())
            {
                Console.WriteLine("{0}", pluginConfigurator.Name);
                Console.WriteLine("\t{0}", pluginConfigurator.Description);
                Console.WriteLine("\tParams: ");
                foreach (IPluginParameter pluginParam in pluginConfigurator.GetParameters())
                {
                    Console.Write("\t\t");

                    if (!pluginParam.IsMandatory)
                        Console.Write("[");

                    Console.Write("{0}={1}", pluginParam.Name, pluginParam.TypeDescription);

                    if (!pluginParam.IsMandatory)
                        Console.WriteLine("]");
                }
                Console.WriteLine();
            }
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
            Console.WriteLine("\t\t-r --disable-url-rewriting - Disables changing urls in imported files");
            Console.WriteLine("\t\t-a --import-all-less - treats every import as less even if ending in .css");
            Console.WriteLine("\t\t-c --inline-css - Inlines CSS file imports into the output");
            Console.WriteLine("\t\t-DKey=Value - prefixes variable to the less");
            Console.WriteLine("\t\t-l --listplugins - Lists the plugins available and options");
            Console.WriteLine("\t\t-p: --plugin:pluginName[:option=value[,option=value...]] - adds the named plugin to dotless with the supplied options");
            Console.WriteLine("\tinputfile: .less file dotless should compile to CSS");
            Console.WriteLine("\toutputfile: (optional) desired filename for .css output");
            Console.WriteLine("\t\t Defaults to inputfile.css");
            Console.WriteLine("");
            Console.WriteLine("Example:");
            Console.WriteLine("\tdotless.Compiler.exe -m -w \"-p:Rtl:forceRtlTransform=true,onlyReversePrefixedRules=true\"");
            Console.WriteLine("\t\tMinify, Watch and add the Rtl plugin");
        }

        private static CompilerConfiguration GetConfigurationFromArguments(List<string> arguments)
        {
            var configuration = new CompilerConfiguration(DotlessConfiguration.GetDefault());

            foreach (var arg in arguments)
            {
                if (arg.StartsWith("-"))
                {
                    if (arg == "-m" || arg == "--minify")
                    {
                        configuration.MinifyOutput = true;
                    }
                    else if (arg == "-h" || arg == "--help" || arg == @"/?")
                    {
                        WriteHelp();
                        configuration.Help = true;
                        return configuration;
                    }
                    else if (arg == "-l" || arg == "--listplugins")
                    {
                        WritePluginList();
                        configuration.Help = true;
                        return configuration;
                    }
                    else if (arg == "-a" || arg == "--import-all-less")
                    {
                        configuration.ImportAllFilesAsLess = true;
                    }
                    else if (arg == "-c" || arg == "--inline-css")
                    {
                        configuration.InlineCssFiles = true;
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
                    else if (arg.StartsWith("-r") || arg.StartsWith("--disable-url-rewriting"))
                    {
                        configuration.DisableUrlRewriting = true;
                    }
                    else if (arg.StartsWith("-p:") || arg.StartsWith("--plugin:"))
                    {
                        var pluginName = arg.Substring(arg.IndexOf(':') + 1);
                        List<string> pluginArgs = null;
                        if (pluginName.IndexOf(':') > 0)
                        {
                            pluginArgs = pluginName.Substring(pluginName.IndexOf(':') + 1).Split(',').ToList();
                            pluginName = pluginName.Substring(0, pluginName.IndexOf(':'));
                        }

                        var pluginConfig = GetPluginConfigurators()
                            .Where(plugin => String.Compare(plugin.Name, pluginName, true) == 0)
                            .FirstOrDefault();

                        if (pluginConfig == null)
                        {
                            Console.WriteLine("Cannot find plugin {0}.", pluginName);
                            continue;
                        }

                        var pluginParams = pluginConfig.GetParameters();

                        foreach (var pluginParam in pluginParams)
                        {
                            var pluginArg = pluginArgs
                                .Where(argString => argString.StartsWith(pluginParam.Name + "="))
                                .FirstOrDefault();

                            if (pluginArg == null)
                            {
                                if (pluginParam.IsMandatory)
                                {
                                    Console.WriteLine("Missing mandatory argument {0} in plugin {1}.", pluginParam.Name, pluginName);
                                }
                                continue;
                            }
                            else
                            {
                                pluginArgs.Remove(pluginArg);
                            }

                            pluginParam.SetValue(pluginArg.Substring(pluginParam.Name.Length + 1));
                        }

                        if (pluginArgs.Count > 0)
                        {
                            for (int i = 0; i < pluginArgs.Count; i++)
                            {
                                Console.WriteLine("Did not recognise argument '{0}'", pluginArgs[i]);
                            }
                        }

                        pluginConfig.SetParameterValues(pluginParams);
                        configuration.Plugins.Add(pluginConfig);
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