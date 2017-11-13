namespace dotless.Core.Test.Specs.Functions
{
    using NUnit.Framework;
    using System;
    using System.Drawing;
    using System.IO;
    using System.Text.RegularExpressions;
    using Core.Exceptions;
    using Core.Parser.Functions;
    using System.Collections;
    using System.Reflection;
    using Color = Core.Parser.Tree.Color;
    using DrawingColor = System.Drawing.Color;

    public class GradientImageFixture : SpecFixtureBase
    {
        private const string CATCH_DATA_IMAGE_PATTERN = @"^url\(data:image\/png;base64,([0-9a-zA-Z/=+]+)\)$";
        private static readonly Regex _catchImageData = new Regex(CATCH_DATA_IMAGE_PATTERN, RegexOptions.Compiled);

        [Test]
        public void TestGradientImage()
        {
            AssertMatchExpression(CATCH_DATA_IMAGE_PATTERN, "gradientImage(#f00, #00f)");
        }

        [Test]
        public void TestImageDimensionsAndColors()
        {
            CheckSimpleImage("#f00", "#00f", 5);
            CheckSimpleImage("#f00", "#00f", 100);
        }

        private void CheckSimpleImage(string from, string to, int pos)
        {
            using (var img = EvaluateImage(string.Format("gradientImage({0}, {1}, {2})", from, to, pos)))
            {
                Assert.AreEqual(1, img.Width);
                Assert.AreEqual(pos + 1, img.Height);
                var fromColor = Color.From(from);
                Assert.AreEqual((DrawingColor)fromColor, img.GetPixel(0, 0));
                var toColor = Color.From(to);
                Assert.AreEqual((DrawingColor)toColor, img.GetPixel(0, pos));
            }
        }

        [Test]
        public void TestGradientInfo()
        {
            const string info1 = "gradientImage(color, color[, position]) is not supported by less.js, so this will work but not compile with other less implementations.";

            AssertExpressionLogMessage(info1, "gradientImage(#f00, #00f)");
        }

        [Test]
        public void TestManyPoints()
        {
            // no position specified - default is used
            using (var img = EvaluateImage("gradientImage(#f00, #0f0, #00f)"))
                Assert.AreEqual(GradientImageFunction.DEFAULT_COLOR_OFFSET * 2 + 1, img.Height);

            // no position for 3rd color - previous offset is used
            using (var img = EvaluateImage("gradientImage(#f00, #0f0, 10, #00f)"))
                Assert.AreEqual(21, img.Height);

            using (var img = EvaluateImage("gradientImage(#f00, #0f0, 10, #00f, 39)"))
                Assert.AreEqual(40, img.Height);
        }

        [Test]
        public void ShouldThrowOnPositionLessThanPrevious()
        {
            Assert.Throws<ParserException>(() => EvaluateExpression("gradientImage(#f00, #0f0, 10, #00f, 9)"));
        }

        [Test]
        public void ShouldCacheResult()
        {
            var cacheList = GetCacheList();
            cacheList.Clear();
            EvaluateExpression("gradientImage(#ff0000, #00ff00, 10, #0000ff)");
            EvaluateExpression("gradientImage(#ff0000, #00ff00, 10, #000ff0)");
            Assert.That(cacheList, Has.Count.EqualTo(2));
        }

        [Test]
        public void ShouldCacheTheSameForEquivalentDeclarations()
        {
            var cacheList = GetCacheList();
            cacheList.Clear();
            EvaluateExpression("gradientImage(#f00,#0f0)");
            EvaluateExpression("gradientImage(#ff0000, #00ff00, " + GradientImageFunction.DEFAULT_COLOR_OFFSET + ")");
            Assert.That(cacheList, Has.Count.EqualTo(1));
        }

        private static IList GetCacheList()
        {
            var field = typeof(GradientImageFunction).GetField("_cache", BindingFlags.Static | BindingFlags.NonPublic);
            Assert.That(field, Is.Not.Null, "GradientFunction._cache");
            return (IList)field.GetValue(null);
        }

        private Bitmap EvaluateImage(string def)
        {
            var base64 = _catchImageData.Match(EvaluateExpression(def)).Groups[1].Value;
            using (var ms = new MemoryStream(Convert.FromBase64String(base64)))
                return (Bitmap)Image.FromStream(ms);
        }
    }
}