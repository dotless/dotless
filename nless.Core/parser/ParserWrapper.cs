#region

using System;
using System.IO;
using nLess;
using nless.Core.engine;
using Peg.Base;

#endregion

namespace nless.Core.parser
{
    internal static class IntParseExtentions
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
                var tw = new StreamWriter(File.OpenWrite("out.txt"));
                Console.WriteLine("SUCCESS: Json Parser matched input file");
                var root = parser.GetRoot();
                try
                {
                    var walker = new TreeWalker(root, src);
                    var nLessRoot = walker.Walk();
                    //Console.WriteLine();

                    using(tw)
                    {
                        tw.Write(nLessRoot.ToCss());
                    }
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex);
                }
           

                

                //var tprint = new TreePrint(tw, src, 60, new NodePrinter(parser).GetNodeName, false);
                //tprint.PrintTree(parser.GetRoot(), 0, 0);
            }
        }
    }

    internal class NodePrinter
    {
        private readonly PegBaseParser parser_;

        internal NodePrinter(PegBaseParser parser)
        {
            parser_ = parser;
        }

        internal string GetNodeName(PegNode n)
        {
            return parser_.GetRuleNameFromId(n.id_);
        }
    }

    internal class ParsingException : Exception
    {
        public ParsingException(string s) : base(s)
        {
        }
    }
}