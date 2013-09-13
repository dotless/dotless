namespace dotless.Core.configuration
{
    using System;
    using Input;
    using Loggers;
    using dotless.Core.Plugins;
    using System.Collections.Generic;

    public enum DotlessSessionStateMode
    {
        /// <summary>
        ///  Session is not used.
        /// </summary>
        Disabled,

        /// <summary>
        ///  Session is loaded for each request to Dotless HTTP handler.
        /// </summary>
        Enabled,

        /// <summary>
        ///  Session is loaded when URL QueryString parameter specified by <seealso cref="DotlessConfiguration.SessionQueryParamName"/> is truthy.
        /// </summary>
        QueryParam
    }

    public class DotlessConfiguration
    {
        public const string DEFAULT_SESSION_QUERY_PARAM_NAME = "sstate";
        internal static IConfigurationManager _configurationManager;

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
            LessSource = new FileReader();
            MinifyOutput = false;
            Debug = false;
            CacheEnabled = true;
            Web = false;
            SessionMode = DotlessSessionStateMode.Disabled;
            SessionQueryParamName = DEFAULT_SESSION_QUERY_PARAM_NAME;
            Logger = null;
            LogLevel = LogLevel.Error;
            Optimization = 1;
            Plugins = new List<IPluginConfigurator>();
            MapPathsToWeb = true;
            HandleWebCompression = true;
            DisableVariableRedefines = false;
            KeepFirstSpecialComment = false;
        }

        public DotlessConfiguration(DotlessConfiguration config)
        {
            LessSource = config.LessSource;
            MinifyOutput = config.MinifyOutput;
            Debug = config.Debug;
            CacheEnabled = config.CacheEnabled;
            Web = config.Web;
            SessionMode = config.SessionMode;
            SessionQueryParamName = config.SessionQueryParamName;
            Logger = null;
            LogLevel = config.LogLevel;
            Optimization = config.Optimization;
            Plugins = new List<IPluginConfigurator>();
            Plugins.AddRange(config.Plugins);
            MapPathsToWeb = config.MapPathsToWeb;
            DisableUrlRewriting = config.DisableUrlRewriting;
            InlineCssFiles = config.InlineCssFiles;
            ImportAllFilesAsLess = config.ImportAllFilesAsLess;
            HandleWebCompression = config.HandleWebCompression;
            DisableParameters = config.DisableParameters;
            DisableVariableRedefines = config.DisableVariableRedefines;
            KeepFirstSpecialComment = config.KeepFirstSpecialComment;
        }

        public static IConfigurationManager ConfigurationManager
        {
            get { return _configurationManager ?? (_configurationManager = new ConfigurationManagerWrapper()); }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                _configurationManager = value;
            }
        }

        /// <summary>
        /// Keep first comment begining /**
        /// </summary>
        public bool KeepFirstSpecialComment { get; set; }

        /// <summary>
        ///  Disable using parameters
        /// </summary>
        public bool DisableParameters { get; set; }

        /// <summary>
        ///  Stops URL's being adjusted depending on the imported file location
        /// </summary>
        public bool DisableUrlRewriting { get; set; }

        /// <summary>
        ///  Disables variables being redefined, so less will search from the bottom of the input up.
        ///  Makes dotless behave like less.js with regard variables
        /// </summary>
        public bool DisableVariableRedefines { get; set; }

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
        ///  Prints helpful comments in the output while debugging.
        /// </summary>
        public bool Debug { get; set; }

        /// <summary>
        ///  For web handlers output in a cached mode. Reccommended on.
        /// </summary>
        public bool CacheEnabled { get; set; }

        /// <summary>
        ///  IFileReader type to use to get imported files
        /// </summary>
        public IFileReader LessSource { get; set; }

        /// <summary>
        ///  Whether this is used in a web context or not
        /// </summary>
        public bool Web { get; set; }

        /// <summary>
        ///  Specifies the mode the HttpContext.Session is loaded.
        /// </summary>
        public DotlessSessionStateMode SessionMode { get; set; }

        /// <summary>
        ///  Gets or sets the URL QueryString parameter name used in conjunction with <seealso cref="SessionMode"/> set to <seealso cref="DotlessSessionStateMode.QueryParam"/>.
        /// </summary>
        public string SessionQueryParamName { get; set; }

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
        ///  Whether to handle the compression (e.g. look at Accept-Encoding) - true or leave it to IIS - false
        /// </summary>
        public bool HandleWebCompression { get; set; }

        /// <summary>
        /// Plugins to use
        /// </summary>
        public List<IPluginConfigurator> Plugins { get; private set; }
    }
}