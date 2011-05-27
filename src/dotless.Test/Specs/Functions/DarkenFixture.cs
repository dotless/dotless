namespace dotless.Test.Specs.Functions 
{
    using NUnit.Framework;

    public class DarkenFixture : SpecFixtureBase 
    {
        [Test]
        public void TestDarken() 
        {
            AssertExpression("#cccc33", "darken(#d6d65c, 10%)");

            // From less.js tests:
            AssertExpression("#330000", "darken(#ff0000, 40%)");
        }
    }
}