using NUnit.Framework;
using String=nless.Core.engine.nodes.Literals.String;

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
