using System;
using nless.Core.engine;
using NUnit.Framework;

namespace nLess.Test.Unit.engine.Literals
{
    [TestFixture]
    public class FontFamilyFixture
    {
        [Test]
        public void CanGetCss()
        {
            var fontFamily = new FontFamily("Arial", "\"Summit\"");
            Console.WriteLine(fontFamily.ToCss());
        }

        
    }
}
