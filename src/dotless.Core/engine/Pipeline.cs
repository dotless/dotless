using System;
using System.Collections.Generic;
using dotless.Core.parser;

namespace dotless.Core.engine
{
    /// <summary>
    /// Pipeline factory to grab the different parts of required engine process. 
    /// </summary>
    public static class Pipeline
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

        private static List<ICssDomPreprocessor> _cssDomPreprocessors = new List<ICssDomPreprocessor>();
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