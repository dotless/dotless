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

using System.Collections.Generic;
using dotless.Core.parser;

namespace dotless.Core.engine.Pipeline
{
    /// <summary>
    /// Pipeline factory to grab the different parts of required engine process. 
    /// </summary>
    public static class PipelineFactory
    {
        private static ILessParser _lessParser = new LessParser();
        public static ILessParser LessParser
        {
            get
            {
                return _lessParser;
            }
            set
            {
                _lessParser = value;
            }
        }

        private static ILessToCssDomConverter _lessToCssDomConverter = new LessToCssDomConverter();
        public static ILessToCssDomConverter LessToCssDomConverter
        {
            get
            {
                return _lessToCssDomConverter;
            }
            set
            {
                _lessToCssDomConverter = value;
            }
        }

        private static List<ILessDomPreprocessor> _lessDomPreprocessors = new List<ILessDomPreprocessor>();
        public static List<ILessDomPreprocessor> LessDomPreprocessors
        {
            get
            {
                return _lessDomPreprocessors;
            }
            set
            {
                _lessDomPreprocessors = value;
            }
        }

        private static List<ICssDomPreprocessor> _cssDomPreprocessors = new List<ICssDomPreprocessor>{ new CssPropertyMerger()};
        public static List<ICssDomPreprocessor> CssDomPreprocessors
        {
            get
            {
                return _cssDomPreprocessors;
            }
            set
            {
                _cssDomPreprocessors = value;
            }
        }

        private static ICssBuilder _cssBuilder = new CssBuilder();
        public static ICssBuilder CssBuilder
        {
            get
            {
                return _cssBuilder;
            }
            set
            {
                _cssBuilder = value;
            }
        }
    }
}