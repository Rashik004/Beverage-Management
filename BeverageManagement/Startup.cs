using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BeverageManagement.Startup))]
namespace BeverageManagement
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
