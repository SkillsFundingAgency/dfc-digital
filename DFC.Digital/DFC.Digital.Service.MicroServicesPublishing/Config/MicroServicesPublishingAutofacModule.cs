using Autofac;
using Autofac.Extras.DynamicProxy;
using DFC.Digital.Core;
using DFC.Digital.Core.Interceptors;

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

            builder.RegisterType<HttpClientService<IMicroServicesPublishingClientProxy>>()
              .AsImplementedInterfaces()
              .SingleInstance()
              .EnableInterfaceInterceptors()
              .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name);
        }
    }
}
