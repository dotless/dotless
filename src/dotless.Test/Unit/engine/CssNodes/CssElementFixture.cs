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
using dotless.Core.engine.CssNodes;
using NUnit.Framework;

namespace dotless.Test.Unit.engine.CssNodes
{
    [TestFixture]
    public class CssElementFixture
    {
        [Test]
        public void Can_Get_And_Set_Values()
        {
            var element = new CssElement("a > a:Hover");
            Assert.AreEqual("a > a:Hover", element.Identifiers);
        }

        [Test]
        public void Cannot_AddProperties_Twice_Get_And_Set_Values()
        {
            var element = new CssElement("a > a:Hover");
            var property = new CssProperty("background", "red");
            element.AddProperty(property);
            element.AddProperty(property);
            Assert.AreEqual(1, element.Properties.Count);
        }
        
        [Test]
        public void Equal_Properties_Grouped_Correctly()
        {
            var element = new CssElement("a .class");
            element.Properties = new HashSet<CssProperty>
                                     {
                                         new CssProperty("padding", "20px"),
                                         new CssProperty("color","red")
                                     };

            var otherElement = new CssElement("a .notherclass");
            otherElement.Properties = new HashSet<CssProperty>
                                          {
                                         new CssProperty("color","red"),
                                         new CssProperty("padding", "20px"),
                                     };
            Assert.IsTrue(element.IsEquiv(otherElement));
        }
    }
}