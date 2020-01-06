using Autofac;
using Autofac.Extras.DynamicProxy;
using DFC.Digital.Core;
using DFC.Digital.Core.Interceptors;
using DFC.FindACourseClient;
using Microsoft.Extensions.Logging;
using System;

namespace DFC.Digital.Service.CourseSearchProvider
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class CourseSearchProviderAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterAssemblyTypes(ThisAssembly).AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name);

            builder.RegisterType<CourseDirectoryFaCApiService>().As<Data.Interfaces.ICourseSearchService>()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name);

            builder.RegisterType<LoggerFactory>().As<ILoggerFactory>().SingleInstance();
            builder.RegisterGeneric(typeof(CourseSearchLogger<>)).As(typeof(ILogger<>)).SingleInstance();
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
            })
            .AsSelf()
            .SingleInstance();
        }
    }
}