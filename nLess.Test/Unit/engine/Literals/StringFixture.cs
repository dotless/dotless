namespace dotless.Test.Unit.engine.Literals
{
    using Core.engine;
    using NUnit.Framework;

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