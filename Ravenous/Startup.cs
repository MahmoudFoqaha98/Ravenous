using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Ravenous.Startup))]
namespace Ravenous
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
