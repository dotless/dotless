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
        // Note: these tests were modified from http://github.com/nex3/haml/blob/0e249c844f66bd0872ed68d99de22b774794e967/test/sass/functions_test.rb

        [Test]
        public void TestExpressionsAsArguments()
        {
            Assert.AreEqual("#03070b", Evaluate("rgb((1+2), (3+4), (5+6))"));
            Assert.AreEqual("#03070b", Evaluate("rgb(1+2, 3+4, 5+6)"));
            Assert.AreEqual("#33cccc", Evaluate("hsl((100 + 80), 60%, 50%)"));
            Assert.AreEqual("#33cccc", Evaluate("hsl(100 + 80, 60%, 50%)"));
        }

        [Test]
        public void TestVariablesAsArguments()
        {
            Assert.AreEqual("#123456", Evaluate("rgba(@c, 1)"));
        }

        [Test]
        public void TestHsl()
        {
            Assert.AreEqual("#33cccc", Evaluate("hsl(180, 60%, 50%)"));
        }

        [Test, Ignore]
        public void TestHslChecksBounds()
        {
            AssertErrorMessage("Saturation -114 must be between 0% and 100% for `hsl'", "hsl(10, -114, 12)");
            AssertErrorMessage("Lightness 256 must be between 0% and 100% for `hsl'", "hsl(10, 10, 256%)");
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
            Assert.AreEqual("25%", Evaluate("percentage(25%)"));
            Assert.AreEqual("2500%", Evaluate("percentage(25)"));
            Assert.AreEqual("50%", Evaluate("percentage(.5)"));
            Assert.AreEqual("100%", Evaluate("percentage(1)"));
            //      Assert.AreEqual("25%", Evaluate("percentage(25px / 100px)"));
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
            Assert.AreEqual("4", Evaluate("round(4)"));
            Assert.AreEqual("5", Evaluate("round(4.8)"));
            Assert.AreEqual("5px", Evaluate("round(4.8px)"));
            Assert.AreEqual("5px", Evaluate("round(5.49px)"));
            Assert.AreEqual("50%", Evaluate("round(50.1%)"));

            AssertErrorMessage("Expected number in function 'round', found #cccccc", "round(#ccc)");
        }

        [Test]
        public void TestFloor()
        {
            Assert.AreEqual("4", Evaluate("floor(4.8)"));
            Assert.AreEqual("4px", Evaluate("floor(4.8px)"));
            Assert.AreEqual("5px", Evaluate("floor(5.49px)"));
            Assert.AreEqual("50%", Evaluate("floor(50.1%)"));

            AssertErrorMessage("Expected number in function 'floor', found \"foo\"", "floor(\"foo\")");
        }

        [Test]
        public void TestCeil()
        {
            Assert.AreEqual("4", Evaluate("ceil(4)"));
            Assert.AreEqual("5", Evaluate("ceil(4.8)"));
            Assert.AreEqual("5px", Evaluate("ceil(4.8px)"));
            Assert.AreEqual("6px", Evaluate("ceil(5.49px)"));
            Assert.AreEqual("51%", Evaluate("ceil(50.1%)"));

            AssertErrorMessage("Expected number in function 'ceil', found \"a\"", "ceil(\"a\")");
        }

        [Test]
        public void TestAbs()
        {
            Assert.AreEqual("5", Evaluate("abs(-5)"));
            Assert.AreEqual("5", Evaluate("abs(5)"));
            Assert.AreEqual("5px", Evaluate("abs(-5px)"));
            Assert.AreEqual("5px", Evaluate("abs(5px)"));

            AssertErrorMessage("Expected number in function 'abs', found #aaaaaa", "abs(#aaa)");
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
            Assert.AreEqual("rgba(16, 32, 48, .5)", Evaluate("rgba(#102030, .5)"));
            // Assert.AreEqual("rgba(0, 0, 255, 0.5)", Evaluate("rgba(blue, 0.5)"));  // color keywords are not evaluated
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
            Assert.AreEqual("18", Evaluate("red(#123456)"));
        }

        [Test]
        public void TestRedException()
        {
            AssertErrorMessage("Expected color in function 'red', found 12", "red(12)");
        }

        [Test]
        public void TestGreen()
        {
            Assert.AreEqual("52", Evaluate("green(#123456)"));
        }

        [Test]
        public void TestGreenException()
        {
            AssertErrorMessage("Expected color in function 'green', found 12", "green(12)");
        }

        [Test]
        public void TestBlue()
        {
            Assert.AreEqual("86", Evaluate("blue(#123456)"));
        }

        [Test]
        public void TestBlueException()
        {
            AssertErrorMessage("Expected color in function 'blue', found 12", "blue(12)");
        }

        [Test]
        public void TestHue()
        {
            Assert.AreEqual("18", Evaluate("hue(hsl(18, 50%, 20%))"));
        }

        [Test]
        public void TestHueException()
        {
            AssertErrorMessage("Expected color in function 'hue', found 12", "hue(12)");
        }

        [Test]
        public void TestSaturation()
        {
            Assert.AreEqual("52%", Evaluate("saturation(hsl(20, 52%, 20%))"));
            Assert.AreEqual("52%", Evaluate("saturation(hsl(20, 52, 20%))"));
        }

        [Test]
        public void TestSaturationException()
        {
            AssertErrorMessage("Expected color in function 'saturation', found 12", "saturation(12)");
        }

        [Test]
        public void TestLightness()
        {
            Assert.AreEqual("86%", Evaluate("lightness(hsl(120, 50%, 86%))"));
            Assert.AreEqual("86%", Evaluate("lightness(hsl(120, 50%, 86))"));
        }

        [Test]
        public void TestLightnessException()
        {
            AssertErrorMessage("Expected color in function 'lightness', found 12", "lightness(12)");
        }

        [Test]
        public void TestAlpha()
        {
            Assert.AreEqual("1", Evaluate("alpha(#123456)"));
            Assert.AreEqual(".34", Evaluate("alpha(rgba(0, 1, 2, 0.34))"));
            Assert.AreEqual("0", Evaluate("alpha(hsla(0, 1, 2, 0))"));
        }

        [Test]
        public void TestAlphaOpacityHack()
        {
            Assert.AreEqual("ALPHA(Opacity=75)", Evaluate("alpha(Opacity=75)"));
        }

        [Test]
        public void TestAlphaTestsTypes()
        {
            AssertErrorMessage("Expected color in function 'alpha', found 12", "alpha(12)");
        }

        [Test]
        public void TestEditRed()
        {
            Assert.AreEqual("#1c3456", Evaluate("red(#123456, 10)"));
        }

        [Test]
        public void TestEditRedTestsTypes()
        {
            AssertErrorMessage("Expected color in function 'red', found 12", "red(12)");
        }

        [Test]
        public void TestEditGreen()
        {
            Assert.AreEqual("#123e56", Evaluate("green(#123456, 10)"));
        }

        [Test]
        public void TestEditGreenTestsTypes()
        {
            AssertErrorMessage("Expected color in function 'green', found 12", "green(12)");
        }

        [Test]
        public void TestEditBlue()
        {
            Assert.AreEqual("#123460", Evaluate("blue(#123456, 10)"));
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
            Assert.AreEqual("rgba(0, 0, 0, .75)", Evaluate("alpha(rgba(0, 0, 0, 0.5), .25)"));
            Assert.AreEqual("rgba(0, 0, 0, .3)", Evaluate("alpha(rgba(0, 0, 0, 0.2), .1)"));
            Assert.AreEqual("rgba(0, 0, 0, .7)", Evaluate("alpha(rgba(0, 0, 0, 0.2), .5px)"));
            Assert.AreEqual("#000000", Evaluate("alpha(rgba(0, 0, 0, 0.2), 0.8)"));
            Assert.AreEqual("#000000", Evaluate("alpha(rgba(0, 0, 0, 0.2), 1)"));
            Assert.AreEqual("rgba(0, 0, 0, .2)", Evaluate("alpha(rgba(0, 0, 0, 0.2), 0)"));

            // Transparentize / Fade Out
            Assert.AreEqual("rgba(0, 0, 0, .3)", Evaluate("alpha(rgba(0, 0, 0, 0.5), -.2)"));
            Assert.AreEqual("rgba(0, 0, 0, .1)", Evaluate("alpha(rgba(0, 0, 0, 0.2), -.1)"));
            Assert.AreEqual("rgba(0, 0, 0, .2)", Evaluate("alpha(rgba(0, 0, 0, 0.5), -.3px)"));
            Assert.AreEqual("rgba(0, 0, 0, 0)", Evaluate("alpha(rgba(0, 0, 0, 0.2), -0.2)"));
            Assert.AreEqual("rgba(0, 0, 0, 0)", Evaluate("alpha(rgba(0, 0, 0, 0.2), -1)"));
            Assert.AreEqual("rgba(0, 0, 0, .2)", Evaluate("alpha(rgba(0, 0, 0, 0.2), 0)"));
        }

        [Test]
        public void TestEditAlphaPercent()
        {
            Assert.AreEqual("rgba(0, 0, 0, .5)", Evaluate("alpha(rgba(0, 0, 0, 0.5), 0%)"));
            Assert.AreEqual("rgba(0, 0, 0, .75)", Evaluate("alpha(rgba(0, 0, 0, 0.5), 25%)"));
            Assert.AreEqual("rgba(0, 0, 0, .25)", Evaluate("alpha(rgba(0, 0, 0, 0.5), -25%)"));
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
        public void TestEditSaturationTestsBounds()
        {
            AssertErrorMessage("Amount -0.001 must be between 0% and 100% for `saturate'",
                                 "saturate(#123, -0.001)");
            AssertErrorMessage("Amount 100.001 must be between 0% and 100% for `saturate'",
                                 "saturate(#123, 100.001)");
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
            Assert.AreEqual("#deeded", Evaluate("hue(hsl(120, 30, 90), 60)"));
            Assert.AreEqual("#ededde", Evaluate("hue(hsl(120, 30, 90), -60)"));
            Assert.AreEqual("#886a11", Evaluate("hue(#811, 45)"));
            Assert.AreEqual("#000000", Evaluate("hue(#000, 45)"));
            Assert.AreEqual("#ffffff", Evaluate("hue(#fff, 45)"));
            Assert.AreEqual("#88aa88", Evaluate("hue(#8a8, 360)"));
            Assert.AreEqual("#88aa88", Evaluate("hue(#8a8, 0)"));
            Assert.AreEqual("rgba(136, 106, 17, .5)", Evaluate("hue(rgba(136, 17, 17, .5), 45)"));
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

        [Test]
        public void TestMixTestsTypes()
        {
            AssertErrorMessage("Expected color in function 'mix', found \"foo\"", "mix(\"foo\", #f00, 10%)");
            AssertErrorMessage("Expected color in function 'mix', found \"foo\"", "mix(#f00, \"foo\", 10%)");
            AssertErrorMessage("Expected number in function 'mix', found \"foo\"", "mix(#f00, #baf, \"foo\")");
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

        [Test]
        public void TestGrayscaleTestsTypes()
        {
            AssertErrorMessage("Expected color in function 'grayscale', found \"foo\"", "grayscale(\"foo\")");
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

        [Test]
        public void TestComplementTestsTypes()
        {
            AssertErrorMessage("Expected color in function 'complement', found \"foo\"", "complement(\"foo\")");
        }

        private static void AssertErrorMessage(string message, string expression)
        {
            Assert.That(() => Evaluate(expression), Throws.Exception.Message.EqualTo(message));
        }


        private static string Evaluate(string expression)
        {
            var less = ".def { @c: #123456; prop: " + expression + "; }";

            var engine = new ExtensibleEngineImpl(less);

            var css = engine.Css;

            if (string.IsNullOrEmpty(css))
                Assert.Fail("expression '{0}' returned no output.", expression);

            var start = css.IndexOf("prop: ");
            var end = css.LastIndexOf("}");

            return css.Substring(start + 6, end - start - 8);
        }
    }
}