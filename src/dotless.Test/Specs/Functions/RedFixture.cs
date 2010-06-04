namespace dotless.Test.Specs.Functions
{
    using NUnit.Framework;

    public class RedFixture : SpecFixtureBase
    {
        [Test]
        public void TestRed()
        {
            AssertExpression("18", "red(#123456)");
        }

        [Test]
        public void TestRedException()
        {
            AssertExpressionError("Expected color in function 'red', found 12", "red(12)");
        }

        [Test]
        public void TestEditRed()
        {
            AssertExpression("#1c3456", "red(#123456, 10)");
        }

        [Test]
        public void TestEditRedTestsTypes()
        {
            AssertExpressionError("Expected color in function 'red', found 12", "red(12)");
        }
    }
}