using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BullardEncuestas.Startup))]
namespace BullardEncuestas
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
