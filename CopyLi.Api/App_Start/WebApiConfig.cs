using Autofac;
using Autofac.Integration.WebApi;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace CopyLi.Api.App_Start
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config, ILifetimeScope container)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            //config.Filters.Add(new AuthorizeAttribute());
            //config.Filters.Add(new FluentValidatorActionFilterAttribute());

            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }

}