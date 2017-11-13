namespace dotless.Core.Test.Specs.Functions
{
    using NUnit.Framework;
    
    public class PowFixture : SpecFixtureBase
    {
        [Test]
        public void Abs()
        {
            AssertExpression("8", "pow(2, 3)");
            AssertExpression("0.25", "pow(2, -2)");
        }

        [Test]
        public void AbsInfo()
        {
            var absInfo = "pow(number, number) is not supported by less.js, so this will work but not compile with other less implementations.";

            AssertExpressionLogMessage(absInfo, "pow(3, 5)");
        }
    }
}
