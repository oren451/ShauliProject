using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ShauliProject.Startup))]
namespace ShauliProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
