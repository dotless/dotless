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

namespace dotless.Test.Unit.engine
{
    using Core.engine;
    using NUnit.Framework;

    [TestFixture]
    public class ElementFixture
    {
        [Test]
        public void CanInstansiateElement()
        {
            var element = new ElementBlock("El");
            Assert.AreEqual(element.Name, "El");
            element = new ElementBlock("El", ">");
            Assert.AreEqual(element.Name, "El");
            Assert.That(element.Selector is Child);
        }
        [Test]
        public void CanAddSubElements()
        {
            var e2 = new ElementBlock("E2");
            var element = new ElementBlock("El");
            element.Add(e2);
            Assert.That(element.Elements.Contains(e2));
        }

        [Test]
        public void CanRetrieveElementPath()
        {
            var e2 = new ElementBlock("E2");
            var e3 = new ElementBlock("E3");
            var element = new ElementBlock("El");
            element.Add(e2);
            e2.Add(e3);
            Assert.That(e3.Path().Contains(element));
        }
    }
}