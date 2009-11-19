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
    using System;
    using NUnit.Framework;
    using System.Collections.Generic;

    [TestFixture]
    public class ElementFixture
    {
        [Test]
        public void CanInstansiateElement()
        {
            var element = new Element("El");
            Assert.AreEqual(element.Name, "El");
            element = new Element("El", ">");
            Assert.AreEqual(element.Name, "El");
            Assert.That(element.Selector is Child);
        }
        [Test]
        public void CanAddSubElements()
        {
            var e2 = new Element("E2");
            var element = new Element("El");
            element.Add(e2);
            Assert.That(element.Elements.Contains(e2));
        }

        [Test]
        public void CanRetrieveElementPath()
        {
            var e2 = new Element("E2");
            var e3 = new Element("E3");
            var element = new Element("El");
            element.Add(e2);
            e2.Add(e3);
            Assert.That(e3.Path().Contains(element));
        }

        [Test]
        public void CanRetrieveNearestElement()
        {
            var root = new Element();
            var e2 = new Element("E2", ">");
            var e3 = new Element("#yahoo");
            var e1 = new Element(".hello");
            var e1_sibling = new Element(".goodbye");
            var e1_siblingb = new Element(".helloAgain");
            root.Add(e1);
            root.Add(e1_sibling);
            root.Add(e1_siblingb);
            root.Add(new Variable("@RootVariable", new Color(1, 1, 1)));
            e1.Add(e2);
            e2.Add(e3);
            e2.Add(new Variable("@Variable", new Color(1, 1, 1)));
            e2.Add(new Variable("@NumVariable", new Number(10)));
            var nearestEl = e3.Nearest("@Variable");
            Assert.AreEqual(nearestEl.ToString(), "@Variable");

            //TODO: Remove this nonsense it isnt a test its just to see ToCSS output 
            var nodes = new List<INode>
                            {
                                new Variable("@Variable", new Color(1, 1, 1)),
                                new Operator("+"),
                                new Number(2)
                            };
            e1.Add(new Property("color", nodes));
            e1_sibling.Add(new Property("color", nodes));
            e1_siblingb.Add(new Property("color", nodes));
            var nodesb = new List<INode>
                             {
                                 new Number("px", 4),
                                 new Operator("*"),
                                 new Variable("@NumVariable")
                             };



            e2.Add(new Property("padding", nodesb));

            var nodesc = new List<INode>
                             {
                                 new Variable("@RootVariable", new Color(1, 1, 1)),
                                 new Operator("+"),
                                 new Variable("@Variable", new Color(1, 1, 1))
                             };
            e3.Add(new Property("background-color", nodesc));
            Console.WriteLine(root.Group().ToCss());
        }
    }
}