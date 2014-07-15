using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(VoltaAoMundo.Startup))]
namespace VoltaAoMundo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
