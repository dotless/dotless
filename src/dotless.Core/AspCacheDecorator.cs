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
    using System.Configuration;
    using Abstractions;

    public class AspCacheDecorator : ILessEngine
    {
        private readonly ILessEngine underlying;
        private readonly ICache cache;

        public AspCacheDecorator(ILessEngine underlying, ICache cache)
        {
            this.underlying = underlying;
            this.cache = cache;
        }

        public string TransformToCss(LessSourceObject source)
        {
            if (!source.Cacheable) throw new ConfigurationErrorsException("Your LessSource does not support ASP caching!\nPlease either disable caching or select a source provider that supports caching like AspServerPathSource or FileSource");
            if (!cache.Exists(source.Key))
            {
                string css = underlying.TransformToCss(source);
                cache.Insert(source.Key, css);
            }
            return cache.Retrieve(source.Key);
        }
    }
}