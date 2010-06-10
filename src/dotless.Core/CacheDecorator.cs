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
    using System;
    using System.Collections.Generic;
    using Abstractions;

    public class CacheDecorator : ILessEngine
    {
        public readonly ILessEngine Underlying;
        public readonly ICache Cache;

        public CacheDecorator(ILessEngine underlying, ICache cache)
        {
            Underlying = underlying;
            Cache = cache;
        }

        public string TransformToCss(string source, string fileName)
        {
            if (!Cache.Exists(fileName))
            {
                var css = Underlying.TransformToCss(source, fileName);
                var imports = GetImports();
                Cache.Insert(fileName, imports, css);
                return css;
            }

            return Cache.Retrieve(fileName);
        }

        public IEnumerable<string> GetImports()
        {
            return Underlying.GetImports();
        }
    }
}