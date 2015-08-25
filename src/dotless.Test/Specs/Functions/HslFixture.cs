namespace dotless.Test.Specs.Functions
{
    using NUnit.Framework;

    public class HslFixture : SpecFixtureBase
    {
        [Test]
        public void TestHsl()
        {
            AssertExpression("#33cccc", "hsl(180, 60%, 50%)");
        }

        [Test]
        public void TestHslOverflows()
        {
            AssertExpression("#1f1f1f", "hsl(10, -114, 12)");
            AssertExpression("#ffffff", "hsl(10, 10, 256%)");

            AssertExpression("350deg", "hue(hsl(-10, 10, 10))");
            AssertExpression("40deg", "hue(hsl(400, 10, 10))");
            AssertExpression("1deg", "hue(hsl(721, 10, 10))");
            AssertExpression("359deg", "hue(hsl(-721, 10, 10))");
        }

        [Test]
        public void TestHslChecksTypes()
        {
            AssertExpressionError("Expected number in function 'hsl', found \"foo\"", 0, "hsl(\"foo\", 10, 12)");
            AssertExpressionError("Expected number in function 'hsl', found \"foo\"", 0, "hsl(10, \"foo\", 12)");
            AssertExpressionError("Expected number in function 'hsl', found \"foo\"", 0, "hsl(10, 10, \"foo\")");
        }
    }
}