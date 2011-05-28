namespace dotless.Test.Specs.Functions 
{
    using NUnit.Framework;

    public class DesaturateFixture : SpecFixtureBase 
    {
        [Test]
        public void TestDesaturate() 
        {
            // Values from http://sass-lang.com/docs/yardoc/Sass/Script/Functions.html#desaturate-instance_method
            AssertExpression("#726b6b", "desaturate(#855, 20%)");

            // From less.js tests:
            AssertExpression("#29332f", "desaturate(#203c31, 20%)");
        }
    }
}