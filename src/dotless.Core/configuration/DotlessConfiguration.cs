namespace dotless.Core.configuration
{
    using System;
    using Input;
    using Loggers;

    public class DotlessConfiguration
    {
        public static readonly DotlessConfiguration Default = new DotlessConfiguration();
        public static readonly DotlessConfiguration DefaultWeb = new DotlessConfiguration
            {
                Web = true
            };

        public DotlessConfiguration()
        {
            LessSource = typeof (FileReader);
            MinifyOutput = false;
            CacheEnabled = true;
            Web = false;
            Logger = null;
            LogLevel = LogLevel.Error;
            Optimization = 1;
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
        }

        public bool MinifyOutput { get; set; }
        public bool CacheEnabled { get; set; }
        public Type LessSource { get; set; }
        public bool Web { get; set; }
        public Type Logger { get; set; }
        public LogLevel LogLevel { get; set; }
        public int Optimization { get; set; }
    }
}