namespace dotless.Core.Test.Specs.Functions
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
        public void TestEditSaturationInfo()
        {
            var info1 = "saturation(color, number) is not supported by less.js, so this will work but not compile with other less implementations." +
                @" You may want to consider using saturate(color, number) or its opposite desaturate(color, number), which does the same thing and is supported.";

            AssertExpressionLogMessage(info1, "saturation(blue, 23)");
            AssertExpressionNoLogMessage(info1, "saturation(blue)");
        }

        [Test]
        public void TestEditSaturation()
        {
            //Saturate
            AssertExpression("#d9f2d9", "saturation(hsl(120, 30, 90), 20%)");
            AssertExpression("#9e3f3f", "saturation(#855, 20%)");
            AssertExpression("#000000", "saturation(#000, 20%)");
            AssertExpression("#ffffff", "saturation(#fff, 20%)");
            AssertExpression("#33ff33", "saturation(#8a8, 100%)");
            AssertExpression("#88aa88", "saturation(#8a8, 0%)");
            AssertExpression("rgba(158, 63, 63, 0.5)", "saturation(rgba(136, 85, 85, 0.5), 20%)");

            // Desaturate
            AssertExpression("#e3e8e3", "saturation(hsl(120, 30, 90), -20%)");
            AssertExpression("#726b6b", "saturation(#855, -20%)");
            AssertExpression("#000000", "saturation(#000, -20%)");
            AssertExpression("#ffffff", "saturation(#fff, -20%)");
            AssertExpression("#999999", "saturation(#8a8, -100%)");
            AssertExpression("#88aa88", "saturation(#8a8, 0%)");
            AssertExpression("rgba(114, 107, 107, 0.5)", "saturation(rgba(136, 85, 85, .5), -20%)");
        }

        [Test]
        public void TestEditSaturation2()
        {
            //Saturate
            AssertExpression("#d9f2d9", "saturate(hsl(120, 30, 90), 20%)");
            AssertExpression("#9e3f3f", "saturate(#855, 20%)");
            AssertExpression("#000000", "saturate(#000, 20%)");
            AssertExpression("#ffffff", "saturate(#fff, 20%)");
            AssertExpression("#33ff33", "saturate(#8a8, 100%)");
            AssertExpression("#88aa88", "saturate(#8a8, 0%)");
            AssertExpression("rgba(158, 63, 63, 0.5)", "saturate(rgba(136, 85, 85, 0.5), 20%)");

            // Desaturate
            AssertExpression("#e3e8e3", "desaturate(hsl(120, 30, 90), 20%)");
            AssertExpression("#726b6b", "desaturate(#855, 20%)");
            AssertExpression("#000000", "desaturate(#000, 20%)");
            AssertExpression("#ffffff", "desaturate(#fff, 20%)");
            AssertExpression("#999999", "desaturate(#8a8, 100%)");
            AssertExpression("#88aa88", "desaturate(#8a8, 0%)");
            AssertExpression("rgba(114, 107, 107, 0.5)", "desaturate(rgba(136, 85, 85, .5), 20%)");
        }

        [Test]
        public void TestEditSaturationOverflow()
        {
            AssertExpression("#33ff33", "saturation(#8a8, 101%)");
            AssertExpression("#999999", "saturation(#8a8, -101%)");
        }

        [Test]
        public void TestEditSaturationOverflow2()
        {
            AssertExpression("#33ff33", "saturate(#8a8, 101%)");
            AssertExpression("#999999", "desaturate(#8a8, 101%)");
        }

        [Test]
        public void TestEditSaturationTestsTypes()
        {
            AssertExpressionError("Expected color in function 'saturation', found \"foo\"", 11, "saturation(\"foo\", 10%)");
            AssertExpressionError("Expected number in function 'saturation', found \"foo\"", 17, "saturation(#fff, \"foo\")");
        }

        [Test]
        public void TestEditSaturationTestsTypes2()
        {
            AssertExpressionError("Expected color in function 'saturate', found \"foo\"", 9, "saturate(\"foo\", 10%)");
            AssertExpressionError("Expected number in function 'saturate', found \"foo\"", 15, "saturate(#fff, \"foo\")");
            AssertExpressionError("Expected color in function 'desaturate', found \"foo\"", 11, "desaturate(\"foo\", 10%)");
            AssertExpressionError("Expected number in function 'desaturate', found \"foo\"", 17, "desaturate(#fff, \"foo\")");
        }
    }
}