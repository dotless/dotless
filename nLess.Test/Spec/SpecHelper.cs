using System;
using System.IO;
using nless.Core.engine;

namespace nLess.Test.Spec
{
    public class SpecHelper
    {
        public static string Lessify(string fileName)
        {
            var file = Path.Combine("Spec/less", fileName + ".less");
            return new Engine(File.ReadAllText(file)).Parse().Css;
        }
        public static string Css(string fileName)
        {
            var file = Path.Combine("Spec/css", fileName + ".css");
            return File.ReadAllText(file);
        }

        public static bool ShouldEqual(string filename)
        {
            var less = Lessify(filename);
            Console.WriteLine(less);
            return less.ShouldEqual(Css(filename));
        }
    }
}