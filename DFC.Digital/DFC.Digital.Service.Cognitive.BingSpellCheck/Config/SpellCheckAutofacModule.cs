using Autofac;
using Autofac.Extras.DynamicProxy2;
using DFC.Digital.Core;
using DFC.Digital.Core.Interceptors;
using DFC.Digital.Data.Interfaces;

namespace DFC.Digital.Service.Cognitive.BingSpellCheck.Config
{
    public class SpellCheckAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterAssemblyTypes(ThisAssembly).AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name)
                ;

            builder.RegisterType<HttpClientService<ISpellCheckService>>().AsImplementedInterfaces().SingleInstance();
        }
    }
}