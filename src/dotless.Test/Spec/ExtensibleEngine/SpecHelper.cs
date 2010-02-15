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

using System.IO;
using System.Linq;
using dotless.Core.engine;
using NUnit.Framework;

namespace dotless.Test.Spec.ExtensibleEngine
{
    public class SpecHelper
    {
        public static string Lessify(string fileName)
        {
            var file = Path.Combine("Spec/ExtensibleEngine/less", fileName + ".less");
            return new ExtensibleEngineImpl(File.ReadAllText(file)).Css.Replace("\r\n", "\n");
        }
        public static string Css(string fileName)
        {
            var file = Path.Combine("Spec/ExtensibleEngine/css", fileName + ".css");
            return File.ReadAllText(file).Replace("\r\n", "\n");
        }

        public static void ShouldEqual(string filename)
        {
            var less = Lessify(filename);
            var css = Css(filename);

            var mismatch = css.Split('\n').Zip(less.Split('\n'))
                .Select((p, i) => new { LineNumber = i + 1, CssLine = p.First.ToLower(), LessLine = p.Second.ToLower() })
                .FirstOrDefault(x => x.CssLine != x.LessLine);

            if (mismatch == null)
                return;

            Assert.That(mismatch.LessLine, Is.EqualTo(mismatch.CssLine), "First mismatch on line {0}", mismatch.LineNumber);
        }
    }
}