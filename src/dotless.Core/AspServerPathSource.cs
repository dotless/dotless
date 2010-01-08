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

namespace dotless.Core
{
    using System.IO;
    using System.Web;

    public class AspServerPathSource : ILessSource
    {
        public LessSourceObject GetSource(string key)
        {
            var path = GetServer().MapPath(key);
            return new LessSourceObject
                       {
                           Content = File.ReadAllText(path),
                           Cacheable = true,
                           Key = path
                       };
        }

        private static HttpServerUtility GetServer()
        {
            return HttpContext.Current.Server;
        }
    }
}