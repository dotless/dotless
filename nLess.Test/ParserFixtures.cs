using System;
using System.IO;
using nless.Core.parser;
using NUnit.Framework;

namespace nLess.Test
{

    //TODO: This test is nonsense, just a quick  point in time proof
    [TestFixture]
    public class ParserFixtures
    {
        [Test]
        public void Can_Parse()
        {
            ParserWrapper.Parse(File.ReadAllText(@"D:\Code\nLess\nLess.Test\Test.css"), Console.Out);
        }
    }
}
