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

using NUnit.Framework;

namespace dotless.Test.Spec.ExtensibleEngine
{
    [TestFixture]
    public class SpecEngine
    {
        private const string Upcoming = "Upcoming functionality";

        [Test]
        public void ShouldParseAccessors()
        {
            SpecHelper.ShouldEqual("accessors"); 
        }
        [Test]
        public void ShouldGroupSelectorsWhenItCan()
        {
            SpecHelper.ShouldEqual("selectors"); 
        }
        [Test]
        public void ShouldParseABigFile()
        {
            SpecHelper.ShouldEqual("big"); 
        }
        [Test]
        public void ShouldHandleComplexColorOperations()
        {
            SpecHelper.ShouldEqual("colors"); 
        }
        [Test]
        public void ShouldParseComments()
        {
            SpecHelper.ShouldEqual("comments"); 
        }
        [Test]
        public void ShouldParseCss3()
        {
            SpecHelper.ShouldEqual("css-3"); 
        }
        [Test]
        public void ShouldParseCss()
        {
            SpecHelper.ShouldEqual("css"); 
        }
        [Test]
        public void ShouldHandleSomeFunctions()
        {
            SpecHelper.ShouldEqual("functions"); 
        }
        [Test]
        public void ShouldWorkWithImport()
        {
            SpecHelper.ShouldEqual("import"); 
        }
        [Test]
        public void ShouldEvaluateVariablesLazily()
        {
            SpecHelper.ShouldEqual("lazy-eval"); 
        }
        [Test, Ignore(Upcoming)]
        public void ShouldParseMixins()
        {
            //Comma seperated mixins not working
            SpecHelper.ShouldEqual("mixins"); 
        }
        [Test, Ignore(Upcoming)]
        public void ShouldParseMixinsWithArguments()
        {
            SpecHelper.ShouldEqual("mixins-args"); 
        }
        [Test]
        public void ShouldParseOperations()
        {
            SpecHelper.ShouldEqual("operations"); 
        }
        [Test]
        public void ShouldParseNestedRules()
        {
            SpecHelper.ShouldEqual("rulesets"); 
        }
        [Test]
        public void ShouldManageScope()
        {
            SpecHelper.ShouldEqual("scope"); 
        }
        [Test]
        public void ShouldManageStrings()
        {
            SpecHelper.ShouldEqual("strings"); 
        }
        [Test]
        public void ShouldManageVariables()
        {
            SpecHelper.ShouldEqual("variables"); 
        }
        [Test]
        public void ShouldManageWhitespace()
        {
            //NOTE: Change this test from original as it was testing rouping and whitespace, which is wrong!
            //See ShouldMergeSameElement for grouping tests
            SpecHelper.ShouldEqual("whitespace"); 
        }

        [Test, Ignore(Upcoming)]
        public void ShouldMergeSameElement()
        {
            SpecHelper.ShouldEqual("merge-same");
        }
        [Test]
        public void ShouldManageNamespacedMixins()
        {
            SpecHelper.ShouldEqual("namespaces");
        }
        [Test]
        public void CantMixVariableContexts()
        {
            SpecHelper.ShouldEqual("mixed-context-variables");
        }
        [Test]
        public void CantWorkWithDoublesInNumbers()
        {
            SpecHelper.ShouldEqual("decimal-round");
        }
    }
}