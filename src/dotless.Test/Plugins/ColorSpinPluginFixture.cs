namespace dotless.Test.Plugins
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.Parser.Infrastructure;
    using Core.Parser.Infrastructure.Nodes;
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
            DefaultEnv = () => new Env {Plugins = new List<IPlugin> {new ColorSpinPlugin(60)}};
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