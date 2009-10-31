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
        private XmlNode GetTestnode(string minifyCssvalue)
        {
            var xml = String.Format("<dotless minifyCss=\"{0}\"></dotless>", minifyCssvalue);
            var document = new XmlDocument();
            document.Load(new StringReader(xml));
            return document.DocumentElement;
        }

        [Test]
        public void ReadsMinificationFromXmlNodeSetsMinifyOutputTrue()
        {
            var element = GetTestnode("true");
            var interpreter = new XmlConfigurationInterpreter();

            var output = interpreter.Process(element);

            Assert.IsTrue(output.MinifyOutput);
        }

        [Test]
        public void MinifyCssAttributeCanBeFalseSetsMinifyOutputFalse()
        {
            XmlNode testnode = GetTestnode("false");
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