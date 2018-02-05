using Autofac;
using Autofac.Extras.DynamicProxy2;
using DFC.Digital.Core.Interceptors;
using DFC.Digital.Data.Interfaces;

namespace DFC.Digital.Web.Sitefinity.DfcSearchModule
{
    public class DfcSearchModuleAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterAssemblyTypes().AsImplementedInterfaces()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name);

            builder.RegisterType<SearchIndexEnhancers.JobProfileIndexEnhancer>().As<IJobProfileIndexEnhancer>();
        }
    }
}