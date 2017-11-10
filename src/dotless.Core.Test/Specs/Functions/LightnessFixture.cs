namespace dotless.Core.Test.Specs.Functions
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
        public void TestEditLightnessInfo()
        {
            var info1 = "lightness(color, number) is not supported by less.js, so this will work but not compile with other less implementations." +
                @" You may want to consider using lighten(color, number) or its opposite darken(color, number), which does the same thing and is supported.";

            AssertExpressionLogMessage(info1, "lightness(blue, 23)");
            AssertExpressionNoLogMessage(info1, "lightness(blue)");
        }

        [Test]
        public void TestEditLightness()
        {
            //Lighten
            AssertExpression("#4c4c4c", "lightness(hsl(0, 0, 0), 30%)");
            AssertExpression("#ee0000", "lightness(#800, 20%)");
            AssertExpression("#ffffff", "lightness(#fff, 20%)");
            AssertExpression("#ffffff", "lightness(#800, 100%)");
            AssertExpression("#880000", "lightness(#800, 0%)");
            AssertExpression("rgba(238, 0, 0, 0.5)", "lightness(rgba(136, 0, 0, .5), 20%)");

            //Darken
            AssertExpression("#ff6a00", "lightness(hsl(25, 100, 80), -30%)");
            AssertExpression("#220000", "lightness(#800, -20%)");
            AssertExpression("#000000", "lightness(#000, -20%)");
            AssertExpression("#000000", "lightness(#800, -100%)");
            AssertExpression("#880000", "lightness(#800, 0%)");
            AssertExpression("rgba(34, 0, 0, 0.5)", "lightness(rgba(136, 0, 0, .5), -20%)");
        }

        [Test]
        public void TestEditLightness2()
        {
            //Lighten
            AssertExpression("#4c4c4c", "lighten(hsl(0, 0, 0), 30%)");
            AssertExpression("#ee0000", "lighten(#800, 20%)");
            AssertExpression("#ffffff", "lighten(#fff, 20%)");
            AssertExpression("#ffffff", "lighten(#800, 100%)");
            AssertExpression("#880000", "lighten(#800, 0%)");
            AssertExpression("rgba(238, 0, 0, 0.5)", "lighten(rgba(136, 0, 0, .5), 20%)");

            //Darken
            AssertExpression("#ff6a00", "darken(hsl(25, 100, 80), 30%)");
            AssertExpression("#220000", "darken(#800, 20%)");
            AssertExpression("#000000", "darken(#000, 20%)");
            AssertExpression("#000000", "darken(#800, 100%)");
            AssertExpression("#880000", "darken(#800, 0%)");
            AssertExpression("rgba(34, 0, 0, 0.5)", "darken(rgba(136, 0, 0, .5), 20%)");
        }

        [Test]
        public void TestEditLightnessOverflow()
        {
            AssertExpression("#ffffff", "lightness(#000000, 101%)");
            AssertExpression("#000000", "lightness(#ffffff, -101%)");
        }

        [Test]
        public void TestEditLightnessOverflow2()
        {
            AssertExpression("#ffffff", "lighten(#000000, 101%)");
            AssertExpression("#000000", "darken(#ffffff, 101%)");
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

        [Test]
        public void TestDarknessInsideGradient()
        {
            AssertExpression("-webkit-linear-gradient(top, white 0%, #ededed 100%)", "-webkit-linear-gradient(top, white 0%, darken(#fff, 7%) 100%)");
        }

        [Test]
        public void TestDarknessInsideGradientMixin()
        {
            var input = @"
.gradientVertical(@from, @to) {
  background: -webkit-linear-gradient(top, @from 0%,darken(@to, 7%) 100%);
}
.test {
  .gradientVertical(#fff, #fff);
}
";
            var expected = @"
.test {
  background: -webkit-linear-gradient(top, #fff 0%, #ededed 100%);
}
";
            AssertLess(input, expected);
        }
    }
}