using nless.Core.engine;
using NUnit.Framework;

namespace nLess.Test.Unit.engine.Literals
{
    [TestFixture]
    public class StringFixture
    {
        [Test]
        public void CanRetrieveStringContent()
        {
           // var str = new String("Trial");
           // Assert.AreEqual("Trial", str.Content);
            var str = new String("'Trial'");
            Assert.AreEqual("Trial", str.Content);
        }

        
    }
}
