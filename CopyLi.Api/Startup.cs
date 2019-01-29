using System;
using System.Threading.Tasks;
using System.Web.Http;
using Autofac;
using CopyLi.Api.App_Start;
using Framework.Security.OAuth;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Owin;
using Swashbuckle.Application;

[assembly: OwinStartup(typeof(CopyLi.Api.Startup))]

namespace CopyLi.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ILifetimeScope container;
            HttpConfiguration httpConfig = new HttpConfiguration();
            AutofacConfig.Register(httpConfig, out container);
            WebApiConfig.Register(httpConfig, container);

            httpConfig.EnableSwagger(x => x.SingleApiVersion("v1", "CopyLi.Api")).EnableSwaggerUi();
            app.UseCors(CorsOptions.AllowAll);
            app.UseAuthorizationServer(container);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
            app.UseWebApi(httpConfig);
        }
    }

}
