namespace dotless.Core
{
    using configuration;

    public static class Less
    {
        public static string Parse(string less)
        {
            return Parse(less, DotlessConfiguration.GetDefault());
        }

        public static string Parse(string less, DotlessConfiguration config)
        {
            return new EngineFactory(config).GetEngine().TransformToCss(less, null);
        }
    }
}
