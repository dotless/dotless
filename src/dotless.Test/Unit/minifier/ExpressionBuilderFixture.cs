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

namespace dotless.Test.Unit.minifier
{
    using Core.minifier;
    using NUnit.Framework;

    [TestFixture]
    public class ExpressionBuilderFixture
    {
        [Test]
        public void CorrectlyIdentifiesFirstColonAsSplitter()
        {
            var input = "a:b:c";
            IExpression expression = BuildExpression(input);
            Assert.AreEqual(expression.Expression.Key, "a");
            Assert.AreEqual(expression.Expression.Value, "b:c");
        }

        [Test]
        public void RemovesUnnecessaryWhitespacesInValue()
        {
            var input = "@a:  1 + 3;";
            var expression = BuildExpression(input);

            Assert.AreEqual("1+3;", expression.Expression.Value);
        }

        [Test]
        public void LeavesStringsIntact()
        {
            var input = "hover: \"a b\"";
            var expression = BuildExpression(input);

            Assert.AreEqual("\"a b\"", expression.Expression.Value);
        }

        [Test]
        public void ContinuesWhiteSpaceRemovalAfterString()
        {
            var input = "hover: \"a b\" + 3;";
            var expression = BuildExpression(input);

            Assert.AreEqual("\"a b\"+3;", expression.Expression.Value);
        }

        [Test]
        public void CanHandleSingleQuotationMarkString()
        {
            var input = "hover: 'a b' + 3;";
            var expression = BuildExpression(input);

            Assert.AreEqual("'a b'+3;", expression.Expression.Value);
        }

        [Test]
        public void CanHandleValuesWithSpaces()
        {
            var input = "outline: 1px solid red;";
            var expression = BuildExpression(input);

            Assert.AreEqual("1px solid red;", expression.Expression.Value);
        }

        [Test]
        public void CanHandleExpressionWithoutTrailingSemicolon()
        {
            var input = "font-size: 12px";
            var expression = BuildExpression(input);

            Assert.AreEqual("12px", expression.Expression.Value);
        }

        private IExpression BuildExpression(string input)
        {
            return new ExpressionBuilder().BuildExpression(input.ToCharArray());
        }
    }
}