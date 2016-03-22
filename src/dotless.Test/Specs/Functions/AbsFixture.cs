namespace dotless.Test.Specs.Functions
{
    using NUnit.Framework;

    public class AbsFixture : SpecFixtureBase
    {
        [Test]
        public void Abs()
        {
            AssertExpression("5", "abs(-5)");
            AssertExpression("5", "abs(5)");
            AssertExpression("5px", "abs(-5px)");
            AssertExpression("5px", "abs(5px)");
        }

        [Test]
        public void AbsInfo()
        {
            var absInfo = "abs(number) is not supported by less.js, so this will work but not compile with other less implementations.";

            AssertExpressionLogMessage(absInfo, "abs(3)");
        }

        [Test]
        public void ThrowsIfIncorrectType()
        {
            AssertExpressionError("Expected number in function 'abs', found #aaa", 4, "abs(#aaa)");
        }
    }
}