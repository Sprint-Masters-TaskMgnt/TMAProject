using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TMA_MVC.Startup))]
namespace TMA_MVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
