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

        public static INode Parse(string src, TextWriter errorOut)
        {
            Element nLessRootNode = null;
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
                    nLessRootNode = walker.Walk();
                    Console.WriteLine(nLessRootNode.ToCss());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

                var tprint = new TreePrint(Console.Out, src, 60, new NodePrinter(parser).GetNodeName, false);
                tprint.PrintTree(parser.GetRoot(), 0, 0);
            }

            return nLessRootNode;
        }
    }
}