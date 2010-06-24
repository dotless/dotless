namespace dotless.Test.Config
{
    using System;
    using Core;
    using Core.Abstractions;
    using Core.Cache;
    using Core.configuration;
    using Core.Input;
    using Core.Loggers;
    using Core.Parser;
    using Core.Response;
    using NUnit.Framework;

    public class ConfigurationFixture
    {
        private ILessEngine GetEngine(DotlessConfiguration config)
        {
            return ((ParameterDecorator)new EngineFactory(config).GetEngine()).Underlying;
        }

        [Test]
        public void DefaultEngineIsParameterDecorator()
        {
            var engine = new EngineFactory().GetEngine();

            Assert.That(engine, Is.TypeOf<ParameterDecorator>());
        }

        [Test]
        public void CachingIsEnabledByDefault()
        {
            var engine = new EngineFactory().GetEngine();
            engine = ((ParameterDecorator)engine).Underlying;

            Assert.That(engine, Is.TypeOf<CacheDecorator>());
        }

        [Test]
        public void IfMinifyOptionSetEngineIsLessEngine()
        {
            var config = new DotlessConfiguration { MinifyOutput = true, CacheEnabled = false };

            var engine = GetEngine(config);

            Assert.That(engine, Is.TypeOf<LessEngine>());
        }

        [Test]
        public void IfWebOptionSetButCachedIsFalseEngineIsLessEngine()
        {
            var config = new DotlessConfiguration { Web = true, CacheEnabled = false };

            var engine = GetEngine(config);

            Assert.That(engine, Is.TypeOf<LessEngine>());
        }

        [Test]
        public void IfWebAndCacheOptionsSetEngineIsCacheDecorator()
        {
            var config = new DotlessConfiguration { Web = true, CacheEnabled = true };

            var engine = GetEngine(config);

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
        public void IfWebCacheAndMinifyOptionsSetEngineIsCacheDecoratorThenLessEngine()
        {
            var config = new DotlessConfiguration { Web = true, CacheEnabled = true, MinifyOutput = true };

            var engine = GetEngine(config);

            Assert.That(engine, Is.TypeOf<CacheDecorator>());

            var aspEngine = (CacheDecorator)engine;
            Assert.That(aspEngine.Underlying, Is.TypeOf<LessEngine>());
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

        [Test]
        public void HttpInstanceIsTransient()
        {
            var config = new DotlessConfiguration { Web = true };

            var serviceLocator = new ContainerFactory().GetContainer(config);

            var http1 = serviceLocator.GetInstance<IHttp>();
            var http2 = serviceLocator.GetInstance<IHttp>();

            Assert.That(http1, Is.Not.SameAs(http2));
        }

        [Test]
        public void CachedCssResponseInstanceIsTransient()
        {
            var config = new DotlessConfiguration { Web = true };

            var serviceLocator = new ContainerFactory().GetContainer(config);

            var response1 = serviceLocator.GetInstance<IResponse>();
            var response2 = serviceLocator.GetInstance<IResponse>();

            Assert.That(response1, Is.Not.SameAs(response2));

            var http1 = (response1 as CachedCssResponse).Http;
            var http2 = (response2 as CachedCssResponse).Http;

            Assert.That(http1, Is.Not.SameAs(http2));
        }

        [Test]
        public void CssResponseInstanceIsTransient()
        {
            var config = new DotlessConfiguration { Web = true, CacheEnabled = false };

            var serviceLocator = new ContainerFactory().GetContainer(config);

            var response1 = serviceLocator.GetInstance<IResponse>();
            var response2 = serviceLocator.GetInstance<IResponse>();

            Assert.That(response1, Is.Not.SameAs(response2));

            var http1 = (response1 as CssResponse).Http;
            var http2 = (response2 as CssResponse).Http;

            Assert.That(http1, Is.Not.SameAs(http2));
        }

        [Test]
        public void HandlerImplInstanceIsTransient()
        {
            var config = new DotlessConfiguration { Web = true };

            var serviceLocator = new ContainerFactory().GetContainer(config);

            var handler1 = serviceLocator.GetInstance<HandlerImpl>();
            var handler2 = serviceLocator.GetInstance<HandlerImpl>();

            Assert.That(handler1, Is.Not.SameAs(handler2));

            var http1 = handler1.Http;
            var http2 = handler2.Http;

            Assert.That(http1, Is.Not.SameAs(http2));

            var response1 = handler1.Response;
            var response2 = handler2.Response;

            Assert.That(response1, Is.Not.SameAs(response2));

            var engine1 = handler1.Engine;
            var engine2 = handler2.Engine;

            Assert.That(engine1, Is.Not.SameAs(engine2));
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