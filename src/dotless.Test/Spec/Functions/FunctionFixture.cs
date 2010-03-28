using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NUnit.Framework.Constraints;


namespace dotless.Test.Spec.Functions
{
    [TestFixture]
    public class FunctionFixture : SpecFixtureBase
    {
        // Note: these tests were modified from http://github.com/nex3/haml/blob/0e249c844f66bd0872ed68d99de22b774794e967/test/sass/functions_test.rb

        [Test]
        public void TestExpressionsAsArguments()
        {
            AssertExpression("#03070b", "rgb((1+2), (3+4), (5+6))");
            AssertExpression("#03070b", "rgb(1+2, 3+4, 5+6)");
            AssertExpression("#33cccc", "hsl((100 + 80), 60%, 50%)");
            AssertExpression("#33cccc", "hsl(100 + 80, 60%, 50%)");
        }

        [Test]
        public void TestVariablesAsArguments()
        {
            var variables = new Dictionary<string, string> {{"c", "#123456"}};
            AssertExpression("#123456", "rgba(@c, 1)", variables);
        }

        [Test]
        public void TestHsl()
        {
            AssertExpression("#33cccc", "hsl(180, 60%, 50%)");
        }

        [Test]
        public void TestHslOverflows()
        {
            AssertExpression("#1f1f1f", "hsl(10, -114, 12)");
            AssertExpression("white", "hsl(10, 10, 256%)");

            AssertExpression("350", "hue(hsl(-10, 10, 10))");
            AssertExpression("40", "hue(hsl(400, 10, 10))");
            AssertExpression("1", "hue(hsl(721, 10, 10))");
            AssertExpression("359", "hue(hsl(-721, 10, 10))");
        }

        [Test]
        public void TestHslChecksTypes()
        {
            AssertErrorMessage("Expected number in function 'hsl', found \"foo\"", "hsl(\"foo\", 10, 12)");
            AssertErrorMessage("Expected number in function 'hsl', found \"foo\"", "hsl(10, \"foo\", 12)");
            AssertErrorMessage("Expected number in function 'hsl', found \"foo\"", "hsl(10, 10, \"foo\")");
        }

        [Test]
        public void TestHsla()
        {
            AssertExpression("rgba(51, 204, 204, .4)", "hsla(180, 60%, 50%, .4)");
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

            AssertExpression("350", "hue(hsla(-10, 10, 10, 1))");
            AssertExpression("40", "hue(hsla(400, 10, 10, .5))");
            AssertExpression("1", "hue(hsla(721, 10, 10, 0))");
            AssertExpression("359", "hue(hsla(-721, 10, 10, 1))");
        }

        [Test]
        public void TestHslaChecksTypes()
        {
            AssertErrorMessage("Expected number in function 'hsla', found \"foo\"", "hsla(\"foo\", 10, 12, 0.3)");
            AssertErrorMessage("Expected number in function 'hsla', found \"foo\"", "hsla(10, \"foo\", 12, 0)");
            AssertErrorMessage("Expected number in function 'hsla', found \"foo\"", "hsla(10, 10, \"foo\", 1)");
            AssertErrorMessage("Expected number in function 'hsla', found \"foo\"", "hsla(10, 10, 10, \"foo\")");
        }

        [Test]
        public void TestPercentage()
        {
            AssertExpression("25%", "percentage(25%)");
            AssertExpression("2500%", "percentage(25)");
            AssertExpression("50%", "percentage(.5)");
            AssertExpression("100%", "percentage(1)");
            AssertExpression("25%", "percentage(25px / 100px)");
        }

        [Test]
        public void TestPercentageChecksTypes()
        {
            AssertErrorMessage("Expected unitless number in function 'percentage', found 25px", "percentage(25px)");
            AssertErrorMessage("Expected number in function 'percentage', found #cccccc", "percentage(#ccc)");
            AssertErrorMessage("Expected number in function 'percentage', found 'string'", "percentage('string')");
        }

        [Test]
        public void TestRound()
        {
            AssertExpression("4", "round(4)");
            AssertExpression("5", "round(4.8)");
            AssertExpression("5px", "round(4.8px)");
            AssertExpression("5px", "round(5.49px)");
            AssertExpression("50%", "round(50.1%)");

            AssertErrorMessage("Expected number in function 'round', found #cccccc", "round(#ccc)");
        }

        [Test]
        public void TestFloor()
        {
            AssertExpression("4", "floor(4.8)");
            AssertExpression("4px", "floor(4.8px)");
            AssertExpression("5px", "floor(5.49px)");
            AssertExpression("50%", "floor(50.1%)");

            AssertErrorMessage("Expected number in function 'floor', found \"foo\"", "floor(\"foo\")");
        }

        [Test]
        public void TestCeil()
        {
            AssertExpression("4", "ceil(4)");
            AssertExpression("5", "ceil(4.8)");
            AssertExpression("5px", "ceil(4.8px)");
            AssertExpression("6px", "ceil(5.49px)");
            AssertExpression("51%", "ceil(50.1%)");

            AssertErrorMessage("Expected number in function 'ceil', found \"a\"", "ceil(\"a\")");
        }

        [Test]
        public void TestAbs()
        {
            AssertExpression("5", "abs(-5)");
            AssertExpression("5", "abs(5)");
            AssertExpression("5px", "abs(-5px)");
            AssertExpression("5px", "abs(5px)");

            AssertErrorMessage("Expected number in function 'abs', found #aaaaaa", "abs(#aaa)");
        }

        [Test]
        public void TestRgb()
        {
            AssertExpression("#123456", "rgb(18, 52, 86)");
            AssertExpression("#beaded", "rgb(190, 173, 237)");
            AssertExpression("#00ff7f", "rgb(0, 255, 127)");
        }

        [Test]
        public void TestRgbPercent()
        {
            AssertExpression("#123456", "rgb(7.1%, 20.4%, 33.7%)");
            AssertExpression("#beaded", "rgb(74.7%, 173, 93%)");
            AssertExpression("#beaded", "rgb(190, 68%, 237)");
            AssertExpression("#00ff80", "rgb(0%, 100%, 50%)");
        }

        [Test]
        public void TestRgbOverflows()
        {
            AssertExpression("#ff0101", "rgb(256, 1, 1)");
            AssertExpression("#01ff01", "rgb(1, 256, 1)");
            AssertExpression("#0101ff", "rgb(1, 1, 256)");
            AssertExpression("#01ffff", "rgb(1, 256, 257)");
            AssertExpression("#000101", "rgb(-1, 1, 1)");
        }

        [Test]
        public void TestRgbTestPercentBounds()
        {
            AssertExpression("red", "rgb(100.1%, 0, 0)");
            AssertExpression("black", "rgb(0, -0.1%, 0)");
            AssertExpression("blue", "rgb(0, 0, 101%)");
        }

        [Test]
        public void TestRgbTestsTypes()
        {
            AssertErrorMessage("Expected number in function 'rgb', found \"foo\"", "rgb(\"foo\", 10, 12)");
            AssertErrorMessage("Expected number in function 'rgb', found \"foo\"", "rgb(10, \"foo\", 12)");
            AssertErrorMessage("Expected number in function 'rgb', found \"foo\"", "rgb(10, 10, \"foo\")");
        }

        [Test]
        public void TestRgba()
        {
            AssertExpression("rgba(18, 52, 86, .5)", "rgba(18, 52, 86, .5)");
            AssertExpression("#beaded", "rgba(190, 173, 237, 1)");
            AssertExpression("rgba(0, 255, 127, 0)", "rgba(0, 255, 127, 0)");
        }

        [Test]
        public void TestRgbaOverflows()
        {
            AssertExpression("rgba(255, 1, 1, .3)", "rgba(256, 1, 1, 0.3)");
            AssertExpression("rgba(1, 1, 255, .3)", "rgba(1, 1, 256, 0.3)");
            AssertExpression("rgba(1, 255, 255, .3)", "rgba(1, 256, 257, 0.3)");
            AssertExpression("rgba(0, 1, 1, .3)", "rgba(-1, 1, 1, 0.3)");
            AssertExpression("rgba(1, 1, 1, 0)", "rgba(1, 1, 1, -0.2)");
            AssertExpression("#010101", "rgba(1, 1, 1, 1.2)");
        }

        [Test]
        public void TestRgbaTestsTypes()
        {
            AssertErrorMessage("Expected number in function 'rgba', found \"foo\"", "rgba(\"foo\", 10, 12, 0.2)");
            AssertErrorMessage("Expected number in function 'rgba', found \"foo\"", "rgba(10, \"foo\", 12, 0.1)");
            AssertErrorMessage("Expected number in function 'rgba', found \"foo\"", "rgba(10, 10, \"foo\", 0)");
            AssertErrorMessage("Expected number in function 'rgba', found \"foo\"", "rgba(10, 10, 10, \"foo\")");
        }

        [Test]
        public void TestRgbaWithColor()
        {
            AssertExpression("rgba(16, 32, 48, .5)", "rgba(#102030, .5)");
            AssertExpression("rgba(0, 0, 255, .5)", "rgba(blue, 0.5)");
        }

        [Test]
        public void TestRgbaWithColorTestsTypes()
        {
            AssertErrorMessage("Expected color in function 'rgba', found \"foo\"", "rgba(\"foo\", 0.2)");
            AssertErrorMessage("Expected number in function 'rgba', found \"foo\"", "rgba(#123456, \"foo\")");
        }

        [Test]
        public void TestRgbaTestsNumArgs()
        {
            AssertErrorMessage("Expected 4 arguments in function 'rgba', found 0", "rgba()");
            AssertErrorMessage("Expected 4 arguments in function 'rgba', found 1", "rgba(blue)");
            AssertErrorMessage("Expected 4 arguments in function 'rgba', found 3", "rgba(1, 2, 3)");
            AssertErrorMessage("Expected 4 arguments in function 'rgba', found 5", "rgba(1, 2, 3, 0.4, 5)");
        }

        [Test]
        public void TestRed()
        {
            AssertExpression("18", "red(#123456)");
        }

        [Test]
        public void TestRedException()
        {
            AssertErrorMessage("Expected color in function 'red', found 12", "red(12)");
        }

        [Test]
        public void TestGreen()
        {
            AssertExpression("52", "green(#123456)");
        }

        [Test]
        public void TestGreenException()
        {
            AssertErrorMessage("Expected color in function 'green', found 12", "green(12)");
        }

        [Test]
        public void TestBlue()
        {
            AssertExpression("86", "blue(#123456)");
        }

        [Test]
        public void TestBlueException()
        {
            AssertErrorMessage("Expected color in function 'blue', found 12", "blue(12)");
        }

        [Test]
        public void TestHue()
        {
            AssertExpression("18", "hue(hsl(18, 50%, 20%))");
        }

        [Test]
        public void TestHueException()
        {
            AssertErrorMessage("Expected color in function 'hue', found 12", "hue(12)");
        }

        [Test]
        public void TestSaturation()
        {
            AssertExpression("52%", "saturation(hsl(20, 52%, 20%))");
            AssertExpression("52%", "saturation(hsl(20, 52, 20%))");
        }

        [Test]
        public void TestSaturationException()
        {
            AssertErrorMessage("Expected color in function 'saturation', found 12", "saturation(12)");
        }

        [Test]
        public void TestLightness()
        {
            AssertExpression("86%", "lightness(hsl(120, 50%, 86%))");
            AssertExpression("86%", "lightness(hsl(120, 50%, 86))");
        }

        [Test]
        public void TestLightnessException()
        {
            AssertErrorMessage("Expected color in function 'lightness', found 12", "lightness(12)");
        }

        [Test]
        public void TestAlpha()
        {
            AssertExpression("1", "alpha(#123456)");
            AssertExpression(".34", "alpha(rgba(0, 1, 2, 0.34))");
            AssertExpression("0", "alpha(hsla(0, 1, 2, 0))");
        }

        [Test]
        public void TestAlphaOpacityHack()
        {
            AssertExpression("ALPHA(Opacity=75)", "alpha(Opacity=75)");
        }

        [Test]
        public void TestAlphaTestsTypes()
        {
            AssertErrorMessage("Expected color in function 'alpha', found 12", "alpha(12)");
        }

        [Test]
        public void TestEditRed()
        {
            AssertExpression("#1c3456", "red(#123456, 10)");
        }

        [Test]
        public void TestEditRedTestsTypes()
        {
            AssertErrorMessage("Expected color in function 'red', found 12", "red(12)");
        }

        [Test]
        public void TestEditGreen()
        {
            AssertExpression("#123e56", "green(#123456, 10)");
        }

        [Test]
        public void TestEditGreenTestsTypes()
        {
            AssertErrorMessage("Expected color in function 'green', found 12", "green(12)");
        }

        [Test]
        public void TestEditBlue()
        {
            AssertExpression("#123460", "blue(#123456, 10)");
        }

        [Test]
        public void TestEditBlueTestsTypes()
        {
            AssertErrorMessage("Expected color in function 'blue', found 12", "blue(12)");
        }

        [Test]
        public void TestEditAlpha()
        {
            // Opacify / Fade In
            AssertExpression("rgba(0, 0, 0, .75)", "alpha(rgba(0, 0, 0, 0.5), .25)");
            AssertExpression("rgba(0, 0, 0, .3)", "alpha(rgba(0, 0, 0, 0.2), .1)");
            AssertExpression("rgba(0, 0, 0, .7)", "alpha(rgba(0, 0, 0, 0.2), .5px)");
            AssertExpression("black", "alpha(rgba(0, 0, 0, 0.2), 0.8)");
            AssertExpression("black", "alpha(rgba(0, 0, 0, 0.2), 1)");
            AssertExpression("rgba(0, 0, 0, .2)", "alpha(rgba(0, 0, 0, 0.2), 0)");

            // Transparentize / Fade Out
            AssertExpression("rgba(0, 0, 0, .3)", "alpha(rgba(0, 0, 0, 0.5), -.2)");
            AssertExpression("rgba(0, 0, 0, .1)", "alpha(rgba(0, 0, 0, 0.2), -.1)");
            AssertExpression("rgba(0, 0, 0, .2)", "alpha(rgba(0, 0, 0, 0.5), -.3px)");
            AssertExpression("rgba(0, 0, 0, 0)", "alpha(rgba(0, 0, 0, 0.2), -0.2)");
            AssertExpression("rgba(0, 0, 0, 0)", "alpha(rgba(0, 0, 0, 0.2), -1)");
            AssertExpression("rgba(0, 0, 0, .2)", "alpha(rgba(0, 0, 0, 0.2), 0)");
        }

        [Test]
        public void TestEditAlphaPercent()
        {
            AssertExpression("rgba(0, 0, 0, .5)", "alpha(rgba(0, 0, 0, 0.5), 0%)");
            AssertExpression("rgba(0, 0, 0, .75)", "alpha(rgba(0, 0, 0, 0.5), 25%)");
            AssertExpression("rgba(0, 0, 0, .25)", "alpha(rgba(0, 0, 0, 0.5), -25%)");
        }


        [Test]
        public void TestEditAlphaTestsTypes()
        {
            AssertErrorMessage("Expected color in function 'alpha', found \"foo\"", "alpha(\"foo\", 10%)");
            AssertErrorMessage("Expected number in function 'alpha', found \"foo\"", "alpha(#fff, \"foo\")");
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
            AssertExpression("rgba(238, 0, 0, .5)", "lightness(rgba(136, 0, 0, .5), 20%)");

            //Darken
            AssertExpression("#ff6a00", "lightness(hsl(25, 100, 80), -30%)");
            AssertExpression("#220000", "lightness(#800, -20%)");
            AssertExpression("black", "lightness(#000, -20%)");
            AssertExpression("black", "lightness(#800, -100%)");
            AssertExpression("#880000", "lightness(#800, 0%)");
            AssertExpression("rgba(34, 0, 0, .5)", "lightness(rgba(136, 0, 0, .5), -20%)");
        }

        [Test]
        public void TestEditLightnessOverflow()
        {
            AssertExpression("white", "lightness(#000000, 101%)");
            AssertExpression("black", "lightness(#ffffff, -101%)");

        }

        [Test]
        public void TestEditLightnessTestsTypes()
        {
            AssertErrorMessage("Expected color in function 'lightness', found \"foo\"", "lightness(\"foo\", 10%)");
            AssertErrorMessage("Expected number in function 'lightness', found \"foo\"", "lightness(#fff, \"foo\")");
        }

        [Test]
        public void TestEditSaturation()
        {
            //Saturate
            AssertExpression("#d9f2d9", "saturation(hsl(120, 30, 90), 20%)");
            AssertExpression("#9e3f3f", "saturation(#855, 20%)");
            AssertExpression("black", "saturation(#000, 20%)");
            AssertExpression("white", "saturation(#fff, 20%)");
            AssertExpression("#33ff33", "saturation(#8a8, 100%)");
            AssertExpression("#88aa88", "saturation(#8a8, 0%)");
            AssertExpression("rgba(158, 63, 63, .5)", "saturation(rgba(136, 85, 85, 0.5), 20%)");

            // Desaturate
            AssertExpression("#e3e8e3", "saturation(hsl(120, 30, 90), -20%)");
            AssertExpression("#726b6b", "saturation(#855, -20%)");
            AssertExpression("black", "saturation(#000, -20%)");
            AssertExpression("white", "saturation(#fff, -20%)");
            AssertExpression("#999999", "saturation(#8a8, -100%)");
            AssertExpression("#88aa88", "saturation(#8a8, 0%)");
            AssertExpression("rgba(114, 107, 107, .5)", "saturation(rgba(136, 85, 85, .5), -20%)");
        }

        [Test]
        public void TestEditSaturationOverflow()
        {
            AssertExpression("#33ff33", "saturation(#8a8, 101%)");
            AssertExpression("#999999", "saturation(#8a8, -101%)");
        }

        [Test]
        public void TestEditSaturationTestsTypes()
        {
            AssertErrorMessage("Expected color in function 'saturation', found \"foo\"", "saturation(\"foo\", 10%)");
            AssertErrorMessage("Expected number in function 'saturation', found \"foo\"", "saturation(#fff, \"foo\")");
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
            AssertExpression("rgba(136, 106, 17, .5)", "hue(rgba(136, 17, 17, .5), 45)");
        }

        [Test]
        public void TestEditHueTestsTypes()
        {
            AssertErrorMessage("Expected color in function 'hue', found \"foo\"", "hue(\"foo\", 10%)");
            AssertErrorMessage("Expected number in function 'hue', found \"foo\"", "hue(#fff, \"foo\")");
        }

        [Test]
        public void TestMix()
        {
            AssertExpression("purple", "mix(#f00, #00f)");
            AssertExpression("gray", "mix(#f00, #0ff)");
            AssertExpression("#809155", "mix(#f70, #0aa)");
            AssertExpression("#4000bf", "mix(#f00, #00f, 25%)");
            AssertExpression("rgba(64, 0, 191, .75)", "mix(rgba(255, 0, 0, .5), #00f)");
            AssertExpression("red", "mix(#f00, #00f, 100%)");
            AssertExpression("blue", "mix(#f00, #00f, 0%)");
            AssertExpression("rgba(255, 0, 0, .5)", "mix(#f00, rgba(#00f, 0))");
            AssertExpression("rgba(0, 0, 255, .5)", "mix(rgba(#f00, 0), #00f)");
            AssertExpression("red", "mix(#f00, rgba(#00f, 0), 100%)");
            AssertExpression("blue", "mix(rgba(#f00, 0), #00f, 0%)");
            AssertExpression("rgba(0, 0, 255, 0)", "mix(#f00, rgba(#00f, 0), 0%)");
            AssertExpression("rgba(255, 0, 0, 0)", "mix(rgba(#f00, 0), #00f, 100%)");
        }

        [Test]
        public void TestMixTestsTypes()
        {
            AssertErrorMessage("Expected color in function 'mix', found \"foo\"", "mix(\"foo\", #f00, 10%)");
            AssertErrorMessage("Expected color in function 'mix', found \"foo\"", "mix(#f00, \"foo\", 10%)");
            AssertErrorMessage("Expected number in function 'mix', found \"foo\"", "mix(#f00, #baf, \"foo\")");
        }

        [Test]
        public void TestMixTestsBounds()
        {
            AssertExpression("#445566", "mix(#123, #456, -0.001)");
            AssertExpression("#112233", "mix(#123, #456, 100.001)");
        }

        [Test]
        public void TestGrayscale()
        {
            AssertExpression("#bbbbbb", "grayscale(#abc)");
            AssertExpression("gray", "grayscale(#f00)");
            AssertExpression("gray", "grayscale(#00f)");
            AssertExpression("white", "grayscale(#fff)");
            AssertExpression("black", "grayscale(#000)");
        }

        [Test]
        public void TestGrayscaleTestsTypes()
        {
            AssertErrorMessage("Expected color in function 'grayscale', found \"foo\"", "grayscale(\"foo\")");
        }

        [Test]
        public void TestComplement()
        {
            AssertExpression("#ccbbaa", "complement(#abc)");
            AssertExpression("aqua", "complement(#f00)");
            AssertExpression("red", "complement(#0ff)");
            AssertExpression("white", "complement(#fff)");
            AssertExpression("black", "complement(#000)");
        }

        [Test]
        public void TestComplementTestsTypes()
        {
            AssertErrorMessage("Expected color in function 'complement', found \"foo\"", "complement(\"foo\")");
        }

        [Test]
        public void TestFormat()
        {
            AssertExpression("abc", "format('abc')");
            AssertExpression("abc", "format(\"abc\")");
            AssertExpression("'abc'", @"format('\'abc\'')");
            AssertExpression("\"abc\"", @"format('\""abc\""')");

            AssertExpression("abc", "format('abc', 'd', 'e')");
            AssertExpression("abc d", "format('abc {0}', 'd', 'e')");
            AssertExpression("abc d e", "format('abc {0} {1}', 'd', 'e')");
            AssertExpression("abc e d", "format('abc {1} {0}', 'd', 'e')");

            var variables = new Dictionary<string, string> {{"x", "'def'"}, {"y", "'ghi'"}, {"z", @"'\'jkl\''"}};

            AssertExpression("abc def ghi", "format('abc {0} {1}', @x, @y)", variables);
            AssertExpression("abc def ghi 'jkl'", "format('abc {0} {1} {2}', @x, @y, @z)", variables);
        }
    }
}