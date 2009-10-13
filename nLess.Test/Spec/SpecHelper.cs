using System;
using System.IO;
using nless.Core.engine;
using NUnit.Framework;

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

        public static void ShouldEqual(string filename)
        {
            var less = Lessify(filename);
            //Console.WriteLine(less);
            var css = Css(filename);
            css.ShouldEqual(less, string.Format("|{0}| != |{1}|", less, css));
        }
    }
    internal static class SpecExtensions
    {
        public static void ShouldEqual(this string a, string b, string assertionFailedMessage)
        {
            Assert.AreEqual(a.ToLower(), b.ToLower());
        }
    }
}