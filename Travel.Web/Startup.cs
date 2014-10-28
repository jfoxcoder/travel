using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Travel.Web.Startup))]
namespace Travel.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
