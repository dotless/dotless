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

namespace dotless.Core.Abstractions
{
    using System.Web;
    using System.Web.Caching;

    public interface ICache
    {
        void Insert(string fileName, string css);
        bool Exists(string filename);
        string Retrieve(string filename);
    }

    public class CssCache : ICache
    {
        public void Insert(string fileName, string css)
        {
            GetCache().Insert(fileName, css, new CacheDependency(fileName));
        }

        public bool Exists(string filename)
        {
            return Retrieve(filename) != null;
        }

        public string Retrieve(string filename)
        {
            return (string)GetCache()[filename];
        }

        private static Cache GetCache()
        {
            return HttpContext.Current.Cache;
        }
    }
}