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

namespace dotless.Test.Unit.configuration
{
    using System;
    using System.IO;
    using System.Xml;
    using Core.configuration;
    using NUnit.Framework;

    [TestFixture]
    public class XmlConfigurationFixture
    {
        private XmlNode GetTestnode(string minifyCssvalue, string cacheValue)
        {
            var xml = String.Format("<dotless minifyCss=\"{0}\" cache=\"{1}\" ></dotless>", minifyCssvalue, cacheValue);
            var document = new XmlDocument();
            document.Load(new StringReader(xml));
            return document.DocumentElement;
        }

        [Test]
        public void ReadsMinificationFromXmlNodeSetsMinifyOutputTrue()
        {
            var element = GetTestnode("true", "true");
            var interpreter = new XmlConfigurationInterpreter();

            var output = interpreter.Process(element);

            Assert.IsTrue(output.MinifyOutput);
        }

        [Test]
        public void ReadsCachingFromXmlNodeSetsCachingTrue()
        {
            var element = GetTestnode("true", "true");
            var interpreter = new XmlConfigurationInterpreter();

            var output = interpreter.Process(element);

            Assert.IsTrue(output.CacheEnabled);
        }

        [Test]
        public void ReadsCachingFromXmlNodeSetsCachingFalse()
        {
            var element = GetTestnode("true", "false");
            var interpreter = new XmlConfigurationInterpreter();

            var output = interpreter.Process(element);

            Assert.IsFalse(output.CacheEnabled);
        }

        [Test]
        public void MinifyCssAttributeCanBeFalseSetsMinifyOutputFalse()
        {
            XmlNode testnode = GetTestnode("false", "true");
            var interpreter = new XmlConfigurationInterpreter();

            var output = interpreter.Process(testnode);

            Assert.IsFalse(output.MinifyOutput);
        }

        [Test]
        public void MinifyCssAttributeCanBeNull_SetsMinifyOutputFalse()
        {
            XmlNode testnode = GetTestnodeWithoutAttribute();
            var interpreter = new XmlConfigurationInterpreter();

            var output = interpreter.Process(testnode);

            Assert.IsFalse(output.MinifyOutput);
        }

        [Test]
        public void CacheIsEnabledByDefault()
        {
            XmlNode attribute = GetTestnodeWithoutAttribute();
            DotlessConfiguration xml = InterpretXml(attribute);
            Assert.IsTrue(xml.CacheEnabled);
        }

        private DotlessConfiguration InterpretXml(XmlNode xml)
        {
            var interpreter = new XmlConfigurationInterpreter();
            return interpreter.Process(xml);
        }

        private XmlNode GetTestnodeWithoutAttribute()
        {
            var xml = "<dotless></dotless>";
            return CreateXmlNode(xml);
        }

        private XmlNode CreateXmlNode(string xml)
        {
            var document = new XmlDocument();
            document.Load(new StringReader(xml));
            return document.DocumentElement;
        }
    }
}