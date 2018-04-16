using Autofac;
using Autofac.Extras.DynamicProxy2;
using DFC.Digital.Core;
using DFC.Digital.Core.Interceptors;

namespace DFC.Digital.Service.LMIFeed
{
    public class LmiFeedAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterAssemblyTypes(ThisAssembly).AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name)
                ;

            builder.RegisterType<HttpClientService<IAsheHttpClientProxy>>()
                .AsImplementedInterfaces()
                .SingleInstance()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name)
                ;
        }
    }
}