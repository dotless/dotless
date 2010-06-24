namespace dotless.Test.Unit.Parameters
{
    using Core.Parameters;
    using NUnit.Framework;

    public class ParameterSourceFixture : HttpFixtureBase
    {
        [Test]
        public void ReturnsEmptyDictionaryIfNoParametersArePassed()
        {
            var queryStringParameterSource = new QueryStringParameterSource(Http.Object);

            var dictionary = queryStringParameterSource.GetParameters();

            Assert.AreEqual(0, dictionary.Count);
        }

        [Test]
        public void GetsParametersFromQueryStringDictionary()
        {
            var queryStringParameterSource = new QueryStringParameterSource(Http.Object);
            QueryString["hello"] = "world";

            var dictionary = queryStringParameterSource.GetParameters();

            Assert.IsNotNull(dictionary["hello"]);
        }

        [Test]
        public void CanHandleMultipleEntries()
        {
            var queryStringParameterSource = new QueryStringParameterSource(Http.Object);

            QueryString["hello"] = "world";
            QueryString["something"] = "else";
            QueryString["width"] = "15px";

            var dictionary = queryStringParameterSource.GetParameters();

            Assert.AreEqual(3, dictionary.Count);
        }

        [Test]
        public void CanHandleNullEntries()
        {
            var queryStringParameterSource = new QueryStringParameterSource(Http.Object);
            
            QueryString["hello"] = "world";
            QueryString[null] = "1234567890";
            QueryString["width"] = "15px";

            var dictionary = queryStringParameterSource.GetParameters();

            Assert.AreEqual(2, dictionary.Count);
        }
    }
}