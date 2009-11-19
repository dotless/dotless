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

using dotless.Core.engine.CssNodes;

namespace dotless.Core.engine
{
    public class AltEngine
    {
        public Element LessDom { get; set; }
        public CssDocument CssDom { get; set; }
        public string Css { get; set; }

        /// <summary>
        /// New engine impl
        /// </summary>
        /// <param name="source"></param>
        public AltEngine(string source)
        {
            //Parse the source file and run any Less preprocessors set
            LessDom = Pipeline.LessParser.Parse(source);
            RunLessDomPreprocessors();

            //Convert the LessDom to the CssDom and run any CSS Dom preprocessors set
            CssDom = Pipeline.LessToCssDomConverter.BuildCssDocument(LessDom);
            RunCssDomPreprocessors();

            //Convert the CssDom to Css
            Css = Pipeline.CssBuilder.ToCss(CssDom);
        }

        /// <summary>
        /// Preprocess the Less document before it is sent to the Css converter
        /// </summary>
        private void RunLessDomPreprocessors()
        {
            if (Pipeline.LessDomPreprocessors != null)
                foreach (var lessPreprocessor in Pipeline.LessDomPreprocessors)
                    LessDom = lessPreprocessor.Process(LessDom);
        }

        /// <summary>
        /// Preprocessing CSS Dom before its converted to Css
        /// </summary>
        private void RunCssDomPreprocessors()
        {
            if (Pipeline.CssDomPreprocessors != null)
                foreach (var cssPreprocessor in Pipeline.CssDomPreprocessors)
                    CssDom = cssPreprocessor.Process(CssDom);
        }

    }
}