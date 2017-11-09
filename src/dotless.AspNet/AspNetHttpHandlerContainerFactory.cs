using System.Net;
using dotless.Core.Abstractions;
using dotless.Core.configuration;
using dotless.Core.Parameters;
using dotless.Core.Response;
using Microsoft.Extensions.DependencyInjection;

namespace dotless.Core
{
    public class AspNetHttpHandlerContainerFactory : AspNetContainerFactory
    {
        protected override void RegisterParameterSource(IServiceCollection services, DotlessConfiguration configuration)
        {
            if (!configuration.DisableParameters)
            {
                services.AddTransient<IParameterSource, QueryStringParameterSource>();
            }
        }
    }
}