using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TaskMgnt_MVC_.Startup))]
namespace TaskMgnt_MVC_
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
