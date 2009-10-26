using NUnit.Framework;

namespace nLess.Test.Unit.utils
{
    using dotless.Core.utils;

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
