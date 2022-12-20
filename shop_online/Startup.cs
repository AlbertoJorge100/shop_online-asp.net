using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(shop_online.Startup))]
namespace shop_online
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
