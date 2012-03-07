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
        }

        public bool DisableUrlRewriting { get; set; }
        /// <summary>
        /// When this is a web configuration, whether to map the paths to the website or just
        /// be relative to the current directory
        /// </summary>
        public bool MapPathsToWeb { get; set; }
        public bool MinifyOutput { get; set; }
        public bool CacheEnabled { get; set; }
        public Type LessSource { get; set; }
        public bool Web { get; set; }
        public Type Logger { get; set; }
        public LogLevel LogLevel { get; set; }
        public int Optimization { get; set; }
        public List<IPluginConfigurator> Plugins { get; private set; }
    }
}