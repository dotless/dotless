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
    using System.Web.Caching;
    using Abstractions;
    using configuration;
    using Microsoft.Practices.ServiceLocation;
    using Pandora;

    public class LessCssHttpHandler : IHttpHandler
    {
        private readonly EngineFactory _engineFactory = new EngineFactory();

        public void ProcessRequest(HttpContext context)
        {
            var config = ConfigurationLoader.GetConfigurationFromWebconfig();
            var engine = _engineFactory.GetEngine(config);
            var cache = new CssCache(context.Cache);
            var provider = new PathProvider(context.Server);
            var request = new Request(context.Request);
            var response = new CssResponse(context.Response);
            var handler = new HandlerImpl();
            handler.Execute(cache, provider, request, response, config, engine);
        }

        public bool IsReusable
        {
            get { return true; }
        }
    }

    public class ContainerFactory
    {
        public IServiceLocator GetContainer()
        {
            var container = new PandoraContainer();
            //ASP.NET Services
            container.Register(p => {
                                        p.Service<Cache>()
                                            .Instance(HttpContext.Current.Cache);
                                        p.Service<HttpServerUtility>()
                                            .Instance(HttpContext.Current.Server);
                                        p.Service<HttpRequest>()
                                            .Instance(HttpContext.Current.Request);
                                        p.Service<HttpResponse>()
                                            .Instance(HttpContext.Current.Response);
            });
            container.Register(p =>
                                   {
                                       p.Service<ICache>()
                                           .Implementor<CssCache>();
                                       p.Service<IPathProvider>()
                                           .Implementor<PathProvider>();
                                       p.Service<IRequest>()
                                           .Implementor<Request>();
                                       p.Service<IResponse>()
                                           .Implementor<CssResponse>();
                                   });
            return new CommonServiceLocatorAdapter(container);
        }
    }
}