﻿using Autofac;
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
            builder.RegisterAssemblyTypes(ThisAssembly).AsImplementedInterfaces()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.NAME, ExceptionInterceptor.NAME);

            builder.RegisterType<SearchIndexEnhancers.JobProfileIndexEnhancer>().As<IJobProfileIndexEnhancer>();
        }
    }
}