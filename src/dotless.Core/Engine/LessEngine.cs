using dotless.Core.Plugins;

namespace dotless.Core
{
    using System.Collections.Generic;
    using System.Linq;
    using Exceptions;
    using Loggers;
    using Parser.Infrastructure;
    using Parser.Tree;
    using System.IO;

    public class LessEngine : ILessEngine
    {
        public Parser.Parser Parser { get; set; }
        public ILogger Logger { get; set; }
        public bool Compress { get; set; }
        public Env Env { get; set; }
        public IEnumerable<IPluginConfigurator> Plugins { get; set; }

        public LessEngine(Parser.Parser parser, ILogger logger, bool compress, IEnumerable<IPluginConfigurator> plugins)
        {
            Parser = parser;
            Logger = logger;
            Compress = compress;
            Plugins = plugins;
        }

        public LessEngine(Parser.Parser parser, ILogger logger, bool compress)
            : this(parser, logger, compress, null)
        {
        }

        public LessEngine(Parser.Parser parser)
            : this(parser, new ConsoleLogger(LogLevel.Error), false, null)
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

                var env = Env ?? new Env { Compress = Compress };

                if (Plugins != null)
                {
                    foreach (IPluginConfigurator configurator in Plugins)
                    {
                        env.AddPlugin(configurator.CreatePlugin());
                    }
                }

                var css = tree.ToCSS(env);

                return css;
            }
            catch (ParserException e)
            {
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