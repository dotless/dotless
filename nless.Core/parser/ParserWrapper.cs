#region

using System;
using System.IO;
using nLess;
using nless.Core.engine;
using nless.Core.Exceptions;
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

        public static INode Parse(string src)
        {
            return Parse(src, Console.Out);
        }

        public static INode Parse(string src, TextWriter errorOut)
        {
            Element nLessRootNode = null;
            
            var bMatches = parser.Parse();

            if (!bMatches)
            {
                throw new ParsingException("FAILURE: Json Parser did not match input file ");
            }
            else
            {
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
            }

            return nLessRootNode;
        }
    }
}