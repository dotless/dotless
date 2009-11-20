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

using dotless.Core.engine.CssNodes;
using NUnit.Framework;

namespace dotless.Test.Unit.engine.CssNodes
{
    [TestFixture]
    public class CssPropertyFixture
    {
        [Test]
        public void Can_Get_And_Set_Property_Values()
        {
            var property = new CssProperty("background", "solid 1px blue");
            Assert.AreEqual("background", property.Key);
            Assert.AreEqual("solid 1px blue", property.Value);
        }
    }
}