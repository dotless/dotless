using System;
using dotless.Core.configuration;

namespace dotless.Core.Parameters
{
    using System.Collections.Generic;
    using Abstractions;
    using System.Text.RegularExpressions;

    public class QueryStringParameterSource : IParameterSource
    {
        private readonly IHttp http;
        private readonly Regex _keyWhitelist = new Regex(@"^[a-zA-Z0-9_-]+$");
        private readonly Regex _valueWhitelist = new Regex(@"^[#@]?[a-zA-Z0-9""' _\.,-]*$");

        public QueryStringParameterSource(IHttp http)
        {
            this.http = http;
        }

        public IDictionary<string, string> GetParameters()
        {
            var dictionary = new Dictionary<string, string>();
            var queryString = http.Context.Request.QueryString;
            var allKeys = queryString.AllKeys;
            var config = new WebConfigConfigurationLoader().GetConfiguration();
            var sessionParam = config.SessionMode == DotlessSessionStateMode.QueryParam ? config.SessionQueryParamName : null;
            foreach (var key in allKeys)
            {
                if (key != null)
                {
                    if (key.Equals(sessionParam, StringComparison.OrdinalIgnoreCase) || !_keyWhitelist.IsMatch(key))
                        continue;

                    string s = queryString[key];

                    if (!_valueWhitelist.IsMatch(s))
                        continue;

                    dictionary.Add(key, s);
                }
            }
            return dictionary;
        }
    }
}