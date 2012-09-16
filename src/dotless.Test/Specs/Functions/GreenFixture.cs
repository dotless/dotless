namespace dotless.Test.Specs.Functions
{
    using NUnit.Framework;

    public class GreenFixture : SpecFixtureBase
    {
        [Test]
        public void TestGreenInfo()
        {
            var greenInfo1 = "green(color, number) is not supported by less.js, so this will work but not compile with other less implementations.";

            AssertExpressionLogMessage(greenInfo1, "green(#123456, 10)");
        }

        [Test]
        public void TestGreen()
        {
            AssertExpression("52", "green(#123456)");
        }

        [Test]
        public void TestGreenException()
        {
            AssertExpressionError("Expected color in function 'green', found 12", 6, "green(12)");
        }

        [Test]
        public void TestEditGreen()
        {
            AssertExpression("#123e56", "green(#123456, 10)");
        }

        [Test]
        public void TestEditGreenTestsTypes()
        {
            AssertExpressionError("Expected color in function 'green', found 12", 6, "green(12)");
        }
    }
}