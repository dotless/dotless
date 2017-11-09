namespace dotless.Core
{
    using configuration;
    using Microsoft.Extensions.DependencyInjection;

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
            return GetEngine(new ContainerFactory());
        }

        public ILessEngine GetEngine(ContainerFactory containerFactory)
        {
            var container = containerFactory.GetContainer(Configuration);
            return container.GetRequiredService<ILessEngine>();
        }
    }
}