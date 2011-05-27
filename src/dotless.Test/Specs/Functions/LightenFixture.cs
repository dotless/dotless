namespace dotless.Test.Specs.Functions 
{
    using NUnit.Framework;

    public class LightenFixture : SpecFixtureBase 
    {
        [Test]
        public void TestLighten() 
        {
            AssertExpression("#d6d65c", "lighten(#cc3, 10%)");

            // From less.js test:
            AssertExpression("#ffcccc", "lighten(#ff0000, 40%)");
        }
    }
}