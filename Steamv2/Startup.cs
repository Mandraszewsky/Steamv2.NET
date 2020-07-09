using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Steamv2.Startup))]
namespace Steamv2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
