/*Author:Martin.Holzherr;Date:20080922;Context:"PEG Support for C#";Licence:CPOL
 * <<History>> 
 *  20080922;V1.0 created
 * <</History>>
*/
using System;
using Peg.Base;
using System.IO;
using System.Diagnostics;

namespace PEG_Console_Parser
{

    
    class SamplePegConsoleParser
    {
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
        static int Main(string[] args)
        {
            if (args.Length < 1 || !File.Exists(args[0]))
            {
                Console.WriteLine("FATAL: First command line parameter must be an existing file");
                return -1;
            }
            try
            {
                FileLoader loader = new FileLoader(json_tree.json_tree.encodingClass,json_tree.json_tree.unicodeDetection, args[0]);
                Debug.Assert(!loader.IsBinaryFile());
                string src;
                if (loader.LoadFile(out src))
                {
                    json_tree.json_tree jsonParser = new json_tree.json_tree(src, Console.Out);
                    bool bMatches = jsonParser.json_text();
                    if (bMatches)
                    {
                        Console.WriteLine("SUCCESS: Json Parser matched input file '{0}'", args[0]);
                        TreePrint tprint = new TreePrint(Console.Out, src, 60, new NodePrinter(jsonParser).GetNodeName, false);
                        tprint.PrintTree(jsonParser.GetRoot(), 0, 0);
                    }
                    else
                    {
                        Console.WriteLine("FAILURE: Json Parser did not match input file '{0]'", args[0]);
                    }
                    return 0;
                }
                else
                {
                    Console.WriteLine("FATAL: File '{0}' could not be loaded", args[0]);
                    return -1;
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine("FATAL: Program terminated by exception '{0}'",e.Message);
                return -1;
            }
        }
    }
}
