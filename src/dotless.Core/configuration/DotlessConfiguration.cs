namespace dotless.Core.configuration
{
    using System;
    using Input;
    using Loggers;
    using Plugins;
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
        public const int DefaultHttpExpiryInMinutes = 10080; //7 days
        internal static IConfigurationManager _configurationManager;

        public static DotlessConfiguration GetDefault()
        {
            return new DotlessConfiguration();;
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
            Debug = false;
            CacheEnabled = true;
            HttpExpiryInMinutes = DefaultHttpExpiryInMinutes;
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
            DisableColorCompression = false;
            KeepFirstSpecialComment = false;
            RootPath = "";
            StrictMath = false;
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
            DisableColorCompression = config.DisableColorCompression;
            KeepFirstSpecialComment = config.KeepFirstSpecialComment;
            RootPath = config.RootPath;
            StrictMath = config.StrictMath;
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
        ///  Allows you to add a path to every generated import and url in your output css.
        ///  This corresponds to 'rootpath' option of lessc.
        /// </summary>
        public string RootPath { get; set; }
        
        /// <summary>
        ///  Disables variables being redefined, so less will search from the bottom of the input up.
        ///  Makes dotless behave like less.js with regard variables
        /// </summary>
        public bool DisableVariableRedefines { get; set; }

        /// <summary>
        ///  Disables hex color shortening
        /// </summary>
        public bool DisableColorCompression { get; set; }

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
        /// When <seealso cref="CacheEnabled"/> is set to true, use this parameter to set how far in the future the expires header will be set. 
        /// For example, to have the browser cache the CSS for five minutes, set this property to 5. 
        /// </summary>
        public int HttpExpiryInMinutes { get; set; }

        /// <summary>
        ///  IFileReader type to use to get imported files
        /// </summary>
        public Type LessSource { get; set; }

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

        /// <summary>
        /// Whether to only evaluate mathematical expressions when they are wrapped in an extra set of parentheses.
        /// </summary>
        public bool StrictMath { get; set; }
    }
}