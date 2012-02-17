namespace dotless.Compiler
{
    using System;
    using Core.configuration;

    internal class CompilerConfiguration : DotlessConfiguration
    {
        public CompilerConfiguration(DotlessConfiguration config) : base(config)
        {
            CacheEnabled = false;
            Web = false;
            Watch = false;
        }

        public bool Watch { get; set; }
        public bool Help { get; set; }
    }
}