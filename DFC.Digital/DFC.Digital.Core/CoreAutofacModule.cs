using Autofac;
using Castle.DynamicProxy;
using DFC.Digital.Core.Interceptors;

namespace DFC.Digital.Core
{
    public class CoreAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterAssemblyTypes(ThisAssembly).AsImplementedInterfaces().InstancePerLifetimeScope();

            //It is necessary to have a singleton of this policy to ensure the policies works accross multiple requests.
            builder.RegisterType<TolerancePolicy>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<TransientFaultHandlingStrategy>().SingleInstance();

            //Single http client is better on performance - https://aspnetmonsters.com/2016/08/2016-08-27-httpclientwrong/
            builder.RegisterType<HttpClientService>().AsImplementedInterfaces().SingleInstance();

            //Register Interceptors
            builder.RegisterType<InstrumentationInterceptor>().AsSelf().Named<IInterceptor>(InstrumentationInterceptor.Name).InstancePerLifetimeScope();
            builder.RegisterType<ExceptionInterceptor>().AsSelf().Named<IInterceptor>(ExceptionInterceptor.Name).InstancePerLifetimeScope();
        }
    }
}