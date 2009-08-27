using NUnit.Framework;

namespace nLess.Test.Spec
{
    [TestFixture]
    public class SpecEngine
    {
        [Test]
        public void Specs()
        {
            SpecHelper.ShouldEqual("accessors");
            SpecHelper.ShouldEqual("big");
            SpecHelper.ShouldEqual("colors");
            SpecHelper.ShouldEqual("comments");
            SpecHelper.ShouldEqual("css-3");
            SpecHelper.ShouldEqual("css");
            SpecHelper.ShouldEqual("functions");
        }
    }
}
