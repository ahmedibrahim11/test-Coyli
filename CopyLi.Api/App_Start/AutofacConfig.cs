using Autofac;
using Autofac.Integration.WebApi;
using AutoMapper;
using CopyLi.Data;
using CopyLi.Services.OAuth.Server;
using CopyLi.Utilites.AutoMapper;
using CopyLi.Utilites.Cryptography;
using Framework.Data.EF;
using Framework.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;

namespace CopyLi.Api.App_Start
{
    public static class AutofacConfig
    {
        public static void Register(HttpConfiguration httpConfig, out ILifetimeScope lifetimeScope)
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterHttpRequestMessage(httpConfig);

            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).InstancePerRequest();
            builder.RegisterType<UnitOfWorkFactory>().As<IUnitOfWorkFactory>().InstancePerRequest();
            builder.RegisterType<DatabaseContext>().As<DbContext>().InstancePerRequest();

            builder.RegisterType<EncrptionService>().As<IEncryptionService>().SingleInstance();

            builder.RegisterOAuth(true);
            builder.RegisterOAuthApplicationProvider<ApplicationProvider>();
            builder.RegisterOAuthUserValidator<PasswordUserValidator>("password");
            builder.RegisterOAuthUserValidator<RegisterUserValidator>("register");

            BuildAutoMappers(builder);

            lifetimeScope = builder.Build();
        }


        private static void BuildAutoMappers(ContainerBuilder builder)
        {
            var mapper = new MapperConfiguration(mapperConfig =>
            {
                var methodsToCall = from type in typeof(AutofacConfig).Assembly.GetTypes()
                                    where type.IsClass && !type.IsAbstract &&
                                          type.GetCustomAttribute<MapperAttribute>() != null
                                    select type.GetMethod(MapperAttribute.METHOD_NAME);
                foreach (var method in methodsToCall)
                    method.Invoke(null, new object[] { mapperConfig });
            });
            builder.RegisterInstance(mapper).As<IConfigurationProvider>();
            builder.RegisterInstance(mapper.CreateMapper()).As<IMapper>();
        }
    }

}