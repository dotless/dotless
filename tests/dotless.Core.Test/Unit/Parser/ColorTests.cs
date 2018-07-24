namespace dotless.Core.Test.Specs.Functions
{
    using NUnit.Framework;
    using Color = Core.Parser.Tree.Color;

    public class ColorTests
    {
        [Test]
        public void TestColor()
        {
            var color = new Color("#ff0000");
            Assert.AreEqual(255, color.R);
            Assert.AreEqual(0, color.G);
            Assert.AreEqual(0, color.B);
        }
    }
}