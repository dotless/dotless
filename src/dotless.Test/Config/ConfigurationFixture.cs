namespace dotless.Test.Config
{
    using System;
    using Core;
    using Core.Cache;
    using Core.configuration;
    using Core.Input;
    using Core.Loggers;
    using Core.Parser;
    using NUnit.Framework;

    public class ConfigurationFixture
    {
        [Test]
        public void DefaultEngineIsCacheDecorator()
        {
            var engine = new EngineFactory().GetEngine();

            Assert.That(engine, Is.TypeOf<CacheDecorator>());

            var aspEngine = (CacheDecorator)engine;
            Assert.That(aspEngine.Underlying, Is.TypeOf<LessEngine>());
        }

        [Test]
        public void IfMinifyOptionSetEngineIsMinifierDecorator()
        {
            var config = new DotlessConfiguration { MinifyOutput = true, CacheEnabled = false };

            var engine = new EngineFactory(config).GetEngine();

            Assert.That(engine, Is.TypeOf<MinifierDecorator>());

            var minEngine = (MinifierDecorator)engine;
            Assert.That(minEngine.Engine, Is.TypeOf<LessEngine>());
        }

        [Test]
        public void IfWebOptionSetButCachedIsFalseEngineIsLessEngine()
        {
            var config = new DotlessConfiguration { Web = true, CacheEnabled = false };

            var engine = new EngineFactory(config).GetEngine();

            Assert.That(engine, Is.TypeOf<LessEngine>());
        }

        [Test]
        public void IfWebAndCacheOptionsSetEngineIsCacheDecorator()
        {
            var config = new DotlessConfiguration { Web = true, CacheEnabled = true };

            var engine = new EngineFactory(config).GetEngine();

            Assert.That(engine, Is.TypeOf<CacheDecorator>());

            var aspEngine = (CacheDecorator)engine;
            Assert.That(aspEngine.Underlying, Is.TypeOf<LessEngine>());
        }

        [Test]
        public void IfWebAndCacheOptionsSetCacheIsHttpCache()
        {
            var config = new DotlessConfiguration { Web = true, CacheEnabled = true };

            var serviceLocator = new ContainerFactory().GetContainer(config);

            var cache = serviceLocator.GetInstance<ICache>();

            Assert.That(cache, Is.TypeOf<HttpCache>());
        }

        [Test]
        public void IfCacheOptionSetCacheIsInMemoryCache()
        {
            var config = new DotlessConfiguration { Web = false, CacheEnabled = true };

            var serviceLocator = new ContainerFactory().GetContainer(config);

            var cache = serviceLocator.GetInstance<ICache>();

            Assert.That(cache, Is.TypeOf<InMemoryCache>());
        }

        [Test]
        public void IfWebCacheAndMinifyOptionsSetEngineIsCacheDecoratorThenMinifierDecorator()
        {
            var config = new DotlessConfiguration { Web = true, CacheEnabled = true, MinifyOutput = true };

            var engine = new EngineFactory(config).GetEngine();

            Assert.That(engine, Is.TypeOf<CacheDecorator>());

            var aspEngine = (CacheDecorator)engine;
            Assert.That(aspEngine.Underlying, Is.TypeOf<MinifierDecorator>());

            var minEngine = (MinifierDecorator)aspEngine.Underlying;
            Assert.That(minEngine.Engine, Is.TypeOf<LessEngine>());
        }

        [Test]
        public void CanPassCustomLogger()
        {
            var config = new DotlessConfiguration { Logger = typeof(DummyLogger) };

            var serviceLocator = new ContainerFactory().GetContainer(config);

            var logger = serviceLocator.GetInstance<ILogger>();

            Assert.That(logger, Is.TypeOf<DummyLogger>());
        }

        [Test]
        public void CanPassCustomLessSource()
        {
            var config = new DotlessConfiguration { LessSource = typeof(DummyFileReader) };

            var serviceLocator = new ContainerFactory().GetContainer(config);

            var source = serviceLocator.GetInstance<IFileReader>();

            Assert.That(source, Is.TypeOf<DummyFileReader>());
        }

        [Test]
        public void CanOverrideOptimization()
        {
            var config = new DotlessConfiguration { Optimization = 7 };

            var serviceLocator = new ContainerFactory().GetContainer(config);

            var parser = serviceLocator.GetInstance<Parser>();

            Assert.That(parser.Tokenizer.Optimization, Is.EqualTo(7));
        }

        [Test]
        public void CanOverrideLogLevel()
        {
            var config = new DotlessConfiguration { LogLevel = LogLevel.Info };

            var serviceLocator = new ContainerFactory().GetContainer(config);

            var logger = serviceLocator.GetInstance<ILogger>();

            Assert.That(logger, Is.TypeOf<ConsoleLogger>());

            var consoleLogger = (ConsoleLogger)logger;

            Assert.That(consoleLogger.Level, Is.EqualTo(LogLevel.Info));
        }

        public class DummyLogger : Logger
        {
            public DummyLogger(LogLevel level) : base(level) { }

            protected override void Log(string message) { }
        }

        public class DummyFileReader : IFileReader
        {
            public string GetFileContents(string fileName)
            {
                throw new NotImplementedException();
            }
        }
    }
}