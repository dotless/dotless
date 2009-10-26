
namespace dotless.Test.Spec
{
    using NUnit.Framework;

    [TestFixture]
    public class SpecEngine
    {
        [Test]
        public void Specs()
        {
            SpecHelper.ShouldEqual("accessors"); //PASS
            SpecHelper.ShouldEqual("big"); //PASS
            SpecHelper.ShouldEqual("colors"); //PASS
            SpecHelper.ShouldEqual("comments"); //PASS
            SpecHelper.ShouldEqual("css-3"); //PASS
            SpecHelper.ShouldEqual("css"); //PASS
            // SpecHelper.ShouldEqual("functions"); //FAIL
            SpecHelper.ShouldEqual("import"); //PASS
            SpecHelper.ShouldEqual("lazy-eval"); //PASS
            // SpecHelper.ShouldEqual("mixins-args"); //FAIL
            // SpecHelper.ShouldEqual("mixins"); //FAIL
            SpecHelper.ShouldEqual("operations"); //PASS
            SpecHelper.ShouldEqual("rulesets"); //PASS
            SpecHelper.ShouldEqual("scope"); //PASS
            //SpecHelper.ShouldEqual("selectors"); //FAIL
            SpecHelper.ShouldEqual("strings"); //PASS
            SpecHelper.ShouldEqual("variables"); //PASS
            // SpecHelper.ShouldEqual("whitespace"); //FAIL
        }
    }
}