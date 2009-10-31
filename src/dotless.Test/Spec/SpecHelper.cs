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
    using System.IO;
    using Core.engine;
    using NUnit.Framework;

    public class SpecHelper
    {
        public static string Lessify(string fileName)
        {
            var file = Path.Combine("Spec/less", fileName + ".less");
            return new Engine(File.ReadAllText(file)).Parse().Css.Replace("\r\n", "\n");
        }
        public static string Css(string fileName)
        {
            var file = Path.Combine("Spec/css", fileName + ".css");
            return File.ReadAllText(file).Replace("\r\n", "\n");
        }

        public static void ShouldEqual(string filename)
        {
            var less = Lessify(filename);
            var css = Css(filename);
            css.ShouldEqual(less, string.Format("|{0}| != |{1}|", less, css));
        }
    }

    internal static class SpecExtensions
    {
        public static void ShouldEqual(this string a, string b, string assertionFailedMessage)
        {
            Assert.AreEqual(a.ToLower(), b.ToLower());
        }
    }
}