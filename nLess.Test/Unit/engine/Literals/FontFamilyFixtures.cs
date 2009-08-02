using System;
using nless.Core.engine.nodes.Literals;
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
