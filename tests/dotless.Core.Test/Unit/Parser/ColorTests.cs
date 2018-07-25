namespace dotless.Core.Test.Unit.Parser
{
    using NUnit.Framework;
    using Color = Core.Parser.Tree.Color;

    public class ColorTests
    {
        [Test]
        public void Color_ConstructedFromHexString_RgbValuesDoNotThrow()
        {
            var color = new Color("#ff0000");
            Assert.AreEqual(255, color.R);
            Assert.AreEqual(0, color.G);
            Assert.AreEqual(0, color.B);
        }
    }
}