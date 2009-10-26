using System;
using System.IO;
using System.Text;
using nless.Core.engine;

namespace nLess.ParserRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length <2)
            {
                Console.WriteLine("Input and output files required");
                return;
            }

            var engine = new Engine(File.ReadAllText(args[0]), Console.Out);
            var path = args[1];


            if (!File.Exists(path))
                using(File.Create(path)){}

            using (var file = File.OpenWrite(path)){
                var bytes = Encoding.ASCII.GetBytes(engine.Parse().Css);
                file.Write(bytes, 0, bytes.Length);
            }

        }
    }
}
