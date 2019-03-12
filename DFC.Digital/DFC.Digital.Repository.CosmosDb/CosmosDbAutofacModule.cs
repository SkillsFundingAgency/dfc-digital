using Autofac;
using Autofac.Extras.DynamicProxy;
using DFC.Digital.Core;
using DFC.Digital.Core.Interceptors;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace DFC.Digital.Repository.CosmosDb
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class CosmosDbAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterAssemblyTypes(ThisAssembly)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name);

            builder.Register<IDocumentClient>(
                ctx => new DocumentClient(
                    new System.Uri(ctx.Resolve<IConfigurationProvider>().GetConfig<string>(Constants.CosmosDbEndPoint)), ctx.Resolve<IConfigurationProvider>().GetConfig<string>(Constants.CosmosDbEndPointPrimaryKey)))
                    .SingleInstance();

            builder.RegisterType<CourseSearchAuditRepository>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .OnActivating(cosmos => cosmos.Instance.Initialise())
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name)
                ;

            builder.RegisterType<EmailAuditRepository>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .OnActivating(cosmos => cosmos.Instance.Initialise())
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name)
                ;
        }
    }
}