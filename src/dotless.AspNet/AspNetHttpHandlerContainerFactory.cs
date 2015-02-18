using dotless.Core.Abstractions;
using dotless.Core.configuration;
using dotless.Core.Parameters;
using dotless.Core.Response;
using Pandora.Fluent;

namespace dotless.Core {
    public class AspNetHttpHandlerContainerFactory : AspNetContainerFactory
    {
        protected override void RegisterServices(FluentRegistration pandora, DotlessConfiguration configuration)
        {
            base.RegisterServices(pandora, configuration);

            if (!configuration.DisableParameters)
            {
                pandora.Service<IParameterSource>().Implementor<QueryStringParameterSource>().Lifestyle.Transient();
            }
        }
    }
}