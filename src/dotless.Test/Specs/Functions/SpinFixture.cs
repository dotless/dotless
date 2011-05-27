namespace dotless.Test.Specs.Functions 
{
    using NUnit.Framework;

    public class SpinFixture : SpecFixtureBase 
    {
        [Test]
        public void TestSpin() 
        {
            // From less.js tests at
            // https://github.com/cloudhead/less.js/blob/master/test/less/functions.less
            // and https://github.com/cloudhead/less.js/blob/master/test/css/functions.css
            AssertExpression("#bf6a40", "spin(hsl(340, 50%, 50%), 40)");
            AssertExpression("#bf4055", "spin(hsl(30, 50%, 50%), -40)");
        }
    }
}