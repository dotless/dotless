namespace dotless.Test.Specs.Functions
{
    using NUnit.Framework;

    public class HslaFixture : SpecFixtureBase
    {
        [Test]
        public void TestHsla()
        {
            AssertExpression("rgba(51, 204, 204, 0.4)", "hsla(180, 60%, 50%, .4)");
            AssertExpression("#33cccc", "hsla(180, 60%, 50%, 1)");
            AssertExpression("rgba(51, 204, 204, 0)", "hsla(180, 60%, 50%, 0)");
        }

        [Test]
        public void TestHslaOverflows()
        {
            AssertExpression("#1f1f1f", "hsla(10, -114, 12, 1)");
            AssertExpression("white", "hsla(10, 10, 256%, 1)");
            AssertExpression("rgba(28, 24, 23, 0)", "hsla(10, 10, 10, -0.1)");
            AssertExpression("#1c1817", "hsla(10, 10, 10, 1.1)");

            AssertExpression("350deg", "hue(hsla(-10, 10, 10, 1))");
            AssertExpression("40deg", "hue(hsla(400, 10, 10, .5))");
            AssertExpression("1deg", "hue(hsla(721, 10, 10, 0))");
            AssertExpression("359deg", "hue(hsla(-721, 10, 10, 1))");
        }

        [Test]
        public void TestHslaChecksTypes()
        {
            AssertExpressionError("Expected number in function 'hsla', found \"foo\"", 0, "hsla(\"foo\", 10, 12, 0.3)");
            AssertExpressionError("Expected number in function 'hsla', found \"foo\"", 0, "hsla(10, \"foo\", 12, 0)");
            AssertExpressionError("Expected number in function 'hsla', found \"foo\"", 0, "hsla(10, 10, \"foo\", 1)");
            AssertExpressionError("Expected number in function 'hsla', found \"foo\"", 0, "hsla(10, 10, 10, \"foo\")");
        }
    }
}