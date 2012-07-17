namespace dotless.Test.Specs.Functions
{
    using NUnit.Framework;
    using System;
    using System.Drawing;
    using System.IO;
    using System.Text.RegularExpressions;
    using Core.Exceptions;
    using Core.Parser.Functions;
    using Color = Core.Parser.Tree.Color;
    using DrawingColor = System.Drawing.Color;

    public class GradientFixture : SpecFixtureBase
    {
        private const string CATCH_DATA_IMAGE_PATTERN = @"^url\(data:image\/png;base64,([0-9a-zA-Z/=+]+)\)$";
        private static readonly Regex _catchImageData = new Regex(CATCH_DATA_IMAGE_PATTERN, RegexOptions.Compiled);

        [Test]
        public void TestGradient()
        {
            AssertMatchExpression(CATCH_DATA_IMAGE_PATTERN, "gradient(#f00, #00f)");
        }

        [Test]
        public void TestImageDimensionsAndColors()
        {
            CheckSimpleImage("#f00", "#00f", 5);
            CheckSimpleImage("#f00", "#00f", 100);
        }

        private void CheckSimpleImage(string from, string to, int pos)
        {
            using (var img = EvaluateImage(string.Format("gradient({0}, {1}, {2})", from, to, pos)))
            {
                Assert.AreEqual(1, img.Width);
                Assert.AreEqual(pos + 1, img.Height);
                var fromColor = new Color(from.TrimStart('#'));
                Assert.AreEqual((DrawingColor) fromColor, img.GetPixel(0, 0));
                var toColor = new Color(to.TrimStart('#'));
                Assert.AreEqual((DrawingColor) toColor, img.GetPixel(0, pos));
            }
        }

        [Test]
        public void TestGradientInfo()
        {
            const string info1 = "gradient(color, color[, position]) is not supported by less.js, so this will work but not compile with other less implementations.";

            AssertExpressionLogMessage(info1, "gradient(#f00, #00f)");
        }

        [Test]
        public void TestManyPoints()
        {
            // no position specified - default is used
            using (var img = EvaluateImage("gradient(#f00, #0f0, #00f)"))
                Assert.AreEqual(GradientFunction.DEFAULT_COLOR_OFFSET * 2 + 1, img.Height);

            // no position for 3rd color - previous offset is used
            using (var img = EvaluateImage("gradient(#f00, #0f0, 10, #00f)"))
                Assert.AreEqual(21, img.Height);

            using (var img = EvaluateImage("gradient(#f00, #0f0, 10, #00f, 39)"))
                Assert.AreEqual(40, img.Height);
        }

        [Test]
        [ExpectedException(typeof(ParserException))]
        public void ShouldThrowOnPositionLessThanPrevious()
        {
            EvaluateExpression("gradient(#f00, #0f0, 10, #00f, 9)");
        }

        private Bitmap EvaluateImage(string def)
        {
            var base64 = _catchImageData.Match(EvaluateExpression(def)).Groups[1].Value;
            using (var ms = new MemoryStream(Convert.FromBase64String(base64)))
                return (Bitmap) Image.FromStream(ms);
        }
    }
}