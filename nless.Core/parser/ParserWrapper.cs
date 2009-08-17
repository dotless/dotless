using System;
using System.Collections.Generic;
using System.IO;
using nLess;
using nless.Core.engine;
using Peg.Base;

namespace nless.Core.parser
{
    static class IntParseExtentions
    {
        internal static EnLess ToEnLess(this int i)
        {
            return (EnLess) i;
        }
    }

    public static class ParserWrapper
    {
        //Hack until I can work out how to retrieve return from the parser
        public static Element Env { get; set; }
        public static void Parse(string src, TextWriter errorOut)
        {
            var parser = new nLess.nLess(src, errorOut);
            var bMatches = parser.Parse();

            if (!bMatches)
            {
                Console.WriteLine("FAILURE: Json Parser did not match input file ");
            }
            else
            {
                Console.WriteLine("SUCCESS: Json Parser matched input file");
                var root = parser.GetRoot();
                var walker = new TreeWalker(root, src);
                var nLessRoot = walker.Walk();
                Console.WriteLine(nLessRoot.ToCss());


                var tprint = new TreePrint(Console.Out, src, 60, new NodePrinter(parser).GetNodeName, false);
                tprint.PrintTree(parser.GetRoot(), 0, 0);


            }
        }
    }
    class NodePrinter
    {
        internal NodePrinter(PegBaseParser parser)
        {
            parser_ = parser;
        }
        internal string GetNodeName(PegNode n)
        {
            return parser_.GetRuleNameFromId(n.id_);
        }
        PegBaseParser parser_;
    }

    public class TreeWalker
    {
        public PegNode Root { get; set; }
        public string Src { get; set; }


        public TreeWalker(PegNode root, string src)
        {
            Root = root;
            Src = src;
        }
        public Element Walk()
        {
            var el = new Element("");
            return (Element)Eval(Root, el);
        }
        private INode Eval(PegNode node, INode lessNode)
        {
            var val = node.GetAsString(Src);
            
            switch(node.id_.ToEnLess())
            {
                case EnLess.Parse:
                    Eval(node.child_,lessNode);
                    INode res = null; 
                    for (node = node.child_; node != null; node = node.next_){
                        res = Eval(node, res);
                    }
                    Console.WriteLine("-->{0}<--", res);
                    break;
                case EnLess.declaration:
                    val = node.child_.GetAsString(Src);
                    if(lessNode is Element)
                    {
                        switch ((EnLess) node.child_.id_)
                        {
                            case EnLess.variable:
                                var variable = new Variable(val, Expressions(node.child_));
                                ((Element)lessNode).Add(variable);
                                return variable;
                                break;
                            case EnLess.ident:
                                var property = new Property(val, Expressions(node.child_));
                                ((Element)lessNode).Add(property);
                                return property;
                                break;
                        }
                    }

                    break;
            }
            return lessNode;
        }

        private IEnumerable<INode> Expressions(PegNode node)
        {
            var expressions = node.id_.ToEnLess();
            INode res = null;
            for (node = node; node != null; node = node.next_)
            {
                Console.WriteLine(string.Format("{0} : {1}",node.id_.ToEnLess(), node.GetAsString(Src)));
            }
            Console.WriteLine("-->{0}<--", res);
            return new List<INode>();
            
        }
        //private static INode Expression(PegNode node)
        //{
        //    return new List<INode>();
        //}
    }
}
