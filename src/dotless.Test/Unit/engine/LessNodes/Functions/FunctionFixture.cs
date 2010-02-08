using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dotless.Core.engine;
using NUnit.Framework;


namespace dotless.Test.Unit.engine.LessNodes.Functions
{
    [TestFixture]
    public class FunctionFixture
    {
        // Note: these tests are lifted from http://github.com/nex3/haml/blob/master/test/sass/functions_test.rb

        [Test, Ignore]
        public void TestHslChecksBounds()
        {
            AssertErrorMessage("Saturation -114 must be between 0% and 100% for `hsl'", "hsl(10, -114, 12)");
            AssertErrorMessage("Lightness 256 must be between 0% and 100% for `hsl'", "hsl(10, 10, 256%)");
        }

        [Test, Ignore]
        public void TestHslChecksTypes()
        {
            AssertErrorMessage("\"foo\" is not a number for `hsl'", "hsl(\"foo\", 10, 12)");
            AssertErrorMessage("\"foo\" is not a number for `hsl'", "hsl(10, \"foo\", 12)");
            AssertErrorMessage("\"foo\" is not a number for `hsl'", "hsl(10, 10, \"foo\")");
        }

        [Test]
        public void TestHsla()
        {
            Assert.AreEqual("rgba(51, 204, 204, .4)", Evaluate("hsla(180, 60%, 50%, .4)"));
            Assert.AreEqual("#33cccc", Evaluate("hsla(180, 60%, 50%, 1)"));
            Assert.AreEqual("rgba(51, 204, 204, 0)", Evaluate("hsla(180, 60%, 50%, 0)"));
        }

        [Test, Ignore]
        public void TestHslaChecksBounds()
        {
            AssertErrorMessage("Saturation -114 must be between 0% and 100% for `hsla'", "hsla(10, -114, 12, 1)");
            AssertErrorMessage("Lightness 256 must be between 0% and 100% for `hsla'", "hsla(10, 10, 256%, 0)");
            AssertErrorMessage("Alpha channel -0.1 must be between 0 and 1 for `hsla'", "hsla(10, 10, 10, -0.1)");
            AssertErrorMessage("Alpha channel 1.1 must be between 0 and 1 for `hsla'", "hsla(10, 10, 10, 1.1)");
        }

        [Test, Ignore]
        public void TestHslaChecksTypes()
        {
            AssertErrorMessage("\"foo\" is not a number for `hsla'", "hsla(\"foo\", 10, 12, 0.3)");
            AssertErrorMessage("\"foo\" is not a number for `hsla'", "hsla(10, \"foo\", 12, 0)");
            AssertErrorMessage("\"foo\" is not a number for `hsla'", "hsla(10, 10, \"foo\", 1)");
            AssertErrorMessage("\"foo\" is not a number for `hsla'", "hsla(10, 10, 10, \"foo\")");
        }

        [Test]
        public void TestPercentage()
        {
            Assert.AreEqual("50%", Evaluate("percentage(.5)"));
            Assert.AreEqual("100%", Evaluate("percentage(1)"));
            //      Assert.AreEqual("25%", Evaluate("percentage(25px / 100px)"));
        }

        [Test, Ignore]
        public void TestPercentageChecksTypes()
        {
            AssertErrorMessage("25px is not a unitless number for `percentage'", "percentage(25px)");
            AssertErrorMessage("#cccccc is not a unitless number for `percentage'", "percentage(#ccc)");
            AssertErrorMessage("\"string\" is not a unitless number for `percentage'", "percentage('string')");
        }

        [Test]
        public void TestRound()
        {
            Assert.AreEqual("5", Evaluate("round(4.8)"));
            Assert.AreEqual("5px", Evaluate("round(4.8px)"));
            Assert.AreEqual("5px", Evaluate("round(5.49px)"));
            Assert.AreEqual("50%", Evaluate("round(50.1%)"));

            //AssertErrorMessage("#cccccc is not a number for `round'", "round(#ccc)");
        }

        [Test]
        public void TestFloor()
        {
            Assert.AreEqual("4", Evaluate("floor(4.8)"));
            Assert.AreEqual("4px", Evaluate("floor(4.8px)"));
            Assert.AreEqual("5px", Evaluate("floor(5.49px)"));
            Assert.AreEqual("50%", Evaluate("floor(50.1%)"));

            //AssertErrorMessage("\"foo\" is not a number for `floor'", "floor(\"foo\")");
        }

        [Test]
        public void TestCeil()
        {
            Assert.AreEqual("5", Evaluate("ceil(4.8)"));
            Assert.AreEqual("5px", Evaluate("ceil(4.8px)"));
            Assert.AreEqual("6px", Evaluate("ceil(5.49px)"));
            Assert.AreEqual("51%", Evaluate("ceil(50.1%)"));

            //AssertErrorMessage("\"a\" is not a number for `ceil'", "ceil(\"a\")");
        }

        [Test]
        public void TestAbs()
        {
            Assert.AreEqual("5", Evaluate("abs(-5)"));
            Assert.AreEqual("5", Evaluate("abs(5)"));
            Assert.AreEqual("5px", Evaluate("abs(-5px)"));
            Assert.AreEqual("5px", Evaluate("abs(5px)"));

            //AssertErrorMessage("#aaaaaa is not a number for `abs'", "abs(#aaa)");
        }

        [Test]
        public void TestRgb()
        {
            Assert.AreEqual("#123456", Evaluate("rgb(18, 52, 86)"));
            Assert.AreEqual("#beaded", Evaluate("rgb(190, 173, 237)"));
            Assert.AreEqual("#00ff7f", Evaluate("rgb(0, 255, 127)"));
        }

        [Test]
        public void TestRgbPercent()
        {
            Assert.AreEqual("#123456", Evaluate("rgb(7.1%, 20.4%, 33.7%)"));
            Assert.AreEqual("#beaded", Evaluate("rgb(74.7%, 173, 93%)"));
            Assert.AreEqual("#beaded", Evaluate("rgb(190, 68%, 237)"));
            Assert.AreEqual("#00ff80", Evaluate("rgb(0%, 100%, 50%)"));
        }

        [Test, Ignore]
        public void TestRgbTestsBounds()
        {
            AssertErrorMessage("Color value 256 must be between 0 and 255 inclusive for `rgb'",
                                 "rgb(256, 1, 1)");
            AssertErrorMessage("Color value 256 must be between 0 and 255 inclusive for `rgb'",
                                 "rgb(1, 256, 1)");
            AssertErrorMessage("Color value 256 must be between 0 and 255 inclusive for `rgb'",
                                 "rgb(1, 1, 256)");
            AssertErrorMessage("Color value 256 must be between 0 and 255 inclusive for `rgb'",
                                 "rgb(1, 256, 257)");
            AssertErrorMessage("Color value -1 must be between 0 and 255 inclusive for `rgb'",
                                 "rgb(-1, 1, 1)");
        }

        [Test, Ignore]
        public void TestRgbTestPercentBounds()
        {
            AssertErrorMessage("Color value 100.1% must be between 0% and 100% inclusive for `rgb'",
                                 "rgb(100.1%, 0, 0)");
            AssertErrorMessage("Color value -0.1% must be between 0% and 100% inclusive for `rgb'",
                                 "rgb(0, -0.1%, 0)");
            AssertErrorMessage("Color value 101% must be between 0% and 100% inclusive for `rgb'",
                                 "rgb(0, 0, 101%)");
        }

        [Test, Ignore]
        public void TestRgbTestsTypes()
        {
            AssertErrorMessage("\"foo\" is not a number for `rgb'", "rgb(\"foo\", 10, 12)");
            AssertErrorMessage("\"foo\" is not a number for `rgb'", "rgb(10, \"foo\", 12)");
            AssertErrorMessage("\"foo\" is not a number for `rgb'", "rgb(10, 10, \"foo\")");
        }

        [Test]
        public void TestRgba()
        {
            Assert.AreEqual("rgba(18, 52, 86, .5)", Evaluate("rgba(18, 52, 86, .5)"));
            Assert.AreEqual("#beaded", Evaluate("rgba(190, 173, 237, 1)"));
            Assert.AreEqual("rgba(0, 255, 127, 0)", Evaluate("rgba(0, 255, 127, 0)"));
        }

        [Test, Ignore]
        public void TestRgbaTestsBounds()
        {
            AssertErrorMessage("Color value 256 must be between 0 and 255 inclusive for `rgba'",
                                 "rgba(256, 1, 1, 0.3)");
            AssertErrorMessage("Color value 256 must be between 0 and 255 inclusive for `rgba'",
                                 "rgba(1, 256, 1, 0.3)");
            AssertErrorMessage("Color value 256 must be between 0 and 255 inclusive for `rgba'",
                                 "rgba(1, 1, 256, 0.3)");
            AssertErrorMessage("Color value 256 must be between 0 and 255 inclusive for `rgba'",
                                 "rgba(1, 256, 257, 0.3)");
            AssertErrorMessage("Color value -1 must be between 0 and 255 inclusive for `rgba'",
                                 "rgba(-1, 1, 1, 0.3)");
            AssertErrorMessage("Alpha channel -0.2 must be between 0 and 1 inclusive for `rgba'",
                                 "rgba(1, 1, 1, -0.2)");
            AssertErrorMessage("Alpha channel 1.2 must be between 0 and 1 inclusive for `rgba'",
                                 "rgba(1, 1, 1, 1.2)");
        }

        [Test, Ignore]
        public void TestRgbaTestsTypes()
        {
            AssertErrorMessage("\"foo\" is not a number for `rgba'", "rgba(\"foo\", 10, 12, 0.2)");
            AssertErrorMessage("\"foo\" is not a number for `rgba'", "rgba(10, \"foo\", 12, 0.1)");
            AssertErrorMessage("\"foo\" is not a number for `rgba'", "rgba(10, 10, \"foo\", 0)");
            AssertErrorMessage("\"foo\" is not a number for `rgba'", "rgba(10, 10, 10, \"foo\")");
        }

        [Test]
        public void TestRgbaWithColor()
        {
            Assert.AreEqual("rgba(16, 32, 48, .5)", Evaluate("rgba(#102030, .5)"));
            // Assert.AreEqual("rgba(0, 0, 255, 0.5)", Evaluate("rgba(blue, 0.5)"));
        }

        [Test, Ignore]
        public void TestRgbaWithColorTestsTypes()
        {
            AssertErrorMessage("\"foo\" is not a color for `rgba'", "rgba(\"foo\", 0.2)");
            AssertErrorMessage("\"foo\" is not a number for `rgba'", "rgba(blue, \"foo\")");
        }

        [Test, Ignore]
        public void TestRgbaTestsNumArgs()
        {
            AssertErrorMessage("wrong number of arguments (0 for 4) for `rgba'", "rgba()");
            AssertErrorMessage("wrong number of arguments (1 for 4) for `rgba'", "rgba(blue)");
            AssertErrorMessage("wrong number of arguments (3 for 4) for `rgba'", "rgba(1, 2, 3)");
            AssertErrorMessage("wrong number of arguments (5 for 4) for `rgba'", "rgba(1, 2, 3, 0.4, 5)");
        }

        [Test]
        public void TestRed()
        {
            Assert.AreEqual("18", Evaluate("red(#123456)"));
        }

        [Test, Ignore]
        public void TestRedException()
        {
            AssertErrorMessage("12 is not a color for `red'", "red(12)");
        }

        [Test]
        public void TestGreen()
        {
            Assert.AreEqual("52", Evaluate("green(#123456)"));
        }

        [Test, Ignore]
        public void TestGreenException()
        {
            AssertErrorMessage("12 is not a color for `green'", "green(12)");
        }

        [Test]
        public void TestBlue()
        {
            Assert.AreEqual("86", Evaluate("blue(#123456)"));
        }

        [Test, Ignore]
        public void TestBlueException()
        {
            AssertErrorMessage("12 is not a color for `blue'", "blue(12)");
        }

        [Test]
        public void TestHue()
        {
            Assert.AreEqual("18", Evaluate("hue(hsl(18, 50%, 20%))"));
        }

        [Test, Ignore]
        public void TestHueException()
        {
            AssertErrorMessage("12 is not a color for `hue'", "hue(12)");
        }

        [Test]
        public void TestSaturation()
        {
            Assert.AreEqual("52%", Evaluate("saturation(hsl(20, 52%, 20%))"));
            Assert.AreEqual("52%", Evaluate("saturation(hsl(20, 52, 20%))"));
        }

        [Test, Ignore]
        public void TestSaturationException()
        {
            AssertErrorMessage("12 is not a color for `saturation'", "saturation(12)");
        }

        [Test]
        public void TestLightness()
        {
            Assert.AreEqual("86%", Evaluate("lightness(hsl(120, 50%, 86%))"));
            Assert.AreEqual("86%", Evaluate("lightness(hsl(120, 50%, 86))"));
        }

        [Test, Ignore]
        public void TestLightnessException()
        {
            AssertErrorMessage("12 is not a color for `lightness'", "lightness(12)");
        }

        [Test]
        public void TestAlpha()
        {
            Assert.AreEqual("1", Evaluate("alpha(#123456)"));
            Assert.AreEqual(".34", Evaluate("alpha(rgba(0, 1, 2, 0.34))"));
            Assert.AreEqual("0", Evaluate("alpha(hsla(0, 1, 2, 0))"));
        }

        [Test]
        public void TestEditAlpha()
        {
            // Opacify / Fade In
            Assert.AreEqual("rgba(0, 0, 0, .75)", Evaluate("alpha(rgba(0, 0, 0, 0.5), .25)"));
            Assert.AreEqual("rgba(0, 0, 0, .3)", Evaluate("alpha(rgba(0, 0, 0, 0.2), .1)"));
            Assert.AreEqual("rgba(0, 0, 0, .7)", Evaluate("alpha(rgba(0, 0, 0, 0.2), .5px)"));
            Assert.AreEqual("#000000", Evaluate("alpha(rgba(0, 0, 0, 0.2), 0.8)"));
            Assert.AreEqual("#000000", Evaluate("alpha(rgba(0, 0, 0, 0.2), 1)"));
            Assert.AreEqual("rgba(0, 0, 0, .2)", Evaluate("alpha(rgba(0, 0, 0, 0.2), 0%)"));

            // Transparentize / Fade Out
            Assert.AreEqual("rgba(0, 0, 0, .3)", Evaluate("alpha(rgba(0, 0, 0, 0.5), -.2)"));
            Assert.AreEqual("rgba(0, 0, 0, .1)", Evaluate("alpha(rgba(0, 0, 0, 0.2), -.1)"));
            Assert.AreEqual("rgba(0, 0, 0, .2)", Evaluate("alpha(rgba(0, 0, 0, 0.5), -.3px)"));
            Assert.AreEqual("rgba(0, 0, 0, 0)", Evaluate("alpha(rgba(0, 0, 0, 0.2), -0.2)"));
            Assert.AreEqual("rgba(0, 0, 0, 0)", Evaluate("alpha(rgba(0, 0, 0, 0.2), -1)"));
            Assert.AreEqual("rgba(0, 0, 0, .2)", Evaluate("alpha(rgba(0, 0, 0, 0.2), 0)"));
        }

        [Test, Ignore]
        public void TestAlphaException()
        {
            AssertErrorMessage("12 is not a color for `alpha'", "alpha(12)");
        }

        [Test]
        public void TestEditLightness()
        {
            //Lighten
            Assert.AreEqual("#4c4c4c", Evaluate("lightness(hsl(0, 0, 0), 30%)"));
            Assert.AreEqual("#ee0000", Evaluate("lightness(#800, 20%)"));
            Assert.AreEqual("#ffffff", Evaluate("lightness(#fff, 20%)"));
            Assert.AreEqual("#ffffff", Evaluate("lightness(#800, 100%)"));
            Assert.AreEqual("#880000", Evaluate("lightness(#800, 0%)"));
            Assert.AreEqual("rgba(238, 0, 0, .5)", Evaluate("lightness(rgba(136, 0, 0, .5), 20%)"));

            //Darken
            Assert.AreEqual("#ff6a00", Evaluate("lightness(hsl(25, 100, 80), -30%)"));
            Assert.AreEqual("#220000", Evaluate("lightness(#800, -20%)"));
            Assert.AreEqual("#000000", Evaluate("lightness(#000, -20%)"));
            Assert.AreEqual("#000000", Evaluate("lightness(#800, -100%)"));
            Assert.AreEqual("#880000", Evaluate("lightness(#800, 0%)"));
            Assert.AreEqual("rgba(34, 0, 0, .5)", Evaluate("lightness(rgba(136, 0, 0, .5), -20%)"));
        }

        [Test, Ignore]
        public void TestEditLightnessTestsBounds()
        {
            AssertErrorMessage("Amount -0.001 must be between 0% and 100% for `lighten'",
                                 "lightness(#123, -0.001)");
            AssertErrorMessage("Amount 100.001 must be between 0% and 100% for `lighten'",
                                 "lightness(#123, 100.001)");
        }

        [Test, Ignore]
        public void TestEditLightnessTestsTypes()
        {
            AssertErrorMessage("\"foo\" is not a color for `lighten'", "lightness(\"foo\", 10%)");
            AssertErrorMessage("\"foo\" is not a number for `lighten'", "lightness(#fff, \"foo\")");
        }

        [Test]
        public void TestEditSaturation()
        {
            //Saturate
            Assert.AreEqual("#d9f2d9", Evaluate("saturation(hsl(120, 30, 90), 20%)"));
            Assert.AreEqual("#9e3f3f", Evaluate("saturation(#855, 20%)"));
            Assert.AreEqual("#000000", Evaluate("saturation(#000, 20%)"));
            Assert.AreEqual("#ffffff", Evaluate("saturation(#fff, 20%)"));
            Assert.AreEqual("#33ff33", Evaluate("saturation(#8a8, 100%)"));
            Assert.AreEqual("#88aa88", Evaluate("saturation(#8a8, 0%)"));
            Assert.AreEqual("rgba(158, 63, 63, .5)", Evaluate("saturation(rgba(136, 85, 85, 0.5), 20%)"));

            // Desaturate
            Assert.AreEqual("#e3e8e3", Evaluate("saturation(hsl(120, 30, 90), -20%)"));
            Assert.AreEqual("#726b6b", Evaluate("saturation(#855, -20%)"));
            Assert.AreEqual("#000000", Evaluate("saturation(#000, -20%)"));
            Assert.AreEqual("#ffffff", Evaluate("saturation(#fff, -20%)"));
            Assert.AreEqual("#999999", Evaluate("saturation(#8a8, -100%)"));
            Assert.AreEqual("#88aa88", Evaluate("saturation(#8a8, 0%)"));
            Assert.AreEqual("rgba(114, 107, 107, .5)", Evaluate("saturation(rgba(136, 85, 85, .5), -20%)"));
        }

        [Test, Ignore]
        public void TestSaturateTestsBounds()
        {
            AssertErrorMessage("Amount -0.001 must be between 0% and 100% for `saturate'",
                                 "saturate(#123, -0.001)");
            AssertErrorMessage("Amount 100.001 must be between 0% and 100% for `saturate'",
                                 "saturate(#123, 100.001)");
        }

        [Test, Ignore]
        public void TestSaturateTestsTypes()
        {
            AssertErrorMessage("\"foo\" is not a color for `saturate'", "saturate(\"foo\", 10%)");
            AssertErrorMessage("\"foo\" is not a number for `saturate'", "saturate(#fff, \"foo\")");
        }


        [Test]
        public void TestEditHue()
        {
            Assert.AreEqual("#deeded", Evaluate("hue(hsl(120, 30, 90), 60)"));
            Assert.AreEqual("#ededde", Evaluate("hue(hsl(120, 30, 90), -60)"));
            Assert.AreEqual("#886a11", Evaluate("hue(#811, 45)"));
            Assert.AreEqual("#000000", Evaluate("hue(#000, 45)"));
            Assert.AreEqual("#ffffff", Evaluate("hue(#fff, 45)"));
            Assert.AreEqual("#88aa88", Evaluate("hue(#8a8, 360)"));
            Assert.AreEqual("#88aa88", Evaluate("hue(#8a8, 0)"));
            Assert.AreEqual("rgba(136, 106, 17, .5)", Evaluate("hue(rgba(136, 17, 17, .5), 45)"));
        }

        [Test, Ignore]
        public void TestAdjustHueTestsTypes()
        {
            AssertErrorMessage("\"foo\" is not a color for `adjust-hue'", "adjust-hue(\"foo\", 10%)");
            AssertErrorMessage("\"foo\" is not a number for `adjust-hue'", "adjust-hue(#fff, \"foo\")");
        }

        [Test]
        public void TestMix()
        {
            Assert.AreEqual("#800080", Evaluate("mix(#f00, #00f)"));
            Assert.AreEqual("#808080", Evaluate("mix(#f00, #0ff)"));
            Assert.AreEqual("#809055", Evaluate("mix(#f70, #0aa)"));
            Assert.AreEqual("#4000bf", Evaluate("mix(#f00, #00f, 25%)"));
            Assert.AreEqual("rgba(64, 0, 191, .75)", Evaluate("mix(rgba(255, 0, 0, .5), #00f)"));
            Assert.AreEqual("#ff0000", Evaluate("mix(#f00, #00f, 100%)"));
            Assert.AreEqual("#0000ff", Evaluate("mix(#f00, #00f, 0%)"));
            Assert.AreEqual("rgba(255, 0, 0, .5)", Evaluate("mix(#f00, rgba(#00f, 0))"));
            Assert.AreEqual("rgba(0, 0, 255, .5)", Evaluate("mix(rgba(#f00, 0), #00f)"));
            Assert.AreEqual("#ff0000", Evaluate("mix(#f00, rgba(#00f, 0), 100%)"));
            Assert.AreEqual("#0000ff", Evaluate("mix(rgba(#f00, 0), #00f, 0%)"));
            Assert.AreEqual("rgba(0, 0, 255, 0)", Evaluate("mix(#f00, rgba(#00f, 0), 0%)"));
            Assert.AreEqual("rgba(255, 0, 0, 0)", Evaluate("mix(rgba(#f00, 0), #00f, 100%)"));
        }

        [Test, Ignore]
        public void TestMixTestsTypes()
        {
            AssertErrorMessage("\"foo\" is not a color for `mix'", "mix(\"foo\", #f00, 10%)");
            AssertErrorMessage("\"foo\" is not a color for `mix'", "mix(#f00, \"foo\", 10%)");
            AssertErrorMessage("\"foo\" is not a number for `mix'", "mix(#f00, #baf, \"foo\")");
        }

        [Test, Ignore]
        public void TestMixTestsBounds()
        {
            AssertErrorMessage("Weight -0.001 must be between 0% and 100% for `mix'",
                                 "mix(#123, #456, -0.001)");
            AssertErrorMessage("Weight 100.001 must be between 0% and 100% for `mix'",
                                 "mix(#123, #456, 100.001)");
        }

        [Test]
        public void TestGrayscale()
        {
            Assert.AreEqual("#bbbbbb", Evaluate("grayscale(#abc)"));
            Assert.AreEqual("#808080", Evaluate("grayscale(#f00)"));
            Assert.AreEqual("#808080", Evaluate("grayscale(#00f)"));
            Assert.AreEqual("#ffffff", Evaluate("grayscale(#fff)"));
            Assert.AreEqual("#000000", Evaluate("grayscale(#000)"));
        }

        [Test, Ignore]
        public void TestGrayscaleTestsTypes()
        {
            AssertErrorMessage("\"foo\" is not a color for `grayscale'", "grayscale(\"foo\")");
        }

        [Test]
        public void TestComplement()
        {
            Assert.AreEqual("#ccbbaa", Evaluate("complement(#abc)"));
            Assert.AreEqual("#00ffff", Evaluate("complement(#f00)"));
            Assert.AreEqual("#ff0000", Evaluate("complement(#0ff)"));
            Assert.AreEqual("#ffffff", Evaluate("complement(#fff)"));
            Assert.AreEqual("#000000", Evaluate("complement(#000)"));
        }

        [Test, Ignore]
        public void TestComplementTestsTypes()
        {
            AssertErrorMessage("\"foo\" is not a color for `complement'", "complement(\"foo\")");
        }

        private static void AssertErrorMessage(string message, string expression)
        {
            Assert.That(() => Evaluate(expression), Throws.Exception.Message.Contains(message));
        }


        private static string Evaluate(string expression)
        {
            var less = ".def { prop: " + expression + "; }";

            var engine = new ExtensibleEngineImpl(less);

            var css = engine.Css;

            var start = css.IndexOf("prop: ");
            var end = css.LastIndexOf("}");

            return css.Substring(start + 6, end - start - 8);
        }
    }
}