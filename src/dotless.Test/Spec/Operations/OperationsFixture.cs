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
        }
    }
}