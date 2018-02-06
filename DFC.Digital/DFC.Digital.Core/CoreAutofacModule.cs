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
            builder.RegisterAssemblyTypes(ThisAssembly).AsImplementedInterfaces();

            //Register Interceptors
            builder.RegisterType<InstrumentationInterceptor>().AsSelf().Named<IInterceptor>(InstrumentationInterceptor.NAME);
            builder.RegisterType<ExceptionInterceptor>().AsSelf().Named<IInterceptor>(ExceptionInterceptor.NAME);
        }
    }
}