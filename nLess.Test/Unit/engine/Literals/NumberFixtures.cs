namespace dotless.Test.Unit.engine.Literals
{
    using Core.engine;
    using NUnit.Framework;

    [TestFixture]
    public class NumberFixture
    {
        [Test]
        public void CanOperateOnNumber()
        {
            var number = new Number("%", 100);
            number += 100;
            Assert.AreEqual("200%", number.ToCss());
        }
    }
}