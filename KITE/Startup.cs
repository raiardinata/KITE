using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(KITE.Startup))]
namespace KITE
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
