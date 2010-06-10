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

//Code contributed by alexander.uslontsev
namespace dotless.Core.Abstractions
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using System.Web;

    public class CachedCssResponse : IResponse
    {
        private readonly IHttp _http;
        private const int CacheAgeMinutes = 5;

        public CachedCssResponse(IHttp http)
        {
            _http = http;
        }

        public void WriteCss(string css)
        {
            var response = _http.Context.Response;
            var request = _http.Context.Request;

            response.Cache.SetCacheability(HttpCacheability.Public);
            response.Cache.SetMaxAge(new TimeSpan(0, CacheAgeMinutes, 0));

            response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(CacheAgeMinutes));
            response.Cache.SetETagFromFileDependencies();

            response.ContentType = "text/css";
            response.Write(css);

/*
            if (request.Headers.Get("If-None-Match") == response.Headers.Get("ETag"))
            {
                response.StatusCode = 304;
                response.StatusDescription = "Not Modified";

                // Explicitly set the Content-Length header so client
                // keeps the connection open for other requests.
                response.AddHeader("Content-Length", "0");
            }
            else
            {
                response.ContentType = "text/css";
                response.Write(css);
            }

            response.End();
*/
        }
    }
}