namespace nless.Core.configuration
{
    public class DotlessConfiguration
    {
        public bool MinifyOutput { get; set; }
        public CacheConfig CacheConfiguration { get; set;}
    }

    public class CacheConfig
    {
        public bool CacheEnabled { get; set; }
        public ICacheStrategy CacheStrategy { get; set; }
        public long CacheExpiration { get; set; }
    }

    public interface ICacheStrategy
    {
        
    }

    public class FileWatcherCacheStrategy : ICacheStrategy
    {
        
    }

    public class TimeExpiringCacheStrategy : ICacheStrategy
    {
        
    }
}