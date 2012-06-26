namespace dotless.Test.Unit.Parameters
{
    using Core.configuration;
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

        [Test]
        public void AttackOnKey()
        {
            var queryStringParameterSource = new QueryStringParameterSource(Http.Object);
            
            QueryString["a:1;c:expression(alert(1));b"] = "world";

            var dictionary = queryStringParameterSource.GetParameters();

            Assert.AreEqual(0, dictionary.Count);
        }

        [Test]
        public void AttackOnValue()
        {
            var queryStringParameterSource = new QueryStringParameterSource(Http.Object);
            
            QueryString["a"] = "world;b:expression(alert(2));c";

            var dictionary = queryStringParameterSource.GetParameters();

            Assert.AreEqual(0, dictionary.Count);
        }

        [Test]
        public void TestValidKeysAccepted()
        {
            var queryStringParameterSource = new QueryStringParameterSource(Http.Object);
            
            QueryString["a-b_--oilAB9_01"] = "world";

            var dictionary = queryStringParameterSource.GetParameters();

            Assert.AreEqual(1, dictionary.Count);
        }

        [Test]
        public void TestValidValuesAccepted()
        {
            var queryStringParameterSource = new QueryStringParameterSource(Http.Object);
            
            QueryString["key1"] = "#fff";
            QueryString["key2"] = "#fff000";
            QueryString["key3"] = "red";
            QueryString["key4"] = "@var-_1";
            QueryString["key5"] = "1.2px";
            QueryString["key6"] = "left";
            QueryString["key7"] = "var, blah";

            var dictionary = queryStringParameterSource.GetParameters();

            Assert.AreEqual(7, dictionary.Count);
        }

        [Test]
        public void ShouldSkipSessionParam()
        {
            const string paramName = "loadSession";

            Config.SessionMode = DotlessSessionStateMode.QueryParam;
            Config.SessionQueryParamName = paramName;

            var queryStringParameterSource = new QueryStringParameterSource(Http.Object);

            QueryString[paramName] = "yeah";

            var dictionary = queryStringParameterSource.GetParameters();

            Assert.IsFalse(dictionary.ContainsKey(paramName));
        }
    }
}