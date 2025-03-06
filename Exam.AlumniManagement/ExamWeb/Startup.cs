using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ExamWeb.Startup))]
namespace ExamWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
