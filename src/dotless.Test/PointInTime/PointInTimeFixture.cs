using System;
using System.IO;
using dotless.Core.engine;
using NUnit.Framework;

namespace dotless.Test.PointInTime
{
    /// <summary>
    /// NOTE: This test is nonsense, just a quick  point in time proof
    /// </summary>
    [TestFixture]
    public class PointInTimeFixture
    {
        [Test]
        public void Parse_Test_Data_And_Output_Tree()
        {
            var engine = new Engine(File.ReadAllText(@"PointInTime/TestData.less"), Console.Out);
            Console.Write(engine.Parse(true).Css);
        }
        [Test]
        public void Parse_Test_Data()
        {
            var engine = new Engine(File.ReadAllText(@"PointInTime/TestData.less"), Console.Out);
            Console.Write(engine.Parse().Css);
        }
    }
}