using NUnit.Framework;

namespace dotless.Test.Spec
{
    internal static class SpecExtensions
    {
        public static void ShouldEqual(this string a, string b, string assertionFailedMessage)
        {
            Assert.AreEqual(a.ToLower(), b.ToLower());
        }
    }
}