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

            Assert.That(prop.ToCss(), Is.EqualTo("background-color: #151515;"));

            var newColor = prop.Evaluate();

            Assert.That(newColor.ToString(), Is.EqualTo("#151515"));
            Assert.That(newColor, Is.TypeOf<Color>());
        }

        [Test]
        public void CanEvaluateExpressionNumberProperties()
        {
            var prop = new Property("height", new Number("px", 1));
            prop.Add(new Operator("+"));
            prop.Add(new Number("px", 2));
            prop.Add(new Operator("*"));
            prop.Add(new Number(20));

            Assert.That(prop.ToCss(), Is.EqualTo("height: 41px;"));

            var newNumber = prop.Evaluate();

            Assert.That(newNumber, Is.TypeOf<Number>());
        }

        [Test]
        public void CanEvaluateSeveralPropertiesWithoutOperators()
        {
            var prop = new Property("border", new Number("px", 1));
            prop.Add(new Number("px", 2));
            prop.Add(new Number("px", 2));
            prop.Add(new Number("px", 2));

            Assert.That(prop.ToCss(), Is.EqualTo("border: 1px 2px 2px 2px;"));
        }
    }
}