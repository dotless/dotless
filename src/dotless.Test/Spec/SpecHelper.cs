namespace dotless.Test.Spec
{
    using System.IO;
    using Core.engine;
    using NUnit.Framework;

    public class SpecHelper
    {
        public static string Lessify(string fileName)
        {
            var file = Path.Combine("Spec/less", fileName + ".less");
            return new Engine(File.ReadAllText(file)).Parse().Css.Replace("\r\n", "\n");
        }
        public static string Css(string fileName)
        {
            var file = Path.Combine("Spec/css", fileName + ".css");
            return File.ReadAllText(file).Replace("\r\n", "\n");
        }

        public static void ShouldEqual(string filename)
        {
            var less = Lessify(filename);
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