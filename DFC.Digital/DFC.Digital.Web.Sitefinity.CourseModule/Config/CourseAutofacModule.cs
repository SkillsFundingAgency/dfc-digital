using Autofac;
using Autofac.Extras.DynamicProxy;
using Autofac.Integration.Mvc;
using DFC.Digital.Core;
using DFC.Digital.Core.Interceptors;
using DFC.FindACourseClient;
using DFC.FindACourseClient.Models.Configuration;
using System;
using System.Diagnostics.CodeAnalysis;

namespace DFC.Digital.Web.Sitefinity.CourseModule
{
    [ExcludeFromCodeCoverage]
    public class CourseAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterAssemblyTypes(ThisAssembly).AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name);

            builder.RegisterFindACourseClientSdk();
            builder.Register(c =>
            {
                var config = c.Resolve<IConfigurationProvider>();
                return new CourseSearchClientSettings
                {
                    CourseSearchAuditCosmosDbSettings = new CourseSearchAuditCosmosDbSettings
                    {
                        AccessKey = config.GetConfig<string>($"FAC.{nameof(CourseSearchAuditCosmosDbSettings)}.{nameof(CourseSearchAuditCosmosDbSettings.AccessKey)}"),
                        CollectionId = config.GetConfig<string>($"FAC.{nameof(CourseSearchAuditCosmosDbSettings)}.{nameof(CourseSearchAuditCosmosDbSettings.CollectionId)}"),
                        DatabaseId = config.GetConfig<string>($"FAC.{nameof(CourseSearchAuditCosmosDbSettings)}.{nameof(CourseSearchAuditCosmosDbSettings.DatabaseId)}"),
                        EndpointUrl = new Uri(config.GetConfig<string>($"FAC.{nameof(CourseSearchAuditCosmosDbSettings)}.{nameof(CourseSearchAuditCosmosDbSettings.EndpointUrl)}")),
                        Environment = config.GetConfig<string>($"FAC.{nameof(CourseSearchAuditCosmosDbSettings)}.{nameof(CourseSearchAuditCosmosDbSettings.Environment)}"),
                        PartitionKey = config.GetConfig<string>($"FAC.{nameof(CourseSearchAuditCosmosDbSettings)}.{nameof(CourseSearchAuditCosmosDbSettings.PartitionKey)}"),
                    },
                    CourseSearchSvcSettings = new CourseSearchSvcSettings
                    {
                        ApiKey = config.GetConfig<string>($"FAC.{nameof(CourseSearchSvcSettings)}.{nameof(CourseSearchSvcSettings.ApiKey)}"),
                        ServiceEndpoint = new Uri(config.GetConfig<string>($"FAC.{nameof(CourseSearchSvcSettings)}.{nameof(CourseSearchSvcSettings.ServiceEndpoint)}")),
                    },
                };
            });
            builder.RegisterType<CourseSearchLogger>().As<Microsoft.Extensions.Logging.ILogger>();

            // Note that ASP.NET MVC requests controllers by their concrete types,
            // so registering them As<IController>() is incorrect.
            // Also, if you register controllers manually and choose to specify
            // lifetimes, you must register them as InstancePerDependency() or
            // InstancePerHttpRequest() - ASP.NET MVC will throw an exception if
            // you try to reuse a controller instance for multiple requests.
            builder.RegisterControllers(ThisAssembly)
                   .InstancePerRequest()

                   //.EnableClassInterceptors()
                   ;

            // OPTIONAL: Register model binders that require DI.
            builder.RegisterModelBinders(ThisAssembly);
            builder.RegisterModelBinderProvider();
        }
    }
}