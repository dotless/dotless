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
        private readonly IHttp _http;
        private readonly IResponse _response;
        private readonly ILessEngine _engine;
        private readonly IFileReader _fileReader;

        public HandlerImpl(IHttp http, IResponse response, ILessEngine engine, IFileReader fileReader)
        {
            _http = http;
            _response = response;
            _engine = engine;
            _fileReader = fileReader;
        }

        public void Execute()
        {
            var localPath = _http.Context.Request.Url.LocalPath;

            var source = _fileReader.GetFileContents(localPath);

            _response.WriteCss(_engine.TransformToCss(source, localPath));
        }
    }
}