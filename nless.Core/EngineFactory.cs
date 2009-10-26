namespace nless.Core
{
    using configuration;

    public class EngineFactory
    {
        public ILessEngine GetEngine(DotlessConfiguration configuration)
        {
            ILessEngine engine = new LessEngine();
            if (configuration.MinifyOutput)
                engine = new MinifierDecorator(engine);
            return engine;
        }
    }
}