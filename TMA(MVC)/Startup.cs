using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TMA_MVC_.Startup))]
namespace TMA_MVC_
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
