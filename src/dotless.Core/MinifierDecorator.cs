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
    using System.Text;
    using minifier;

    public class MinifierDecorator : ILessEngine
    {
        private readonly ILessEngine engine;

        public MinifierDecorator(ILessEngine engine)
        {
            this.engine = engine;
        }

        public string TransformToCss(string filename)
        {
            string buffer = engine.TransformToCss(filename);
            var processor = new Processor(buffer);
            return new StringBuilder().Append(processor.Output).ToString();
        }
    }
}