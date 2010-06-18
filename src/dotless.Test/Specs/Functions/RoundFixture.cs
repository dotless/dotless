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
    }
}