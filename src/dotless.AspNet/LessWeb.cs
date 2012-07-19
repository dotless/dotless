using dotless.Core.configuration;

namespace dotless.Core
{
    public static class LessWeb
    {
        public static string Parse(string less, DotlessConfiguration config)
        {
            return GetEngine(config).TransformToCss(less, null);
        }

        public static ILessEngine GetEngine(DotlessConfiguration config)
        {
            if (config == null)
            {
                config = new WebConfigConfigurationLoader().GetConfiguration();
            }
            return new EngineFactory(config).GetEngine(new AspNetContainerFactory());
        }
    }
}
