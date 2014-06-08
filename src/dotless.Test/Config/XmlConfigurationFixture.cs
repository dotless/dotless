namespace dotless.Test.Config
{
    using System.Xml;
    using NUnit.Framework;
    using Core.configuration;
    
    public class XmlConfigurationFixture
    {
        [Test]
        public void CheckSessionMode()
        {
            Assert.That(LoadConfig(@"<dotless sessionMode=""enabled""/>").SessionMode, Is.EqualTo(DotlessSessionStateMode.Enabled));
            Assert.That(LoadConfig(@"<dotless />").SessionMode, Is.EqualTo(DotlessSessionStateMode.Disabled));
            Assert.That(LoadConfig(@"<dotless sessionMode=""QueryParam""/>").SessionMode, Is.EqualTo(DotlessSessionStateMode.QueryParam));
        }

        [Test]
        public void DefaultHttpExpiryInMinutes()
        {
            Assert.That(LoadConfig(@"<dotless />").HttpExpiryInMinutes, Is.EqualTo(DotlessConfiguration.DefaultHttpExpiryInMinutes));
        }

        [Test]
        public void CustomHttpExpiryInMinutes()
        {
            Assert.That(LoadConfig(@"<dotless httpExpiryInMinutes=""5""/>").HttpExpiryInMinutes, Is.EqualTo(5));
        }

        [Test]
        [ExpectedException]
        public void ShouldThrowOnEmptySessionParamName()
        {
            LoadConfig(@"<dotless sessionMode=""queryParam"" sessionQueryParamName=""""/>");
        }

        private DotlessConfiguration LoadConfig(string xml)
        {
            var interpreter = new XmlConfigurationInterpreter();
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            return interpreter.Process(doc.DocumentElement);
        }
    }
}
