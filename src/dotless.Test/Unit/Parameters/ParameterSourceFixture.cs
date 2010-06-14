namespace dotless.Test.Unit.Parameters
{
    using System.Collections.Specialized;
    using Core.Parameters;
    using NUnit.Framework;

    public class ParameterSourceFixture : HttpFixtureBase
    {
        [Test]
        public void ReturnsEmptyDictionaryIfNoParametersArePassed()
        {
            var queryStringParameterSource = new QueryStringParameterSource(Http.Object);
            HttpRequest.Setup(p => p.QueryString).Returns(new NameValueCollection());

            var dictionary = queryStringParameterSource.GetParameters();

            Assert.AreEqual(0, dictionary.Count);
        }

        [Test]
        public void GetsParametersFromQueryStringDictionary()
        {
            var queryStringParameterSource = new QueryStringParameterSource(Http.Object);
            HttpRequest.Setup(p => p.QueryString).Returns(new NameValueCollection {{"hello", "world"}});

            var dictionary = queryStringParameterSource.GetParameters();

            Assert.IsNotNull(dictionary["hello"]);
        }

        [Test]
        public void CanHandleMultipleEntries()
        {
            var queryStringParameterSource = new QueryStringParameterSource(Http.Object);
            HttpRequest.Setup(p => p.QueryString).Returns(new NameValueCollection
                                                              {
                                                                  {"hello", "world"},
                                                                  {"something", "else"},
                                                                  {"width", "15px"}
                                                              });

            var dictionary = queryStringParameterSource.GetParameters();

            Assert.AreEqual(3, dictionary.Count);
        }
    }
}