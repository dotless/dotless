using System;
using System.IO;
using nless.Core.parser;

namespace nLess.ParserRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            ParserWrapper.Parse(File.ReadAllText(args[0]), Console.Out);
            Console.ReadLine();
        }
    }
}
