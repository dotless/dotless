namespace dotless.Core.Test.Specs.Functions
{
    using NUnit.Framework;

    public class BlueFixture : SpecFixtureBase
    {
        [Test]
        public void TestBlue()
        {
            AssertExpression("86", "blue(#123456)");
        }

        [Test]
        public void TestBlueException()
        {
            AssertExpressionError("Expected color in function 'blue', found 12", 5, "blue(12)");
        }

        [Test]
        public void TestEditBlue()
        {
            AssertExpression("#123460", "blue(#123456, 10)");
        }

        [Test]
        public void TestEditBlueTestsTypes()
        {
            AssertExpressionError("Expected color in function 'blue', found 12", 5, "blue(12)");
        }

        [Test]
        public void TestBlueInfo()
        {
            var blueInfo1 = "blue(color, number) is not supported by less.js, so this will work but not compile with other less implementations.";

            AssertExpressionLogMessage(blueInfo1, "blue(#123456, 10)");
        }
    }
}