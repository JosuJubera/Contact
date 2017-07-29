using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Contact.Startup))]
namespace Contact
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
