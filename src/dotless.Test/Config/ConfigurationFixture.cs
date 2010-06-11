namespace dotless.Test.Config
{
    using Core;
    using Core.configuration;
    using NUnit.Framework;

    public class ConfigurationFixture
    {
        [Test]
        public void DefaultEngineIsParameterDecorator()
        {
            var engine = new EngineFactory().GetEngine();

            Assert.That(engine, Is.TypeOf<ParameterDecorator>());

            var aspEngine = (ParameterDecorator)engine;
            Assert.That(aspEngine.Underlying, Is.TypeOf<CacheDecorator>());
        }

        [Test]
        public void IfMinifyOptionSetEngineIsMinifierDecorator()
        {
            var config = new DotlessConfiguration { MinifyOutput = true, CacheEnabled = false };

            var engine = ((ParameterDecorator)new EngineFactory(config).GetEngine()).Underlying;

            Assert.That(engine, Is.TypeOf<MinifierDecorator>());

            var minEngine = (MinifierDecorator)engine;
            Assert.That(minEngine.Engine, Is.TypeOf<LessEngine>());
        }

        [Test]
        public void IfAspOptionSetButCachedIsFalseEngineIsLessEngine()
        {
            var config = new DotlessConfiguration { Web = true, CacheEnabled = false };

            var engine = ((ParameterDecorator)new EngineFactory(config).GetEngine()).Underlying;

            Assert.That(engine, Is.TypeOf<LessEngine>());
        }

        [Test]
        public void IfAspAndCacheOptionsSetEngineIsAspCacheDecorator()
        {
            var config = new DotlessConfiguration { Web = true, CacheEnabled = true };

            var engine = ((ParameterDecorator)new EngineFactory(config).GetEngine()).Underlying;

            Assert.That(engine, Is.TypeOf<CacheDecorator>());

            var aspEngine = (CacheDecorator)engine;
            Assert.That(aspEngine.Underlying, Is.TypeOf<LessEngine>());
        }

        [Test]
        public void IfAspCacheAndMinifyOptionsSetEngineIsAspCacheDecoratorThenMinifierDecorator()
        {
            var config = new DotlessConfiguration { Web = true, CacheEnabled = true, MinifyOutput = true };

            var engine = ((ParameterDecorator)new EngineFactory(config).GetEngine()).Underlying;

            Assert.That(engine, Is.TypeOf<CacheDecorator>());

            var aspEngine = (CacheDecorator)engine;
            Assert.That(aspEngine.Underlying, Is.TypeOf<MinifierDecorator>());

            var minEngine = (MinifierDecorator)aspEngine.Underlying;
            Assert.That(minEngine.Engine, Is.TypeOf<LessEngine>());
        }
    }
}