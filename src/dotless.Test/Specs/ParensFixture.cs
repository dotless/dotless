namespace dotless.Test.Specs
{
    using System.Collections.Generic;
    using NUnit.Framework;

    public class ParensFixture : SpecFixtureBase
    {
        [Test]
        public void Parens()
        {
            var variables = new Dictionary<string, string>();
            variables["var"] = "1px";

            AssertExpression("2px solid black", "(@var * 2) solid black", variables);
            AssertExpression("1px 3px 16 3", "(@var * 1) (@var + 2) (4 * 4) 3", variables);
            AssertExpression("36", "(6 * 6)");
            AssertExpression("2px 36px", "2px (6px * 6px)");
        }

        [Test]
        public void MoreParens()
        {
            var variables = new Dictionary<string, string>();
            variables["var"] = "(2 * 2)";

            AssertExpression("8 4 4 4px", "(2 * @var) 4 4 (@var * 1px)", variables);
            AssertExpression("96", "(@var * @var) * 6", variables);
            AssertExpression("113", "(7 * 7) + (8 * 8)");
            AssertExpression("12", "4 * (5 + 5) / 2 - (@var * 2)", variables);
        }

        [Test, Ignore("Unsupported")]
        public void UnitsOutsideParens()
        {
            AssertExpression("36px", "(6 * 6)px");
        }

        [Test]
        public void NestedParens()
        {
            AssertExpression("71", "2 * (4 * (2 + (1 + 6))) - 1");
            AssertExpression("6", "((2+3)*(2+3) / (9-4)) + 1");
        }

        [Test]
        public void MixedUnits()
        {
            AssertExpressionUnchanged("2px 4em 1 5pc");
            AssertExpression("6px 1em 2px 2", "(2px + 4px) 1em 2px 2");
        }
    }
}