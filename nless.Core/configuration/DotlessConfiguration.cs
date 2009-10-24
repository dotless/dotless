namespace nless.Core.configuration
{
    using System;

    public class DotlessConfiguration
    {
        public bool MinifyOutput { get; set; }
        public CacheConfig CacheConfiguration { get; set;}
    }

    public class CacheConfig
    {
        public bool CacheEnabled { get; set; }
        public string CacheStragy { get; set; }
        public long CacheExpiration { get; set; }
    }
}