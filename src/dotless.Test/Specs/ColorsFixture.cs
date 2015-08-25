namespace dotless.Test.Specs
{
    using NUnit.Framework;

    public class ColorsFixture : SpecFixtureBase
    {
        [Test]
        public void ColorLiteralsUnchanged()
        {
            AssertExpressionUnchanged("#fea");
            AssertExpressionUnchanged("#ffeeaa");
            AssertExpressionUnchanged("#00f");
            AssertExpressionUnchanged("#0000ff");
        }

        [Test]
        public void ColorLiterals()
        {
            AssertExpression("#fea", "#fea");
            AssertExpression("#ffeeaa", "#ffeeaa");
            AssertExpression("blue", "blue");
        }

        [Test]
        public void Transparent()
        {
            //Test that transparent can be used as a name colour input
            AssertExpression("rgba(255, 255, 255, 0.5)", "mix(white, transparent)");

            AssertExpressionUnchanged("rgba(0, 0, 0, 0)");

            AssertExpressionUnchanged("transparent url('file.gif') 32px 1px no-repeat");
        }

        [Test]
        public void RgbaNonOpaqueColorsUnchanged()
        {
            AssertExpressionUnchanged("rgba(255, 238, 170, 0.1)");
            AssertExpressionUnchanged("rgba(0, 0, 255, 0.1)");
        }

        [Test]
        public void RgbaOpaqueColors()
        {
            AssertExpression("#ffeeaa", "rgba(255, 238, 170, 1)");
            AssertExpression("#0000ff", "rgba(0, 0, 255, 1)");
        }

        [Test]
        public void Overflow()
        {
            AssertExpression("#000000", "#111111 - #444444");
            AssertExpression("#ffffff", "#eee + #fff");
            AssertExpression("#ffffff", "#aaa * 3");
            AssertExpression("#00ff00", "#00ee00 + #009900");
        }

        [Test]
        public void Gray()
        {
            AssertExpression("#c8c8c8", "rgb(200, 200, 200)");
            AssertExpression("#808080", "hsl(50, 0, 50)");
            AssertExpression("#808080", "hsl(50, 0%, 50%)");
        }

        [Test]
        public void Green()
        {
            AssertExpression("#00ff00", "hsl(120, 100%, 50%)");
        }
    }
}