namespace dotless.Core.Parameters
{
    using System.Collections.Generic;
    using Abstractions;

    public class QueryStringParameterSource : IParameterSource
    {
        private readonly IHttp http;

        public QueryStringParameterSource(IHttp http)
        {
            this.http = http;
        }

        public IDictionary<string, string> GetParameters()
        {
            var dictionary = new Dictionary<string, string>();
            var queryString = http.Context.Request.QueryString;
            var allKeys = queryString.AllKeys;
            foreach (var key in allKeys)
            {
                if (key != null)
                {
                    string s = queryString[key];
                    dictionary.Add(key, s);
                }
            }
            return dictionary;
        }
    }
}