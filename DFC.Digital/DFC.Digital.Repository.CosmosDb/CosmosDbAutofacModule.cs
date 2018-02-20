using Autofac;
using Autofac.Extras.DynamicProxy2;
using DFC.Digital.Core.Interceptors;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System.Configuration;

namespace DFC.Digital.Repository.CosmosDb
{
    public class CosmosDbAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterAssemblyTypes(ThisAssembly).AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.NAME, ExceptionInterceptor.NAME);

            var endpoint = ConfigurationManager.AppSettings.Get("DFC.Digital.CourseSearchAudit.EndpointUrl");
            var key = ConfigurationManager.AppSettings.Get("DFC.Digital.CourseSearchAudit.PrimaryKey");

            builder.Register<IDocumentClient>(ctx => new DocumentClient(new System.Uri(endpoint), key));
        }
    }
}