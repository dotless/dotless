using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(dotless.SampleWeb.Startup))]
namespace dotless.SampleWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {

        }
    }
}
