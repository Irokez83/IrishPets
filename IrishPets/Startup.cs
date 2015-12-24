using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IrishPets.Startup))]
namespace IrishPets
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}