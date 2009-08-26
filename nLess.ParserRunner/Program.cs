using System;
using nless.Core.engine;

namespace nLess.ParserRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            var engine = new Engine(args[0], Console.Out);
            Console.Write(engine.Less);
        }
    }
}
