using System;
using System.IO;
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
