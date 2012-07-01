using dotless.Core.Plugins;

namespace dotless.Core
{
    using System.Collections.Generic;
    using System.Linq;
    using Exceptions;
    using Loggers;
    using Parser.Infrastructure;

    public class LessEngine : ILessEngine
    {
        public Parser.Parser Parser { get; set; }
        public ILogger Logger { get; set; }
        public bool Compress { get; set; }
        public bool Debug { get; set; }
        public bool DisableVariableRedefines { get; set; }
        public bool KeepFirstSpecialComment { get; set; }
        public Env Env { get; set; }
        public IEnumerable<IPluginConfigurator> Plugins { get; set; }
        public bool LastTransformationSuccessful { get; private set; }

        public LessEngine(Parser.Parser parser, ILogger logger, bool compress, bool debug, bool disableVariableRedefines, bool keepFirstSpecialComment, IEnumerable<IPluginConfigurator> plugins)
        {
            Parser = parser;
            Logger = logger;
            Compress = compress;
            Debug = debug;
            DisableVariableRedefines = disableVariableRedefines;
            Plugins = plugins;
            KeepFirstSpecialComment = keepFirstSpecialComment;
        }

        public LessEngine(Parser.Parser parser, ILogger logger, bool compress, bool debug)
            : this(parser, logger, compress, debug, false, false, null)
        {
        }

        public LessEngine(Parser.Parser parser, ILogger logger, bool compress, bool debug, bool disableVariableRedefines)
            : this(parser, logger, compress, debug, disableVariableRedefines, false, null)
        {
        }

        public LessEngine(Parser.Parser parser)
            : this(parser, new ConsoleLogger(LogLevel.Error), false, false, false, false, null)
        {
        }

        public LessEngine()
            : this(new Parser.Parser())
        {
        }

        public string TransformToCss(string source, string fileName)
        {
            try
            {
                var tree = Parser.Parse(source, fileName);

                var env = Env ?? new Env { Compress = Compress, Debug = Debug, KeepFirstSpecialComment = KeepFirstSpecialComment, DisableVariableRedefines = DisableVariableRedefines };

                if (Plugins != null)
                {
                    foreach (IPluginConfigurator configurator in Plugins)
                    {
                        env.AddPlugin(configurator.CreatePlugin());
                    }
                }

                var css = tree.ToCSS(env);

                LastTransformationSuccessful = true;
                return css;
            }
            catch (ParserException e)
            {
                LastTransformationSuccessful = false;
                Logger.Error(e.Message);
            }

            return "";
        }

        public IEnumerable<string> GetImports()
        {
            return Parser.Importer.Imports.Distinct();
        }

        public void ResetImports()
        {
            Parser.Importer.Imports.Clear();
        }

    }
}