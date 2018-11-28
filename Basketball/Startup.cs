using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Basketball.Startup))]
namespace Basketball
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
