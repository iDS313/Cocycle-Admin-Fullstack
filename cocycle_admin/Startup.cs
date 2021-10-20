using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(cocycle_admin.Startup))]
namespace cocycle_admin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
