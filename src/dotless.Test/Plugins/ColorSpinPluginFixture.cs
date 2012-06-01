namespace dotless.Test.Plugins
{
    using System.Collections.Generic;
    using Core.Parser.Infrastructure;
    using Core.Parser.Tree;
    using Core.Plugins;
    using NUnit.Framework;

    public class ColorSpinPluginFixture : SpecFixtureBase
    {
        protected List<Color> Colors;
        protected Ruleset Tree { get; set; }
        protected ColorSpinPlugin Plugin { get; set; }

        [SetUp]
        public void Setup()
        {
            DefaultEnv = () => {
                Env env = new Env();
                env.AddPlugin(new ColorSpinPlugin(60));
                return env;
            };
        }

        [Test]
        public void ColorIsRotated60Degrees()
        {
            AssertExpression("#646400", "#640000");
            AssertExpression("#00c8c8", "#00c800");
        }

        [Test]
        public void ExpressionIsRotatedAfterEvaluation()
        {
            AssertExpression("#00c864", "#640000 + #00c800");
        }

    }
}