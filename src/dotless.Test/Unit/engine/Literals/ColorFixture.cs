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

namespace dotless.Test.Unit.engine.Literals
{
    using Core.engine;
    using NUnit.Framework;

    [TestFixture]
    public class ColorFixture
    {
        [Test]
        public void CanNormailzeRgb()
        {
            var color = new Color(10, 277, -10);
            Assert.AreEqual(10, color.R);
            Assert.AreEqual(255, color.G);
            Assert.AreEqual(0, color.B);
        }

        [Test]
        public void CanAddToRgbColor()
        {
            var color = new Color(10, 10, 10);
            Assert.AreEqual(10, color.R);
            Assert.AreEqual(10, color.G);
            Assert.AreEqual(10, color.B);

            color = color + 10;

            Assert.AreEqual(20, color.R);
            Assert.AreEqual(20, color.G);
            Assert.AreEqual(20, color.B);

            color = color + new Color(20, 30, 10);

            Assert.AreEqual(40, color.R);
            Assert.AreEqual(50, color.G);
            Assert.AreEqual(30, color.B);
        }

        [Test]
        public void CanMinusFromRgbColor()
        {
            var color = new Color(100, 100, 100);
            color = color - 10;

            Assert.AreEqual(90, color.R);
            Assert.AreEqual(90, color.G);
            Assert.AreEqual(90, color.B);

            color = color - new Color(20, 30, 10);

            Assert.AreEqual(70, color.R);
            Assert.AreEqual(60, color.G);
            Assert.AreEqual(80, color.B);
        }

        [Test]
        public void CanDivideRgbColors()
        {
            var color = new Color(100, 100, 100);
            color = color /2;

            Assert.AreEqual(50, color.R);
            Assert.AreEqual(50, color.G);
            Assert.AreEqual(50, color.B);

            color = color / new Color(2, 1, 5);

            Assert.AreEqual(25, color.R);
            Assert.AreEqual(50, color.G);
            Assert.AreEqual(10, color.B);
        }

        [Test]
        public void CanMultiplyRgbColors()
        {
            var color = new Color(100, 100, 100);
            color = color * 2;

            Assert.AreEqual(200, color.R);
            Assert.AreEqual(200, color.G);
            Assert.AreEqual(200, color.B);

            color = color * new Color(2, 1, 5);

            Assert.AreEqual(255, color.R);
            Assert.AreEqual(200, color.G);
            Assert.AreEqual(255, color.B);
        }

        [Test]
        public void CanRetrieveColorHex()
        {
            var color = new Color(255, 255, 255);
            Assert.AreEqual("#ffffff", color.ToCss());

            color = new Color(0, 0, 0);
            Assert.AreEqual("#000000", color.ToCss());
        }
    }
}