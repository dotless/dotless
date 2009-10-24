namespace nLess.Test.Unit.configuration
{
    using System;
    using System.IO;
    using System.Xml;
    using nless.Core.configuration;
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
        public void CacheAttributeCanBeOmitted_DisablesCache()
        {
            XmlNode testnode = GetTestnodeWithoutAttribute();
            var interpeter = new XmlConfigurationInterpreter();

            var output = interpeter.Process(testnode);

            Assert.IsFalse(output.CacheConfiguration.CacheEnabled);
        }

        [Test]
        public void CacheEnabledSetToFalse_DisablesCache()
        {
            XmlNode testnode = GetTestNodeWithCache("disabled");
            var interpeter = new XmlConfigurationInterpreter();

            var output = interpeter.Process(testnode);

            Assert.IsFalse(output.CacheConfiguration.CacheEnabled);
        }

        [Test]
        public void CacheEnabledSetToEnabled_EnablesCache()
        {
            var testnode = GetTestNodeWithCache("enabled");
            var interpeter = new XmlConfigurationInterpreter();

            var output = interpeter.Process(testnode);

            Assert.IsTrue(output.CacheConfiguration.CacheEnabled);
        }

        [Test]
        public void CacheExpirationTimeCanBeSet()
        {
            var testnode = GetTestNodeWithTimeAndExpiration(100);
            var interpreter = new XmlConfigurationInterpreter();

            var output = interpreter.Process(testnode);

            Assert.AreEqual(100, output.CacheConfiguration.CacheExpiration);
        }

        [Test]
        public void CacheStrategyIsFileByDefault()
        {
            var node = GetTestNodeWithCache("enabled");
            var interpreter = new XmlConfigurationInterpreter();

            var output = interpreter.Process(node);

            Assert.AreEqual("file", output.CacheConfiguration.CacheStragy);
        }

        [Test]
        public void CacheStrategy_CanBeSetToFile()
        {
            var node = GetTestNodeWithCacheAndStrategy("file");
            var interpreter = new XmlConfigurationInterpreter();

            var output = interpreter.Process(node);

            Assert.AreEqual("file", output.CacheConfiguration.CacheStragy);
        }

        [Test]
        public void CacheStrategy_CanBeSetToExpiration()
        {
            var node = GetTestNodeWithCacheAndStrategy("expiration");
            var interpreter = new XmlConfigurationInterpreter();

            var output = interpreter.Process(node);

            Assert.AreEqual("expiration", output.CacheConfiguration.CacheStragy);
        }

        private XmlNode GetTestNodeWithTimeAndExpiration(int expiration)
        {
            var xml = String.Format("<dotless cache=\"enabled\" strategy=\"time\" expires=\"{0}\"></dotless>", expiration);
            return CreateXmlNode(xml);
        }

        private XmlNode GetTestNodeWithCache(string cacheEnabled)
        {
            var xml = String.Format("<dotless cache=\"{0}\"></dotless>", cacheEnabled);
            return CreateXmlNode(xml);
        }

        private XmlNode GetTestNodeWithCacheAndStrategy(string cacheStrategy)
        {
            var xml = String.Format("<dotless cache=\"enabled\" strategy=\"{0}\"></dotless>", cacheStrategy);
            return CreateXmlNode(xml);
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