namespace dotless.Test.Specs.Compression
{
    using Core.Parser.Infrastructure;
    using NUnit.Framework;

    public class CompressedSpecFixtureBase : SpecFixtureBase
    {
        [SetUp]
        public void Setup()
        {
            DefaultEnv = () => new Env { Compress = true };
        }
    }
}