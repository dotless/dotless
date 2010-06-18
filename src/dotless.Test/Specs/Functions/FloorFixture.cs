namespace dotless.Test.Specs.Functions
{
    using NUnit.Framework;

    public class FloorFixture : SpecFixtureBase
    {
        [Test]
        public void TestFloor()
        {
            AssertExpression("4", "floor(4.8)");
            AssertExpression("4px", "floor(4.8px)");
            AssertExpression("5px", "floor(5.49px)");
            AssertExpression("50%", "floor(50.1%)");

            AssertExpressionError("Expected number in function 'floor', found \"foo\"", 6, "floor(\"foo\")");
        }
    }
}