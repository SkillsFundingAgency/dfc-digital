using Autofac;
using Autofac.Extras.DynamicProxy2;
using Autofac.Integration.Mvc;
using DFC.Digital.Core.Interceptors;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterModule<AutofacWebTypesModule>();

            builder.RegisterAssemblyTypes(ThisAssembly).AsImplementedInterfaces()
               .EnableInterfaceInterceptors()
               .InterceptedBy(InstrumentationInterceptor.NAME, ExceptionInterceptor.NAME);
        }
    }
}