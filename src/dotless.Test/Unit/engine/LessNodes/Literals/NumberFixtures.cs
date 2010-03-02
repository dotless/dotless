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

namespace dotless.Test.Unit.engine.Literals
{
    using Core.engine;
    using NUnit.Framework;
    using System.Threading;
    using System.Globalization;

    [TestFixture]
    public class NumberFixture
    {
        [Test]
        public void CanOperateOnNumber()
        {
            var number = new Number("%", 100);
            number += 100;
            Assert.AreEqual("200%", number.ToCss());
        }

        [TestCase("en-GB")]
        [TestCase("de-DE")]
        [TestCase("fr-FR")]
        public void DoesNotBreakOnDifferentLocale(string locale)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(locale);

            var number = new Number(1234.5);
            var css = number.ToCss();
            Assert.AreEqual("1234.5", css);
        }
    }
}