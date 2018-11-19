using Autofac;
using Castle.DynamicProxy;
using DFC.Digital.Core.Configuration;
using DFC.Digital.Core.Interceptors;

namespace DFC.Digital.Core
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class CoreAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterAssemblyTypes(ThisAssembly).AsImplementedInterfaces().InstancePerLifetimeScope();

            //It is necessary to have a singleton of this policy to ensure the policies works accross multiple requests.
            builder.RegisterType<TolerancePolicy>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<AppConfigConfigurationProvider>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<TransientFaultHandlingStrategy>().SingleInstance();
            builder.RegisterType<CachePolicy>().AsImplementedInterfaces().SingleInstance();

            //Register Interceptors
            builder.RegisterType<InstrumentationInterceptor>().AsSelf().Named<IInterceptor>(InstrumentationInterceptor.Name).InstancePerLifetimeScope();
            builder.RegisterType<ExceptionInterceptor>().AsSelf().Named<IInterceptor>(ExceptionInterceptor.Name).InstancePerLifetimeScope();
        }
    }
}