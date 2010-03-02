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

    public class HandlerImpl
    {
        private readonly IRequest request;
        private readonly IResponse response;
        private readonly ILessEngine engine;
        private readonly ILessSource lessSource;

        public HandlerImpl(IRequest request, IResponse response, ILessEngine engine, ILessSource lessSource)
        {
            this.request = request;
            this.response = response;
            this.engine = engine;
            this.lessSource = lessSource;
        }

        public void Execute()
        {
            // our unprocessed filename   
            var source = lessSource.GetSource(request.LocalPath);
            response.WriteCss(engine.TransformToCss(source));
        }
    }
}