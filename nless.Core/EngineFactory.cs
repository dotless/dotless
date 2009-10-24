namespace nless.Core
{
    using configuration;

    public class EngineFactory
    {
        public ILessEngine GetEngine(DotlessConfiguration configuration)
        {
            ILessEngine engine = new LessEngine();
            if (configuration.MinifyOutput)
                engine = new MinifierDecorator(engine);
            CacheConfig cacheConfig = configuration.CacheConfiguration;
            if (cacheConfig.CacheEnabled)
            {
                if (cacheConfig.CacheStragy == "file")
                    engine = new FileWatcherCacheDecorator(engine);
                if (cacheConfig.CacheStragy == "expiration")
                    engine = new TimeExpirationCacheDecorator(engine, cacheConfig.CacheExpiration);
            }
            return engine;
        }
    }
}