/* Copyright 2009 dotless project, http://www.dotlesscss.com
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 *     
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License. */

namespace dotless.Core
{
    using System.Web;
    using Abstractions;
    using configuration;
    using Loggers;
    using Microsoft.Practices.ServiceLocation;
    using Pandora;
    using Stylizers;

    public class ContainerFactory
    {
        private PandoraContainer Container { get; set; }

        public IServiceLocator GetContainer(DotlessConfiguration configuration)
        {
            Container = new PandoraContainer();

            RegisterServices(configuration);

            return new CommonServiceLocatorAdapter(Container);
        }

        private void RegisterServices(DotlessConfiguration configuration)
        {
            Container.Register(p =>
                                   {
                                       if (configuration.Web)
                                       {
                                           p.Service<HttpContextBase>().Implementor<HttpContextWrapper>().Parameters("httpContext").Set("httpContext.Current");
                                           p.Service<HttpContext>("httpContext.Current").Instance(HttpContext.Current);

                                           if (configuration.CacheEnabled)
                                               p.Service<IResponse>().Implementor<CachedCssResponse>();
                                           else
                                               p.Service<IResponse>().Implementor<CssResponse>();

                                           p.Service<ICache>().Implementor<HttpCache>();
                                           p.Service<ILogger>().Implementor<AspResponseLogger>().Parameters("level").Set("error-level");
                                           p.Service<IPathResolver>().Implementor<AspServerPathResolver>();
                                       }
                                       else
                                       {
                                           p.Service<ICache>().Implementor<InMemoryCache>();
                                           p.Service<ILogger>().Implementor<ConsoleLogger>().Parameters("level").Set("error-level");
                                           p.Service<IPathResolver>().Implementor<RelativePathResolver>();
                                       }

                                       p.Service<LogLevel>("error-level").Instance(LogLevel.Error);
                                       p.Service<IStylizer>().Implementor<PlainStylizer>();

                                       p.Service<Parser.Parser>().Implementor<Parser.Parser>().Parameters("optimization").Set("default-optimization");
                                       p.Service<int>("default-optimization").Instance(2);

                                       if (configuration.MinifyOutput)
                                           p.Service<ILessEngine>().Implementor<MinifierDecorator>();

                                       if (configuration.CacheEnabled)
                                            p.Service<ILessEngine>().Implementor<CacheDecorator>();

                                       p.Service<ILessEngine>().Implementor<LessEngine>();
                                       p.Service<IFileReader>().Implementor(configuration.LessSource);
                                   });
        }
    }
}