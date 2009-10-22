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
        public void DefaultCacheStrategyIsSetToFileWatcher()
        {
            var testnode = GetTestNodeWithCache("enabled");
            var interpreter = new XmlConfigurationInterpreter();

            var output = interpreter.Process(testnode);


            Assert.IsInstanceOf(typeof(FileWatcherCacheStrategy), output.CacheConfiguration.CacheStrategy);
        }

        public void CacheStrategyCanBeSetTo_TimeExpiring()
        {
            var testnode = GetTestNodeWithCache("enabled");
            var interpreter = new XmlConfigurationInterpreter();

            var output = interpreter.Process(testnode);


            Assert.IsInstanceOf(typeof(FileWatcherCacheStrategy), output.CacheConfiguration.CacheStrategy);
        }

        private XmlNode GetTestNodeWithCache(string cacheEnabled)
        {
            var xml = String.Format("<dotless cache=\"{0}\"></dotless>", cacheEnabled);
            var document = new XmlDocument();
            document.Load(new StringReader(xml));
            return document.DocumentElement;
        }



        private XmlNode GetTestNodeWithCacheAndStrategy(string cacheStrategy)
        {
            throw new NotImplementedException();
        }

        private XmlNode GetTestnodeWithoutAttribute()
        {
            var xml = "<dotless></dotless>";
            var document = new XmlDocument();
            document.Load(new StringReader(xml));
            return document.DocumentElement;
        }
    }
}