namespace dotless.Test
{
    using Core.engine;
    using System;
    using System.IO;
    using NUnit.Framework;

    [TestFixture]
    public class ParserFixtures
    {
        //NOTE: This test is nonsense, just a quick  point in time proof
        [Test]
        public void Can_Parse()
        {
            var engine = new Engine(File.ReadAllText(@"TestData/EngineRoom.less"), Console.Out);
            Console.Write(engine.Parse(true).Css);
        }
    }
}