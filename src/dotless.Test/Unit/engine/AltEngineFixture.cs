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

using System;
using System.IO;
using dotless.Core.engine;
using NUnit.Framework;

namespace dotless.Test.Unit.engine
{
    [TestFixture]
    public class AltEngineFixture
    {
        [Test]
        public void Can_Retrieve_Css_Dom()
        {
            var engine = new ExtensibleEngineImpl(".outer .inner > a:Hover { background: red; }");
            Assert.AreEqual(1, engine.CssDom.Elements.Count);
            Assert.AreEqual(1, engine.CssDom.Elements[0].Properties.Count);
        }

        [Test]
        public void Css_Dom_Should_Contain_Two_Elements_For_CSV_Props()
        {
            var engine = new ExtensibleEngineImpl(".test, .trial{ background: red; }");
            Assert.AreEqual(2, engine.CssDom.Elements.Count);
        }

        [Test]
        public void Css_Dom_Should_Contain_One_Elements_For_Nested_Element_With_No_Props()
        {
            var engine = new ExtensibleEngineImpl(".outer{ .inner { background: red; } }");
            Assert.AreEqual(1, engine.CssDom.Elements.Count);
        }

        [Test]
        public void Css_Dom_Should_Contain_Two_Elements_For_Nested_Element_With_Props()
        {
            var engine = new ExtensibleEngineImpl(".outer{ background: blue; .inner { background: red; } }");
            Assert.AreEqual(2, engine.CssDom.Elements.Count);
        }

        [Test]
        public void Css_Dom_Should_Contain_Four_Elements_For_Nested_Element_With_Props()
        {
            var engine = new ExtensibleEngineImpl(@".outer{ .inner { background: red; }  background: blue;}
                                         .outerb{ .innerb { background: red; } background: blue; }");
            Assert.AreEqual(4, engine.CssDom.Elements.Count);
        }

        [Test]
        public void Css_Dom_Should_Contain_Five_Elements_For_Nested_Element_With_Props()
        {
            var engine = new ExtensibleEngineImpl(@".outer{ background: blue; .inner { background: red; } }
                                         .outerb{ background: blue; .innerb { background: red; } .innerc { background: red; }}");
            Assert.AreEqual(5, engine.CssDom.Elements.Count);
        }

    }
}