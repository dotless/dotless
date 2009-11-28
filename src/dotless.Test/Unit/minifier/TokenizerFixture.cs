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
    using System.Linq;
    using Core.minifier;
    using NUnit.Framework;

    [TestFixture]
    public class TokenizerFixture
    {
        [Test]
        public void AppendsExpressionToCurrentLevel()
        {
            var input = "background: black;";
            ITreeNode tree = BuildTree(input);;

            Assert.AreEqual(1, tree.Expressions.Count());
        }

        [Test]
        public void AppendsSubNodesToCurrentLevel()
        {
            var input = ".a { background: black; }";
            ITreeNode tree = BuildTree(input);;

            Assert.AreEqual(1, tree.Children.Count());
        }

        [Test]
        public void CanHandleMultipleChildrenOnOneLevel()
        {
            var input = ".a { background: black; } .b {b:black;}";

            ITreeNode tree = BuildTree(input);;
            Assert.AreEqual(2, tree.Children.Count());
        }

        [Test]
        public void CanHandleMultipleExpressionsOnOneLevel()
        {
            var input = "@y: @x + 1;@z: @x * 2 + @y;";

            ITreeNode tree = BuildTree(input);;
            Assert.AreEqual(2, tree.Expressions.Count());
        }

        [Test]
        public void CanHandleNestedChildren()
        {
            var input = "#namespace { .borders { border-style: dotted; } }";
            ITreeNode tree = BuildTree(input);

            Assert.AreEqual(1, tree.Children.Count());
            Assert.AreEqual(1, tree.Children.First().Children.Count());
        }

        [Test]
        public void CanHandleNestedChildExpressions()
        {
            var input = "#namespace { .borders { border-style: dotted; } }";
            ITreeNode tree = BuildTree(input);

            Assert.AreEqual(1, tree
                                   .Children.First()
                                   .Children.First().Expressions.Count());
        }

        [Test]
        public void RootNodeIsCalledROOT()
        {
            var input = "";
            ITreeNode tree = BuildTree(input);

            Assert.AreEqual("ROOT", tree.Descriptor);
        }

        [Test]
        public void NestedChildNodeDescriptorMatchesCSSDescriptor()
        {
            var input = ".a { .b { x:b; } }";
            ITreeNode tree = BuildTree(input);

            Assert.AreEqual(".b", tree
                                      .Children.First()
                                      .Children.First().Descriptor);
        }

        [Test]
        public void CanHandleBracesInStrings()
        {
            var input = ".a { background: \"{\"; }";
            ITreeNode tree = BuildTree(input);

            Assert.AreEqual("\"{\"", tree
                                         .Children.First()
                                         .Expressions.First()
                                         .Expression.Value);
        }

        [Test]
        public void CanHandleSingleQuoteStrings()
        {
            var input = "background: '{';";
            ITreeNode tree = BuildTree(input);

            Assert.AreEqual("'{'", tree.Expressions.First()
                                       .Expression.Value);
        }

        [Test]
        public void CanHandleComplexStrings()
        {
            var input = "content: \"#*%:&^,)!.(~*})\";";
            ITreeNode tree = BuildTree(input);

            Assert.AreEqual("\"#*%:&^,)!.(~*})\"", tree.Expressions.First().Expression.Value);
        }

        [Test]
        public void CanHandleMinusComposedValues()
        {
            var input = "margin: 0 auto -168px;";
            ITreeNode tree = BuildTree(input);
            Assert.AreEqual("0 auto -168px", tree.Expressions.First().Expression.Value);
        }

        [Test]
        public void CanHandleMissingTralingSemicolon()
        {
            var input = "body { font-size: 12px }";
            ITreeNode tree = BuildTree(input);

            Assert.AreEqual("12px", tree.Children.First().Expressions.First().Expression.Value);
        }

        private ITreeNode BuildTree(string input)
        {
            var tokenizer = new Tokenizer();
            return tokenizer.BuildTree(input);;
        }
    }
}