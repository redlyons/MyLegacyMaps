using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MyLegacyMaps.Startup))]
namespace MyLegacyMaps
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
