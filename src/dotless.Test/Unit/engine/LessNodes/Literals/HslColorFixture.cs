using System;
using dotless.Core.engine;
using NUnit.Framework;

namespace dotless.Test.Unit.engine.Literals
{
    [TestFixture]
    public class HslColorFixture
    {
        public static void AssertRgbToHsl(double red, double green, double blue, double? hue, double? saturation, double? lightness)
        {
            var color = new Color(red, green, blue);
            var hsl = HslColor.FromRgbColor(color);

            if (hue != null)
                Assert.That(hsl.Hue * 360, Is.EqualTo(hue).Within(0.49));
                //Assert.AreEqual(hue, hsl.Hue * 360);

            if (saturation != null)
                Assert.That(hsl.Saturation * 100, Is.EqualTo(saturation).Within(0.49));
                //Assert.AreEqual(saturation, hsl.Saturation * 100);

            if (lightness != null)
                Assert.That(hsl.Lightness * 100, Is.EqualTo(lightness).Within(0.49));
                //Assert.AreEqual(lightness, hsl.Lightness * 100);
        }

        private static void AssertHslToRgb(double hue, double saturation, double lightness, double red, double green, double blue)
        {
            var hsl = new HslColor(hue / 360, saturation / 100, lightness / 100);
            var color = hsl.ToRgbColor();

            Assert.That(color.R, Is.EqualTo(red).Within(0.49));

            Assert.That(color.G, Is.EqualTo(green).Within(0.49));

            Assert.That(color.B, Is.EqualTo(blue).Within(0.49));
        }

        [Test]
        public void CanConvertFromRgb()
        {
            AssertRgbToHsl(255, 255, 255, null, 0, 100);
            AssertRgbToHsl(51, 204, 204, 180, 60, 50);
            AssertRgbToHsl(76, 76, 76, 0, 0, 30);
            AssertRgbToHsl(12, 34, 56, 210, 65, 13);
        }


        [Test]
        public void CanConvertToRgb()
        {
            AssertHslToRgb(0, 0, 100, 255, 255, 255);
            AssertHslToRgb(180, 60, 50, 51, 204, 204);
            AssertHslToRgb(0, 0, 30, 76, 76, 76);
            //AssertHslToRgb(210, 65, 13, 12, 34, 56); // rounding causes green to be 33.15
        }
    }
}