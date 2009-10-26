namespace dotless.Test.Unit.utils
{
    using Core.utils;
    using NUnit.Framework;

    [TestFixture]
    public class RegexExtentionsFixtures
    {
        [Test]
        public void CanRetrieveIdents()
        {
            Assert.IsFalse("abcd".IsIdent());
            Assert.IsTrue(".abcd".IsIdent());
            Assert.IsTrue("#abcd".IsIdent());
        }
    }
}