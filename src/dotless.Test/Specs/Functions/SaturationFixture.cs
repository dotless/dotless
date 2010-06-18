namespace dotless.Test.Specs.Functions
{
    using NUnit.Framework;

    public class SaturationFixture : SpecFixtureBase
    {
        [Test]
        public void TestSaturation()
        {
            AssertExpression("52%", "saturation(hsl(20, 52%, 20%))");
            AssertExpression("52%", "saturation(hsl(20, 52, 20%))");
        }

        [Test]
        public void TestSaturationException()
        {
            AssertExpressionError("Expected color in function 'saturation', found 12", 11, "saturation(12)");
        }

        [Test]
        public void TestEditSaturation()
        {
            //Saturate
            AssertExpression("#d9f2d9", "saturation(hsl(120, 30, 90), 20%)");
            AssertExpression("#9e3f3f", "saturation(#855, 20%)");
            AssertExpression("black", "saturation(#000, 20%)");
            AssertExpression("white", "saturation(#fff, 20%)");
            AssertExpression("#33ff33", "saturation(#8a8, 100%)");
            AssertExpression("#88aa88", "saturation(#8a8, 0%)");
            AssertExpression("rgba(158, 63, 63, 0.5)", "saturation(rgba(136, 85, 85, 0.5), 20%)");

            // Desaturate
            AssertExpression("#e3e8e3", "saturation(hsl(120, 30, 90), -20%)");
            AssertExpression("#726b6b", "saturation(#855, -20%)");
            AssertExpression("black", "saturation(#000, -20%)");
            AssertExpression("white", "saturation(#fff, -20%)");
            AssertExpression("#999999", "saturation(#8a8, -100%)");
            AssertExpression("#88aa88", "saturation(#8a8, 0%)");
            AssertExpression("rgba(114, 107, 107, 0.5)", "saturation(rgba(136, 85, 85, .5), -20%)");
        }

        [Test]
        public void TestEditSaturationOverflow()
        {
            AssertExpression("#33ff33", "saturation(#8a8, 101%)");
            AssertExpression("#999999", "saturation(#8a8, -101%)");
        }

        [Test]
        public void TestEditSaturationTestsTypes()
        {
            AssertExpressionError("Expected color in function 'saturation', found \"foo\"", 11, "saturation(\"foo\", 10%)");
            AssertExpressionError("Expected number in function 'saturation', found \"foo\"", 17, "saturation(#fff, \"foo\")");
        }
    }
}