﻿using Autofac;
using Autofac.Extras.DynamicProxy;
using DFC.Digital.Core.Interceptors;

namespace DFC.Digital.Service.GovUkNotify.Config
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class GovUkNotifyAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterAssemblyTypes(ThisAssembly).AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name)
                ;
        }
    }
}