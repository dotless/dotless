using System.Net;
using dotless.Core.Abstractions;
using dotless.Core.configuration;
using dotless.Core.Parameters;
using dotless.Core.Response;
using Pandora.Fluent;

namespace dotless.Core {
    public class AspNetHttpHandlerContainerFactory : AspNetContainerFactory
    {
        protected override void RegisterParameterSource(FluentRegistration pandora, DotlessConfiguration configuration)
        {
            if (!configuration.DisableParameters)
            {
                pandora.Service<IParameterSource>().Implementor<QueryStringParameterSource>().Lifestyle.Transient();
            }
        }
    }
}