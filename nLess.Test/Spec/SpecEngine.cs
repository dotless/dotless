using NUnit.Framework;

namespace nLess.Test.Spec
{
    internal static class SpecExtensions
    {
        public static bool ShouldEqual(this object a, object b)
        {
            return a.Equals(b);
        }
    }

    [TestFixture]
    public class SpecEngine
    {
        [Test]
        public void Css()
        {
            Assert.IsTrue(SpecHelper.ShouldEqual("css"));
        }
    }
}
