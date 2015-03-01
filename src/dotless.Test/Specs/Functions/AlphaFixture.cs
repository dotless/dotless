namespace dotless.Test.Specs.Functions
{
    using System.Collections.Generic;
    using NUnit.Framework;

    public class AlphaFixture : SpecFixtureBase
    {
        [Test]
        public void TestAlpha()
        {
            AssertExpression("1", "alpha(#123456)");
            AssertExpression("0.34", "alpha(rgba(0, 1, 2, 0.34))");
            AssertExpression("0", "alpha(hsla(0, 1, 2, 0))");
        }

        [Test]
        public void TestAlphaOpacityHack()
        {
            AssertExpression("alpha(opacity=75)", "alpha(Opacity=75)");
        }

        [Test]
        public void WhiteSpaceInAlphaExpressionIsIgnored()
        {
            AssertExpression("alpha(opacity=75)", "alpha( Opacity = 75 )");
        }

        [Test]
        public void TestAlphaOpacityHackWithVariable()
        {
            var variables = new Dictionary<string, string> {{"opacity", "88"}};

            AssertExpression("alpha(opacity=88)", "alpha(Opacity=@opacity)", variables);
        }

        [Test]
        public void TestAlphaTestsTypes()
        {
            AssertExpressionError("Expected color in function 'alpha', found 12", 6, "alpha(12)");
        }

        [Test]
        public void TestEditAlphaWarning()
        {
            var alphaWarning = "alpha(color, number) is not supported by less.js, so this will work but not compile with other less implementations." + 
                " You may want to consider using fadein(color, number) or the opposite fadeout(color, number), which does the same thing and is supported.";

            AssertExpressionLogMessage(alphaWarning, "alpha(rgba(0, 0, 0, 0.5), .25)");
            AssertExpressionNoLogMessage(alphaWarning, "fadein(rgba(0, 0, 0, 0.5), .25)");
        }

        [Test]
        public void TestEditAlpha()
        {
            // Opacify / Fade In
            AssertExpression("rgba(0, 0, 0, 0.75)", "alpha(rgba(0, 0, 0, 0.5), 25)");
            AssertExpression("rgba(0, 0, 0, 0.3)", "alpha(rgba(0, 0, 0, 0.2), 10)");
            AssertExpression("rgba(0, 0, 0, 0.7)", "alpha(rgba(0, 0, 0, 0.2), 50px)");
            AssertExpression("black", "alpha(rgba(0, 0, 0, 0.2), 80)");
            AssertExpression("black", "alpha(rgba(0, 0, 0, 0.2), 100)");
            AssertExpression("rgba(0, 0, 0, 0.2)", "alpha(rgba(0, 0, 0, 0.2), 0)");

            // Transparentize / Fade Out
            AssertExpression("rgba(0, 0, 0, 0.3)", "alpha(rgba(0, 0, 0, 0.5), -20)");
            AssertExpression("rgba(0, 0, 0, 0.1)", "alpha(rgba(0, 0, 0, 0.2), -10)");
            AssertExpression("rgba(0, 0, 0, 0.2)", "alpha(rgba(0, 0, 0, 0.5), -30px)");
            AssertExpression("transparent", "alpha(rgba(0, 0, 0, 0.2), -20)");
            AssertExpression("transparent", "alpha(rgba(0, 0, 0, 0.2), -100)");
            AssertExpression("rgba(0, 0, 0, 0.2)", "alpha(rgba(0, 0, 0, 0.2), 0)");
        }

        [Test]
        public void TestEditAlphaFade()
        {
            AssertExpression("rgba(0, 0, 0, 0.25)", "fade(rgba(0, 0, 0, 0.5), 25)");
            AssertExpression("black", "fade(rgba(0, 0, 0, 0.5), 100)");
            AssertExpression("rgba(255, 255, 255, 0.1)", "fade(white, 10)");
            AssertExpression("white", "fade(white, 120)");
        }

        [Test]
        public void TestEditAlpha2()
        {
            // Opacify / Fade In
            AssertExpression("rgba(0, 0, 0, 0.75)", "fade-in(rgba(0, 0, 0, 0.5), 25)");
            AssertExpression("rgba(0, 0, 0, 0.3)", "fade-in(rgba(0, 0, 0, 0.2), 10)");
            AssertExpression("rgba(0, 0, 0, 0.7)", "fade-in(rgba(0, 0, 0, 0.2), 50px)");
            AssertExpression("black", "fade-in(rgba(0, 0, 0, 0.2), 80)");
            AssertExpression("black", "fade-in(rgba(0, 0, 0, 0.2), 100)");
            AssertExpression("rgba(0, 0, 0, 0.2)", "fade-in(rgba(0, 0, 0, 0.2), 0)");

            // Transparentize / Fade Out
            AssertExpression("rgba(0, 0, 0, 0.3)", "fade-out(rgba(0, 0, 0, 0.5), 20)");
            AssertExpression("rgba(0, 0, 0, 0.1)", "fade-out(rgba(0, 0, 0, 0.2), 10)");
            AssertExpression("rgba(0, 0, 0, 0.2)", "fade-out(rgba(0, 0, 0, 0.5), 30px)");
            AssertExpression("transparent", "fade-out(rgba(0, 0, 0, 0.2), 20)");
            AssertExpression("transparent", "fade-out(rgba(0, 0, 0, 0.2), 100)");
            AssertExpression("rgba(0, 0, 0, 0.2)", "fade-out(rgba(0, 0, 0, 0.2), 0)");
        }

        [Test]
        public void TestEditAlphaPercent()
        {
            AssertExpression("rgba(0, 0, 0, 0.5)", "alpha(rgba(0, 0, 0, 0.5), 0%)");
            AssertExpression("rgba(0, 0, 0, 0.75)", "alpha(rgba(0, 0, 0, 0.5), 25%)");
            AssertExpression("rgba(0, 0, 0, 0.25)", "alpha(rgba(0, 0, 0, 0.5), -25%)");
        }

        [Test]
        public void TestEditAlphaPercent2()
        {
            AssertExpression("rgba(0, 0, 0, 0.5)", "fade-in(rgba(0, 0, 0, 0.5), 0%)");
            AssertExpression("rgba(0, 0, 0, 0.75)", "fade-in(rgba(0, 0, 0, 0.5), 25%)");
            AssertExpression("rgba(0, 0, 0, 0.25)", "fade-out(rgba(0, 0, 0, 0.5), 25%)");
        }

        [Test]
        public void TestEditAlphaTestsTypes()
        {
            AssertExpressionError("Expected color in function 'alpha', found \"foo\"", 6, "alpha(\"foo\", 10%)");
            AssertExpressionError("Expected number in function 'alpha', found \"foo\"", 12, "alpha(#fff, \"foo\")");
        }

        [Test]
        public void TestEditAlphaTestsTypes2()
        {
            AssertExpressionError("Expected color in function 'fade-in', found \"foo\"", 8, "fade-in(\"foo\", 10%)");
            AssertExpressionError("Expected number in function 'fade-in', found \"foo\"", 14, "fade-in(#fff, \"foo\")");
            AssertExpressionError("Expected color in function 'fade-out', found \"foo\"", 9, "fade-out(\"foo\", 10%)");
            AssertExpressionError("Expected number in function 'fade-out', found \"foo\"", 15, "fade-out(#fff, \"foo\")");
        }
    }
}