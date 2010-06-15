namespace dotless.Test.Specs.Compression
{
    using Core.Parser.Infrastructure;

    public class CompressedSpecFixtureBase : SpecFixtureBase
    {
        public CompressedSpecFixtureBase()
        {
            DefaultEnv = () => new Env { Compress = true };
        }
    }
}