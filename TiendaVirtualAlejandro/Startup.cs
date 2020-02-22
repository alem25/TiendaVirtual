using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TiendaVirtualAlejandro.Startup))]
namespace TiendaVirtualAlejandro
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
