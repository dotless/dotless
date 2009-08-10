using nless.Core.utils;
using NUnit.Framework;

namespace nLess.Test.Unit.utils
{
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
