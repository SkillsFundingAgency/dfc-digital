using Autofac;
using Autofac.Extras.DynamicProxy;
using DFC.Digital.Core;
using DFC.Digital.Core.Interceptors;
using DFC.Digital.Data.Interfaces;

namespace DFC.Digital.Service.MicroServicesPublishing
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class MicroServicesPublishingAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterAssemblyTypes(ThisAssembly).AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name);

            builder.RegisterType<HttpClientService<IMicroServicesPublishingService>>()
              .AsImplementedInterfaces()
              .SingleInstance()
              .EnableInterfaceInterceptors()
              .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name);
        }
    }
}
