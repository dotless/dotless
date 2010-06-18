namespace dotless.Test.Specs.Functions
{
    using NUnit.Framework;

    public class HueFixture : SpecFixtureBase
    {
        [Test]
        public void TestHue()
        {
            AssertExpression("18deg", "hue(hsl(18, 50%, 20%))");
        }

        [Test]
        public void TestHueException()
        {
            AssertExpressionError("Expected color in function 'hue', found 12", 4, "hue(12)");
        }

        [Test]
        public void TestEditHue()
        {
            AssertExpression("#deeded", "hue(hsl(120, 30, 90), 60)");
            AssertExpression("#ededde", "hue(hsl(120, 30, 90), -60)");
            AssertExpression("#886a11", "hue(#811, 45)");
            AssertExpression("black", "hue(#000, 45)");
            AssertExpression("white", "hue(#fff, 45)");
            AssertExpression("#88aa88", "hue(#8a8, 360)");
            AssertExpression("#88aa88", "hue(#8a8, 0)");
            AssertExpression("rgba(136, 106, 17, 0.5)", "hue(rgba(136, 17, 17, .5), 45)");
        }

        [Test]
        public void TestEditHueTestsTypes()
        {
            AssertExpressionError("Expected color in function 'hue', found \"foo\"", 4, "hue(\"foo\", 10%)");
            AssertExpressionError("Expected number in function 'hue', found \"foo\"", 10, "hue(#fff, \"foo\")");
        }
    }
}