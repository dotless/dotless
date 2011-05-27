namespace dotless.Test.Specs.Functions 
{
    using NUnit.Framework;

    public class SaturateFixture : SpecFixtureBase 
    {
        [Test]
        public void TestSaturate() 
        {
            // Values from http://sass-lang.com/docs/yardoc/Sass/Script/Functions.html#saturate-instance_method
            AssertExpression("#9e3f3f", "saturate(#855, 20%)");

            // From less.js tests:
            AssertExpression("#203c31", "saturate(#29332f, 20%)");
        }
    }
}