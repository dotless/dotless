namespace dotless.Core.Test.Specs.Functions
{
    using NUnit.Framework;

    public class RedFixture : SpecFixtureBase
    {
        [Test]
        public void TestRedInfo()
        {
            var redInfo1 = "red(color, number) is not supported by less.js, so this will work but not compile with other less implementations.";

            AssertExpressionLogMessage(redInfo1, "red(#123456, 10)");
        }

        [Test]
        public void TestRed()
        {
            AssertExpression("18", "red(#123456)");
        }

        [Test]
        public void TestRedException()
        {
            AssertExpressionError("Expected color in function 'red', found 12", 4, "red(12)");
        }

        [Test]
        public void TestEditRed()
        {
            AssertExpression("#1c3456", "red(#123456, 10)");
        }

        [Test]
        public void TestEditRedTestsTypes()
        {
            AssertExpressionError("Expected color in function 'red', found 12", 4, "red(12)");
        }
    }
}