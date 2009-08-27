using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using nless.Core.Exceptions;
using nless.Core.parser;
using Peg.Base;

namespace nless.Core.engine
{
    public class Engine
    {
        private nLess.nLess _parser;  
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
        
        public Engine(string less) : this(less, Console.Out){}
        public Engine(string less, TextWriter errorOut)
        {
            this.less = less;
            _parser = new nLess.nLess(less, errorOut);
        }
        public Engine Parse()
        {
            return Parse(true, new Element());
        }
        public Engine Parse(bool build, Element env)
        {
            var matches = _parser.Parse();
            if(!matches) throw new ParsingException("FAILURE: Parser did not match input file");
            var pegEl = _parser.GetRoot();
            var treeWalker = new TreeBuilder(pegEl, less);
            var rootElement = treeWalker.Build();
            css = rootElement.ToCss();
            var tprint = new TreePrint(Console.Out, less, 60, new NodePrinter(_parser).GetNodeName, false);
            tprint.PrintTree(_parser.GetRoot(), 0, 0);
            return this;
        }
    }
}
