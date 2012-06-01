namespace dotless.Test.Specs
{
    using NUnit.Framework;

    public class ColorsFixture : SpecFixtureBase
    {
        [Test]
        [Ignore]
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
            AssertExpression("#ffeeaa", "#fea");
            AssertExpression("#ffeeaa", "#ffeeaa");
            AssertExpression("blue", "#00f");
            AssertExpression("blue", "#0000ff");
        }

        [Test]
        public void Transparent()
        {
            //Test that transparent can be used as a name colour input
            AssertExpression("rgba(255, 255, 255, 0.5)", "mix(white, transparent)");

            // transparent has better browser support and is shorter, so until we record the original format of colours
            // and treat that as significant, we should do the below conversion
            AssertExpression("transparent", "rgba(0, 0, 0, 0)");

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
            AssertExpression("blue", "rgba(0, 0, 255, 1)");
        }

        [Test]
        public void Overflow()
        {
            AssertExpression("black", "#111111 - #444444");
            AssertExpression("white", "#eee + #fff");
            AssertExpression("white", "#aaa * 3");
            AssertExpression("lime", "#00ee00 + #009900");
        }

        [Test]
        public void Gray()
        {
            AssertExpression("#c8c8c8", "rgb(200, 200, 200)");
            AssertExpression("gray", "hsl(50, 0, 50)");
            AssertExpression("gray", "hsl(50, 0%, 50%)");
        }

        [Test]
        public void Green()
        {
            AssertExpression("lime", "hsl(120, 100%, 50%)");
        }
    }
}