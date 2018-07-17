using Autofac;
using Autofac.Extras.DynamicProxy2;
using DFC.Digital.Core.Interceptors;
using DFC.Digital.Service.OnetService.Services;

namespace DFC.Digital.Service.GovUkNotify.Config
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class OnetServiceAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterAssemblyTypes(ThisAssembly).AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name)
                ;
            builder.RegisterType<InMemoryReportAuditRepository>()
                .AsImplementedInterfaces()
                .InstancePerRequest()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name)
                ;
        }
    }
}