using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PZ.Startup))]
namespace PZ
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
