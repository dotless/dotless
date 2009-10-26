using System;
using System.IO;
using System.Linq;
using Peg.Base;
using Peg.Samples;

namespace dotless.GrammarGen
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Count()==0) throw new ArgumentException("arguments requires", "args");
            var input = args[0];
            var output = args[1];
            var src = File.ReadAllText(input);

            PegCharParser.Matcher startRule = (new PegGrammarParser()).peg_module;

            var pg = (PegCharParser)startRule.Target;
            pg.Construct(src, Console.Out);
            pg.SetSource(src);
            pg.SetErrorDestination(Console.Out);
            bool bMatches = startRule();

            var tree = ((PegCharParser)startRule.Target).GetRoot();

            IParserPostProcessor generator = new PegParserGenerator();
            var postProcParams = new ParserPostProcessParams(output, "", "", tree, src, Console.Out);
            generator.Postprocess(postProcParams);
            Console.ReadLine();

        }
    }
}