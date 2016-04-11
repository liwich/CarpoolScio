using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(carpool4Service.Startup))]

namespace carpool4Service
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}