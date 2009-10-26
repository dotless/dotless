namespace dotless.Core.configuration
{
    public class DotlessConfiguration
    {
        public bool MinifyOutput { get; set; }
        public CacheConfig CacheConfiguration { get; set; }

        public static DotlessConfiguration Default
        {
            get
            {
                return new DotlessConfiguration
                           {
                               MinifyOutput = false,
                               CacheConfiguration = new CacheConfig {CacheEnabled = false}
                           };
            }
        }
    }

    public class CacheConfig
    {
        public bool CacheEnabled { get; set; }
        public string CacheStragy { get; set; }
        public long CacheExpiration { get; set; }
    }
}