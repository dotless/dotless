using System;
using NUnit.Framework;

namespace nLess.Test.Unit.engine.Literals
{
    using dotless.Core.engine;

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
