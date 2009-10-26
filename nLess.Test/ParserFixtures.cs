using System;
using System.IO;
using NUnit.Framework;

namespace nLess.Test
{
    using dotless.Core.engine;

    //NOTE: This test is nonsense, just a quick  point in time proof
    [TestFixture]
    public class ParserFixtures
    {
        [Test]
        public void Can_Parse()
        {
            var engine = new Engine(File.ReadAllText(@"TestData/EngineRoom.less"), Console.Out);
            Console.Write(engine.Parse().Css);
        }
    }
}
