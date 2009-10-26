namespace dotless.Test.Unit.engine.Literals
{
    using Core.engine;
    using System;
    using NUnit.Framework;

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