namespace dotless.Test.Unit
{
    using Core;
    using Core.configuration;
    using NUnit.Framework;

    public class ConfigurationFixture
    {
        [Test]
        public void DefaultEngineIsCacheDecorator()
        {
            var engine = new EngineFactory().GetEngine();

            Assert.That(engine is CacheDecorator);

            var aspEngine = (CacheDecorator)engine;
            Assert.That(aspEngine.Underlying is LessEngine);
        }

        [Test]
        public void IfMinifyOptionSetEngineIsMinifierDecorator()
        {
            var config = new DotlessConfiguration { MinifyOutput = true, CacheEnabled = false };

            var engine = new EngineFactory(config).GetEngine();

            Assert.That(engine is MinifierDecorator);

            var minEngine = (MinifierDecorator)engine;
            Assert.That(minEngine.Engine is LessEngine);
        }

        [Test]
        public void IfAspOptionSetButCachedIsFalseEngineIsLessEngine()
        {
            var config = new DotlessConfiguration { Web = true, CacheEnabled = false };

            var engine = new EngineFactory(config).GetEngine();

            Assert.That(engine is LessEngine);
        }

        [Test]
        public void IfAspAndCacheOptionsSetEngineIsAspCacheDecorator()
        {
            var config = new DotlessConfiguration { Web = true, CacheEnabled = true };

            var engine = new EngineFactory(config).GetEngine();

            Assert.That(engine is CacheDecorator);

            var aspEngine = (CacheDecorator)engine;
            Assert.That(aspEngine.Underlying is LessEngine);
        }

        [Test]
        public void IfAspCacheAndMinifyOptionsSetEngineIsAspCacheDecoratorThenMinifierDecorator()
        {
            var config = new DotlessConfiguration { Web = true, CacheEnabled = true, MinifyOutput = true };

            var engine = new EngineFactory(config).GetEngine();

            Assert.That(engine is CacheDecorator);

            var aspEngine = (CacheDecorator)engine;
            Assert.That(aspEngine.Underlying is MinifierDecorator);

            var minEngine = (MinifierDecorator)aspEngine.Underlying;
            Assert.That(minEngine.Engine is LessEngine);
        }
    }
}