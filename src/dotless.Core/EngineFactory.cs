namespace dotless.Core
{
    using configuration;

    public class EngineFactory
    {
        public DotlessConfiguration Configuration { get; set; }

        public EngineFactory(DotlessConfiguration configuration)
        {
            Configuration = configuration;
        }
        public EngineFactory() : this(DotlessConfiguration.GetDefault())
        {
        }

        public ILessEngine GetEngine()
        {
            var container = new ContainerFactory().GetContainer(Configuration);
            return container.GetInstance<ILessEngine>();
        }
    }
}