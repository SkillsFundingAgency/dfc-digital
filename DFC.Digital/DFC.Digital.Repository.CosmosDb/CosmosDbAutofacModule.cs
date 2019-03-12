using Autofac;
using Autofac.Extras.DynamicProxy;
using DFC.Digital.Core.Interceptors;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System.Configuration;

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

            var endpoint = ConfigurationManager.AppSettings.Get("DFC.Digital.CourseSearchAudit.EndpointUrl");
            var key = ConfigurationManager.AppSettings.Get("DFC.Digital.CourseSearchAudit.PrimaryKey");

            builder.Register<IDocumentClient>(ctx => new DocumentClient(new System.Uri(endpoint), key)).SingleInstance();
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