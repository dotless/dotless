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
    using Abstractions;
    using configuration;

    public class HandlerImpl
    {
        public void Execute(ICache cache, IPathProvider pathProvider, IRequest request, IResponse response, DotlessConfiguration configuration, ILessEngine engine)
        {
            // our unprocessed filename   
            var lessFile = pathProvider.MapPath(request.LocalPath);

            if (configuration.CacheEnabled)
            {
                
                if (!cache.Exists(lessFile))
                {
                    string css = engine.TransformToCss(lessFile);
                    cache.Insert(lessFile, css);
                }
                response.WriteCss(cache.Retrieve(lessFile));
            }
            else
            {
                string css = engine.TransformToCss(lessFile);
                response.WriteCss(css);
            }
        }
    }
}