namespace dotless.Core.configuration
{
    using System;
    using Input;

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
        }

        public DotlessConfiguration(DotlessConfiguration config)
        {
            LessSource = config.LessSource;
            MinifyOutput = config.MinifyOutput;
            CacheEnabled = config.CacheEnabled;
            Web = config.Web;
        }

        public bool MinifyOutput { get; set; }
        public bool CacheEnabled { get; set; }
        public Type LessSource { get; set; }
        public bool Web { get; set; }
    }
}