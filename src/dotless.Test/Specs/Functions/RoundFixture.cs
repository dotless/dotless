namespace dotless.Test.Specs.Functions
{
    using NUnit.Framework;

    public class RoundFixture : SpecFixtureBase
    {
        [Test]
        public void TestRound()
        {
            AssertExpression("4", "round(4)");
            AssertExpression("5", "round(4.8)");
            AssertExpression("5px", "round(4.8px)");
            AssertExpression("5px", "round(5.49px)");
            AssertExpression("50%", "round(50.1%)");

            AssertExpressionError("Expected number in function 'round', found #cccccc", 6, "round(#ccc)");
        }

        [Test]
        public void RoundsMidpointAwayFromZero()
        {
            AssertExpression("5", "round(4.5)");
            AssertExpression("6", "round(5.5)");
            AssertExpression("7", "round(6.5)");
            AssertExpression("8", "round(7.5)");
        }
    }
}