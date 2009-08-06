using System;
using nless.Core.engine;
using NUnit.Framework;

namespace nLess.Test.Unit.engine
{
    [TestFixture]
    public class PropertyFixture
    {
        [Test]
        public void CanEvaluateSingleProperty()
        {
            var prop = new Property("background-color", new Color(1,1,1));
            var newColor = prop.Evaluate();
            Assert.AreEqual(newColor.GetType(), typeof(Color));
        }

        [Test]
        public void CanEvaluatePropertyWithExpression()
        {
            var prop = new Property("background-color", new Color(1, 1, 1));
            prop.Add(new Operator("+"));
            prop.Add(new Color(1, 1, 1));
            var newColor = prop.Evaluate();
            Assert.AreEqual(newColor.GetType(), typeof(Color));
            Console.WriteLine(prop.ToCss());
        }

        [Test]
        public void CanEvaluatePropertyWithTwoExpressions()
        {
            var prop = new Property("background-color", new Color(1, 1, 1));
            prop.Add(new Operator("+"));
            prop.Add(new Color(1, 1, 1));
            prop.Add(new Operator("*"));
            prop.Add(new Number(20));
            var newColor = prop.Evaluate();
            Assert.AreEqual(newColor.GetType(), typeof(Color));
            Console.WriteLine(prop.ToCss());
        }

        [Test]
        public void CanRetrieveNodeFromExpressionWhenNumberReturned()
        {
            var prop = new Property("height", new Number("px", 1));
            prop.Add(new Operator("+"));
            prop.Add(new Number(2));
            prop.Add(new Operator("*"));
            prop.Add(new Number(20));
            var newNumber = prop.Evaluate();
            Assert.AreEqual(newNumber.GetType(), typeof(Number));
            Console.WriteLine(prop.ToCss());
        }
    }
}