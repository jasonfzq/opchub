using System.Web.Http;
using Owin;

namespace OpcHub.Ae.Service
{
    public class Startup
    {
        /// <summary>
        /// https://github.com/dcomartin/AspNetSelfHostDemo/blob/master/src/Startup.cs
        /// https://docs.microsoft.com/en-us/aspnet/web-api/overview/advanced/calling-a-web-api-from-a-net-client
        /// </summary>
        /// <param name="app"></param>
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            app.UseWebApi(config);
        }
    }
}
