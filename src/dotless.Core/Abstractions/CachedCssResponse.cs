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
        private const int CacheAgeMinutes = 5;

        public void WriteCss(string css)
        {
            var response = GetResponse();
            var request = GetRequest();

            // The etag should come from the cache instead and have a filedependency on file.less..
            var etag = GetFileETag("file.less", DateTime.Now);

            response.Cache.SetCacheability(HttpCacheability.Public);
            response.Cache.SetMaxAge(new TimeSpan(0, CacheAgeMinutes,
                                                  0));

            response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(CacheAgeMinutes));
            response.Cache.SetETag(etag);

            if (request.Headers.Get("If-None-Match") == etag)
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
        }

        private static string GetFileETag(string fileName, DateTime
                                                        modifiedDate)
        {
            var identifier = fileName + modifiedDate;

            var algorithm = MD5.Create();
            var hash =
                algorithm.ComputeHash(Encoding.UTF8.GetBytes(identifier));
            var hashString = BitConverter.ToString(hash).Replace("-", "");
            return string.Format("\"{0}\"", hashString);
        }

        private static HttpResponse GetResponse()
        {
            return HttpContext.Current.Response;
        }

        private static HttpRequest GetRequest()
        {
            return HttpContext.Current.Request;
        }
    }
}