namespace dotless.Test.Specs.Functions
{
    using NUnit.Framework;

    public class UnitFixture : SpecFixtureBase
    {
        [Test]
        public void Unit()
        {
            AssertExpression("5", "unit(5px)");
            AssertExpression("15", "unit(15em)");
            AssertExpression("5", "unit(5rem)");
            AssertExpression("18", "unit(18%)");
            AssertExpression("5px", "unit(5rem, px)");
            AssertExpression("5em", "unit(5, em)");
            AssertExpression("42anything", "unit(42, anything)");
            AssertExpression("36omg", "unit(36, omg)");
            AssertExpression("36'omg'", "unit(36, 'omg')");
            //AssertExpression("5%", "unit(5px, %)"); // FIX '%' is not supported as a parameter
        }

        [Test]
        public void ThrowsIfIncorrectType()
        {
            AssertExpressionError("Expected number in function 'unit', found at", 0, "unit(at, px)");
        }
    }
}
