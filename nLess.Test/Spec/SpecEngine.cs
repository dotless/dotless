using NUnit.Framework;

namespace nLess.Test.Spec
{
    [TestFixture]
    public class SpecEngine
    {
        [Test]
        public void Specs()
        {
            SpecHelper.ShouldEqual("accessors"); //PASS
            SpecHelper.ShouldEqual("big"); //FAIL
            SpecHelper.ShouldEqual("colors"); //PASS
            SpecHelper.ShouldEqual("comments");
            SpecHelper.ShouldEqual("css-3"); //PASS
            SpecHelper.ShouldEqual("css"); //FAIL
            SpecHelper.ShouldEqual("functions");
            SpecHelper.ShouldEqual("lazy-eval"); //PASS
            SpecHelper.ShouldEqual("import"); //FAIL
            SpecHelper.ShouldEqual("operations"); //FAIL
        }

        [Test]
        public void Single()
        {
            SpecHelper.ShouldEqual("import"); //FAIL
        }
    }
}
