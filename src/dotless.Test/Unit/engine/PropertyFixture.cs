namespace dotless.Test.Unit.engine
{
    using Core.engine;
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class PropertyFixture
    {
        [Test]
        public void CanEvaluateColorProperties()
        {
            var prop = new Property("background-color", new Color(1, 1, 1));
            prop.Add(new Operator("+"));
            prop.Add(new Color(1, 1, 1));
            prop.Add(new Operator("*"));
            prop.Add(new Number(20));
            var newColor = prop.Evaluate();
            Console.WriteLine(newColor.ToString());
            Assert.AreEqual(newColor.GetType(), typeof(Color));
            Console.WriteLine(prop.ToCss());
        }
        [Test]
        public void CanEvaluateExpressionNumberProperties()
        {
            var prop = new Property("height", new Number("px", 1));
            prop.Add(new Operator("+"));
            prop.Add(new Number("px", 2));
            prop.Add(new Operator("*"));
            prop.Add(new Number(20));
            var newNumber = prop.Evaluate();
            Assert.AreEqual(newNumber.GetType(), typeof(Number));
            Console.WriteLine(prop.ToCss());
        }
        [Test]
        public void CanEvaluateSeveralPropertiesWithoutOperators()
        {
            var prop = new Property("border", new Number("px", 1));
            prop.Add(new Number("px", 2));
            prop.Add(new Number("px", 2));
            prop.Add(new Number("px", 2));
            Console.WriteLine(prop.ToCss());
        }
    }
}