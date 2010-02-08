using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dotless.Core.engine;
using dotless.Core.engine.LessNodes;
using dotless.Core.engine.Pipeline;
using NUnit.Framework;

namespace dotless.Test.Unit.engine.Pipeline
{
    [TestFixture]
    public class LessToCssDomConverterFixture
    {
        LessToCssDomConverter converter = new LessToCssDomConverter();

        [Test]
        public void ShouldConvertElementBlocksInLessDomToCssDom()
        {
            var root = new ElementBlock("*");
            root.Add(GetElementBlock());
            var cssDocument = converter.BuildCssDocument(root);
            Assert.That(cssDocument.Elements.Count, Is.EqualTo(1));
        }


        [Test]
        public void ShouldNotConvertElementBlocksInLessDomToCssDomIfInNestedFalseIfBlock()
        {
            var root = new ElementBlock("*");
            var ifBlock = new IfBlock(new BoolExpression(new List<INode> {new Bool(false)}));
            root.Add(ifBlock);
            ifBlock.Add(GetElementBlock());
            var cssDocument = converter.BuildCssDocument(root);
            Assert.That(cssDocument.Elements.Count, Is.EqualTo(0));
        }

        [Test]
        public void ShouldConvertElementBlocksInLessDomToCssDomIfInNestedTrueIfBlock()
        {
            var root = new ElementBlock("*");
            var ifBlock = new IfBlock(new BoolExpression(new List<INode> { new Bool(true) }));
            root.Add(ifBlock);
            ifBlock.Add(GetElementBlock());
            var cssDocument = converter.BuildCssDocument(root);
            Assert.That(cssDocument.Elements.Count, Is.EqualTo(1));
        }

        private ElementBlock GetElementBlock()
        {
            var elementShouldBeRendered = new ElementBlock(".showme");
            elementShouldBeRendered.Add(new Property("height", new Number("px", 1)));
            elementShouldBeRendered.Add(new Property("width", new Number("px", 1)));
            return elementShouldBeRendered;
        }
    }
}
