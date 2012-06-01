using NUnit.Framework;
using dotless.Core;
using dotless.Core.Abstractions;
using dotless.Core.Cache;
using dotless.Core.Response;
using dotless.Core.configuration;

namespace dotless.Test.Config
{
    public class AspNetConfigurationFixture
    {
        private ILessEngine GetEngine(DotlessConfiguration config)
        {
            var container = new AspNetContainerFactory().GetContainer(config);
            return ((ParameterDecorator)container.GetInstance<ILessEngine>()).Underlying;
        }

        [Test]
        public void CachedCssResponseInstanceIsTransient()
        {
            var config = new DotlessConfiguration { Web = true };

            var serviceLocator = new AspNetContainerFactory().GetContainer(config);

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

            var serviceLocator = new AspNetContainerFactory().GetContainer(config);

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

            var serviceLocator = new AspNetContainerFactory().GetContainer(config);

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

        [Test]
        public void HttpInstanceIsTransient()
        {
            var config = new DotlessConfiguration { Web = true };

            var serviceLocator = new AspNetContainerFactory().GetContainer(config);

            var http1 = serviceLocator.GetInstance<IHttp>();
            var http2 = serviceLocator.GetInstance<IHttp>();

            Assert.That(http1, Is.Not.SameAs(http2));
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

            var serviceLocator = new AspNetContainerFactory().GetContainer(config);

            var cache = serviceLocator.GetInstance<ICache>();

            Assert.That(cache, Is.TypeOf<HttpCache>());
        }
    }
}