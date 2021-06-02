using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ShopIn.WebUI.Startup))]
namespace ShopIn.WebUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
