namespace dotless.Test.Specs.Functions
{
    using NUnit.Framework;

    public class LightnessFixture : SpecFixtureBase
    {
        [Test]
        public void TestLightness()
        {
            AssertExpression("86%", "lightness(hsl(120, 50%, 86%))");
            AssertExpression("86%", "lightness(hsl(120, 50%, 86))");
        }

        [Test]
        public void TestLightnessException()
        {
            AssertExpressionError("Expected color in function 'lightness', found 12", 10, "lightness(12)");
        }

        [Test]
        public void TestEditLightness()
        {
            //Lighten
            AssertExpression("#4c4c4c", "lightness(hsl(0, 0, 0), 30%)");
            AssertExpression("#ee0000", "lightness(#800, 20%)");
            AssertExpression("white", "lightness(#fff, 20%)");
            AssertExpression("white", "lightness(#800, 100%)");
            AssertExpression("#880000", "lightness(#800, 0%)");
            AssertExpression("rgba(238, 0, 0, 0.5)", "lightness(rgba(136, 0, 0, .5), 20%)");

            //Darken
            AssertExpression("#ff6a00", "lightness(hsl(25, 100, 80), -30%)");
            AssertExpression("#220000", "lightness(#800, -20%)");
            AssertExpression("black", "lightness(#000, -20%)");
            AssertExpression("black", "lightness(#800, -100%)");
            AssertExpression("#880000", "lightness(#800, 0%)");
            AssertExpression("rgba(34, 0, 0, 0.5)", "lightness(rgba(136, 0, 0, .5), -20%)");
        }

        [Test]
        public void TestEditLightness2()
        {
            //Lighten
            AssertExpression("#4c4c4c", "lighten(hsl(0, 0, 0), 30%)");
            AssertExpression("#ee0000", "lighten(#800, 20%)");
            AssertExpression("white", "lighten(#fff, 20%)");
            AssertExpression("white", "lighten(#800, 100%)");
            AssertExpression("#880000", "lighten(#800, 0%)");
            AssertExpression("rgba(238, 0, 0, 0.5)", "lighten(rgba(136, 0, 0, .5), 20%)");

            //Darken
            AssertExpression("#ff6a00", "darken(hsl(25, 100, 80), 30%)");
            AssertExpression("#220000", "darken(#800, 20%)");
            AssertExpression("black", "darken(#000, 20%)");
            AssertExpression("black", "darken(#800, 100%)");
            AssertExpression("#880000", "darken(#800, 0%)");
            AssertExpression("rgba(34, 0, 0, 0.5)", "darken(rgba(136, 0, 0, .5), 20%)");
        }

        [Test]
        public void TestEditLightnessOverflow()
        {
            AssertExpression("white", "lightness(#000000, 101%)");
            AssertExpression("black", "lightness(#ffffff, -101%)");
        }

        [Test]
        public void TestEditLightnessOverflow2()
        {
            AssertExpression("white", "lighten(#000000, 101%)");
            AssertExpression("black", "darken(#ffffff, 101%)");
        }

        [Test]
        public void TestEditLightnessTestsTypes()
        {
            AssertExpressionError("Expected color in function 'lightness', found \"foo\"", 10, "lightness(\"foo\", 10%)");
            AssertExpressionError("Expected number in function 'lightness', found \"foo\"", 16, "lightness(#fff, \"foo\")");
        }

        [Test]
        public void TestEditLightnessTestsTypes2()
        {
            AssertExpressionError("Expected color in function 'lighten', found \"foo\"", 8, "lighten(\"foo\", 10%)");
            AssertExpressionError("Expected number in function 'lighten', found \"foo\"", 14, "lighten(#fff, \"foo\")");
            AssertExpressionError("Expected color in function 'darken', found \"foo\"", 7, "darken(\"foo\", 10%)");
            AssertExpressionError("Expected number in function 'darken', found \"foo\"", 13, "darken(#fff, \"foo\")");
        }
    }
}