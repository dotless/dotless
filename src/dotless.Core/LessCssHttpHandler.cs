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

using System.Web.Caching;

namespace dotless.Core
{
    using System.Web;
    using Abstractions;
    using configuration;

    public class LessCssHttpHandler : IHttpHandler
    {
        private readonly EngineFactory engineFactory = new EngineFactory();

        public void ProcessRequest(HttpContext context)
        {
            var config = ConfigurationLoader.GetConfigurationFromWebconfig();
            var engine = engineFactory.GetEngine(config);


            var cache = new CssCache(context.Cache);
            var provider = new PathProvider(context.Server);
            var request = new Request(context.Request);
            var response = new CssResponse(context.Response);

            var handler = new HandlerImpl();
            handler.Execute(cache, provider, request, response, config, engine);

            // our unprocessed filename   
            var lessFile = context.Server.MapPath(context.Request.Url.LocalPath);

            //context.Response.AddFileDependency(lessFile);
            context.Response.Cache.SetCacheability(HttpCacheability.Public);    //Anyone can cache this

            //TODO: Clean up this code. Seperate concerns here
            context.Response.ContentType = "text/css";
            if (context.Cache[lessFile] == null)
            {
                string css = engine.TransformToCss(lessFile);
                if (config.CacheEnabled)
                    context.Cache.Insert(lessFile, css, new CacheDependency(lessFile));
            }
            context.Response.Write(context.Cache[lessFile]);
            context.Response.End();

            
        }

        public bool IsReusable
        {
            get { return true; }
        }
    }
}