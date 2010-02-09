using NUnit.Framework;

namespace dotless.Test.Spec.Operations
{
    [TestFixture]
    public class OperationsFixture : SpecFixtureBase
    {
        [Test, Ignore]
        public void CanMultiplyPixelsByPercent()
        {
            AssertExpression("10px", "20px * 50%");
            AssertExpression("10px", "20% * 50px");
        }
        [Test]
        public void DividingPixelsByPixelsHasNoUnits()
        {
            AssertExpression("2", "100px / 50px");
        }
    }
}