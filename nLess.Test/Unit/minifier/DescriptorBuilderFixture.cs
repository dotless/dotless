/*
 * Copyright 2009 Less.Net
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *  http://www.apache.org/licenses/LICENSE-2.0
 *  
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

namespace nLess.Test.Unit.minifier
{
    using nless.Core.minifier;
    using NUnit.Framework;

    [TestFixture]
    public class DescriptorBuilderFixture
    {
        [Test]
        public void DescriptorBuilderTrimsWhiteSpaces()
        {
            var input = ".a ";
            string descriptor = BuildDescription(input);

            Assert.AreEqual(".a", descriptor);
        }

        private string BuildDescription(string input)
        {
            return new DescriptorBuilder().BuildDescriptor(input.ToCharArray());
        }
    }
}