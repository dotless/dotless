namespace dotless.Test.Unit.ConsoleRunner
{
    using System;
    using System.IO;
    using Compiler;
    using NUnit.Framework;
    public class CompilerFixture
    {
        [Test]
        public void TransformsFileCorrectly()
        {
            string inputFile = @"Spec\ExtensibleEngine\less\variables.less";
            string outputFile = inputFile + ".css";
            if (File.Exists(outputFile))
                File.Delete(outputFile);
            string[] args = { inputFile };
            Program.Main(args);
            Assert.True(File.Exists(outputFile));
        }
    }
}