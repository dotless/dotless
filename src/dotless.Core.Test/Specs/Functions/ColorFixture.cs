namespace dotless.Core.Test.Specs.Functions
{
    using NUnit.Framework;
    using Color = Core.Parser.Tree.Color;
    using DrawingColor = System.Drawing.Color;

    public class ColorFixture : SpecFixtureBase
    {
        [Test]
        public void TestColor()
        {
            AssertExpression("#ff0000", @"color(""#ff0000"")");
            AssertExpression("#0ff", @"color(""#0ff"")");
        }

        [Test]
        public void TestToDrawingColor()
        {
            Assert.That((DrawingColor) new Color(255d, 0d, 0d), Is.EqualTo(DrawingColor.FromArgb(255, 0, 0)));
            Assert.That((DrawingColor) new Color(0d, 255d, 0d), Is.EqualTo(DrawingColor.FromArgb(0, 255, 0)));
            Assert.That((DrawingColor) new Color(0d, 0d, 255d), Is.EqualTo(DrawingColor.FromArgb(0, 0, 255)));
            Assert.That((DrawingColor) new Color(0d, 0d, 255d, 0.5d), Is.EqualTo(DrawingColor.FromArgb(128, 0, 0, 255)));
        }

        [Test]
        public void AcceptsColorKeywords() {
            AssertExpression("red", "color(\"red\")");
            AssertExpression("green", "color(\"green\")");
            AssertExpression("blue", "color(\"blue\")");
            AssertExpression("black", "color(\"black\")");
            AssertExpression("white", "color(\"white\")");
        }
    }
}