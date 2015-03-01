namespace dotless.Test.Specs.Functions
{
    using System.Collections.Generic;
    using NUnit.Framework;

    public class ArgbFixture : SpecFixtureBase
    {
        [Test]
        public void TestArgb()
        {
            AssertExpression("#ff123456", "argb(#123456)");
            AssertExpression("#00000000", "argb(transparent)");
            AssertExpression("#80ffffff", "argb(alpha(#ffffff, -50))");
        }
    }
}