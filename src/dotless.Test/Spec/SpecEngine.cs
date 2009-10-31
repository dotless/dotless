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

namespace dotless.Test.Spec
{
    using NUnit.Framework;

    [TestFixture]
    public class SpecEngine
    {
        private const string Upcoming = "Upcoming functionality";

        [Test]
        public void ShouldParseAccessors()
        {
            SpecHelper.ShouldEqual("accessors"); //PASS
        }
        [Test, Ignore(Upcoming)]
        public void ShouldGroupSelectorsWhenItCan()
        {
            SpecHelper.ShouldEqual("selectors"); //IGNORE
        }
        [Test]
        public void ShouldParseABigFile()
        {
            SpecHelper.ShouldEqual("big"); //PASS
        }
        [Test]
        public void ShouldHandleComplexColorOperations()
        {
            SpecHelper.ShouldEqual("colors"); //PASS
        }
        [Test]
        public void ShouldParseComments()
        {
            SpecHelper.ShouldEqual("comments"); //PASS
        }
        [Test]
        public void ShouldParseCss3()
        {
            SpecHelper.ShouldEqual("css-3"); //PASS
        }
        [Test]
        public void ShouldParseCss()
        {
            SpecHelper.ShouldEqual("css"); //PASS
        }
        [Test, Ignore(Upcoming)]
        public void ShouldHandleSomeFunctions()
        {
            SpecHelper.ShouldEqual("functions"); //IGNORE
        }   
        [Test]
        public void ShouldWorkWithImport()
        {
            SpecHelper.ShouldEqual("import"); //PASS
        }
        [Test]
        public void ShouldEvaluateVariablesLazily()
        {
            SpecHelper.ShouldEqual("lazy-eval"); //PASS
        }
        [Test, Ignore(Upcoming)]
        public void ShouldParseMixins()
        {
            SpecHelper.ShouldEqual("mixins"); //IGNORE
        }
        [Test, Ignore(Upcoming)]
        public void ShouldParseMixinsWithArguments()
        {
            SpecHelper.ShouldEqual("mixins-args"); //IGNORE
        }
        [Test]
        public void ShouldParseOperations()
        {
            SpecHelper.ShouldEqual("operations"); //PASS
        }
        [Test]
        public void ShouldParseNestedRules()
        {
            SpecHelper.ShouldEqual("rulesets"); //PASS
        }
        [Test]
        public void ShouldManageScope()
        {
            SpecHelper.ShouldEqual("scope"); //PASS
        } 
        [Test]
        public void ShouldManageStrings()
        {
             SpecHelper.ShouldEqual("strings"); //PASS
        } 
        [Test]
        public void ShouldManageVariables()
        {
            SpecHelper.ShouldEqual("variables"); //PASS
        }
        [Test, Ignore(Upcoming)]
        public void ShouldManageWhitespace()
        {
            SpecHelper.ShouldEqual("whitespace"); //IGNORE
        } 
    }
}