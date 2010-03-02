/* Copyright 2009 dotless project, http://www.dotlesscss.com
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 *     
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License. */

using System.Collections.Generic;
using dotless.Core.engine;
using dotless.Core.engine.LessNodes;
using dotless.Core.engine.Pipeline;
using NUnit.Framework;

namespace dotless.Test.Unit.engine.Pipeline
{
    [TestFixture]
    public class LessToCssDomConverterFixture
    {
        readonly LessToCssDomConverter converter = new LessToCssDomConverter();

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

        private static ElementBlock GetElementBlock()
        {
            var elementShouldBeRendered = new ElementBlock(".showme");
            elementShouldBeRendered.Add(new Property("height", new Number("px", 1)));
            elementShouldBeRendered.Add(new Property("width", new Number("px", 1)));
            return elementShouldBeRendered;
        }
    }
}
