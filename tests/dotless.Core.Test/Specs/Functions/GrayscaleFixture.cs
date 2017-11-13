namespace dotless.Core.Test.Specs.Functions
{
    using NUnit.Framework;

    public class GrayscaleFixture : SpecFixtureBase
    {
        [Test]
        public void TestGrayscale()
        {
            AssertExpression("#bbbbbb", "grayscale(#abc)");
            AssertExpression("#808080", "grayscale(#f00)");
            AssertExpression("#808080", "grayscale(#00f)");
            AssertExpression("#ffffff", "grayscale(#fff)");
            AssertExpression("#000000", "grayscale(#000)");
        }

        [Test]
        public void TestGreyscale()
        {
            AssertExpression("#bbbbbb", "greyscale(#abc)");
            AssertExpression("#808080", "greyscale(#f00)");
            AssertExpression("#808080", "greyscale(#00f)");
            AssertExpression("#ffffff", "greyscale(#fff)");
            AssertExpression("#000000", "greyscale(#000)");
        }

        [Test]
        public void TestEditGrayscaleWarning()
        {
            var alphaWarning = "grayscale(color) is not supported by less.js, so this will work but not compile with other less implementations." +
                " You may want to consider using greyscale(color) which does the same thing and is supported.";

            AssertExpressionLogMessage(alphaWarning, "grayscale(#00f)");
            AssertExpressionNoLogMessage(alphaWarning, "greyscale(#00f)");
        }

        [Test]
        public void TestGrayscaleTestsTypes()
        {
            AssertExpressionError("Expected color in function 'grayscale', found \"foo\"", 10, "grayscale(\"foo\")");
        }
    }
}