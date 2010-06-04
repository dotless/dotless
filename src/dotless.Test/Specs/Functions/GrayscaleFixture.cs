namespace dotless.Test.Specs.Functions
{
    using NUnit.Framework;

    public class GrayscaleFixture : SpecFixtureBase
    {
        [Test]
        public void TestGrayscale()
        {
            AssertExpression("#bbbbbb", "grayscale(#abc)");
            AssertExpression("gray", "grayscale(#f00)");
            AssertExpression("gray", "grayscale(#00f)");
            AssertExpression("white", "grayscale(#fff)");
            AssertExpression("black", "grayscale(#000)");
        }

        [Test]
        public void TestGrayscaleTestsTypes()
        {
            AssertExpressionError("Expected color in function 'grayscale', found \"foo\"", "grayscale(\"foo\")");
        }
    }
}