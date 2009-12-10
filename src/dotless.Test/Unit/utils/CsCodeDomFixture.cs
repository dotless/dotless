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

namespace dotless.Test.Unit.utils
{
    using System;
    using NUnit.Framework;
    using Core.engine;
    using Core.utils;

    [TestFixture]
    public class CsCodeDomFixture
    {
        [Test]
        public void CanEvaluateSingleProperty()
        {
            var color = new Color(10, 10, 10);
            Console.WriteLine(color.ToCSharp());
            Console.WriteLine(CsEval.Eval(color.ToCSharp()));
        }

        [Test]
        public void CanEvaluateNumbers()
        {
            float result = (float)CsEval.Eval("10f/4;");
            Assert.AreEqual(2.5f, result);
        }

        [Test]
        public void CanEvaluateBraces()
        {
            float result = (float) CsEval.Eval("(2f + 3) * 2");
            Assert.AreEqual(10, result);
        }
    }
}