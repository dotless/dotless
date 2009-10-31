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

namespace dotless.Core.engine
{
    using System;
    using System.IO;
    using Peg.Base;

    using exceptions;
    using parser;

    public class Engine
    {
        private readonly nLess.nLess _parser;  
        private string css = "";
        public string Css
        {
            get
            {
                return css;
            }
        }
        private string less = "";
        public string Less
        {
            get
            {
                return less;
            }
        }

        internal Element Root;
        
        public Engine(string less) : this(less, Console.Out){}
        public Engine(string less, TextWriter errorOut)
        {
            this.less = less;
            _parser = new nLess.nLess(less, errorOut);
        }
        public Engine Parse()
        {
            return Parse(false);
        }
        public Engine Parse(bool showTree)
        {
            return Parse(showTree, new Element());
        }
        internal Engine Parse(bool showTree, Element env)
        {
            var matches = _parser.Parse();
            if(!matches) throw new ParsingException("FAILURE: Parser did not match input file");
            var pegEl = _parser.GetRoot();
            var treeWalker = new TreeBuilder(pegEl, less);
            Root = treeWalker.Build();
            css = Root.Group().ToCss();
            if (showTree)
            {
                var tprint = new TreePrint(Console.Out, less, 60, new NodePrinter(_parser).GetNodeName, false);
                tprint.PrintTree(_parser.GetRoot(), 0, 0);
            }
            return this;
        }
    }
}