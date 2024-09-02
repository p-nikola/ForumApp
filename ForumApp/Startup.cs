using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ForumApp.Startup))]
namespace ForumApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
