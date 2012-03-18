namespace dotless.Core.configuration
{
    using System;
    using Input;
    using Loggers;
    using dotless.Core.Plugins;
    using System.Collections.Generic;

    public class DotlessConfiguration
    {
        public static DotlessConfiguration GetDefault()
        {
            return new DotlessConfiguration();
        }

        public static DotlessConfiguration GetDefaultWeb()
        {
            return new DotlessConfiguration
            {
                Web = true
            };
        }

        public DotlessConfiguration()
        {
            LessSource = typeof (FileReader);
            MinifyOutput = false;
            CacheEnabled = true;
            Web = false;
            Logger = null;
            LogLevel = LogLevel.Error;
            Optimization = 1;
            Plugins = new List<IPluginConfigurator>();
            MapPathsToWeb = true;
        }

        public DotlessConfiguration(DotlessConfiguration config)
        {
            LessSource = config.LessSource;
            MinifyOutput = config.MinifyOutput;
            CacheEnabled = config.CacheEnabled;
            Web = config.Web;
            Logger = null;
            LogLevel = config.LogLevel;
            Optimization = config.Optimization;
            Plugins = new List<IPluginConfigurator>();
            Plugins.AddRange(config.Plugins);
            MapPathsToWeb = config.MapPathsToWeb;
            DisableUrlRewriting = config.DisableUrlRewriting;
            InlineCssFiles = config.InlineCssFiles;
            ImportAllFilesAsLess = config.ImportAllFilesAsLess;
        }

        /// <summary>
        ///  Stops URL's being adjusted depending on the imported file location
        /// </summary>
        public bool DisableUrlRewriting { get; set; }

        /// <summary>
        ///  Inlines css files into the less output
        /// </summary>
        public bool InlineCssFiles { get; set; }

        /// <summary>
        ///  import all files (even if ending in .css) as less files
        /// </summary>
        public bool ImportAllFilesAsLess { get; set; }

        /// <summary>
        /// When this is a web configuration, whether to map the paths to the website or just
        /// be relative to the current directory
        /// </summary>
        public bool MapPathsToWeb { get; set; }

        /// <summary>
        ///  Whether to minify the ouput
        /// </summary>
        public bool MinifyOutput { get; set; }

        /// <summary>
        ///  For web handlers output in a cached mode. Reccommended on.
        /// </summary>
        public bool CacheEnabled { get; set; }

        /// <summary>
        ///  IFileReader type to use to get imported files
        /// </summary>
        public Type LessSource { get; set; }

        /// <summary>
        ///  Whether this is used in a web context or not
        /// </summary>
        public bool Web { get; set; }

        /// <summary>
        ///  The ILogger type
        /// </summary>
        public Type Logger { get; set; }

        /// <summary>
        ///  The Log level
        /// </summary>
        public LogLevel LogLevel { get; set; }

        /// <summary>
        ///  Optimisation int
        ///  0 - do not chunk up the input
        ///  > 0 - chunk up output
        ///  
        ///  Recommended value - 1
        /// </summary>
        public int Optimization { get; set; }

        /// <summary>
        /// Plugins to use
        /// </summary>
        public List<IPluginConfigurator> Plugins { get; private set; }
    }
}