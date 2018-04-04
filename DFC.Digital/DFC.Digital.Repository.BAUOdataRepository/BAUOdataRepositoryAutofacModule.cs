using System.Configuration;
using Autofac;
using Autofac.Extras.DynamicProxy2;
using DFC.Digital.Core.Interceptors;

namespace DFC.Digital.Repository.BAUOdataRepository
{
    public class BAUOdataRepositoryAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterAssemblyTypes(ThisAssembly)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name);
        }
    }
}