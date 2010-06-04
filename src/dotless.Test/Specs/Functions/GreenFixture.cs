namespace dotless.Test.Specs.Functions
{
    using NUnit.Framework;

    public class GreenFixture : SpecFixtureBase
    {
        [Test]
        public void TestGreen()
        {
            AssertExpression("52", "green(#123456)");
        }

        [Test]
        public void TestGreenException()
        {
            AssertExpressionError("Expected color in function 'green', found 12", "green(12)");
        }

        [Test]
        public void TestEditGreen()
        {
            AssertExpression("#123e56", "green(#123456, 10)");
        }

        [Test]
        public void TestEditGreenTestsTypes()
        {
            AssertExpressionError("Expected color in function 'green', found 12", "green(12)");
        }
    }
}