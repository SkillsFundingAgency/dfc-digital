﻿using Autofac;
using Autofac.Extras.DynamicProxy2;
using DFC.Digital.Core.Interceptors;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Web.Sitefinity.Core;
using DFC.Digital.Web.Sitefinity.Core.Interface;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<WebAppContext>().As<IWebAppContext>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.NAME, ExceptionInterceptor.NAME);

            builder.RegisterType<SitefinityCurrentContext>().As<ISitefinityCurrentContext>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.NAME, ExceptionInterceptor.NAME);

            builder.RegisterType<SitefinityPage>().As<ISitefinityPage>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.NAME, ExceptionInterceptor.NAME);

            builder.RegisterType<JobProfilePageContentService>().As<IJobProfilePage>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.NAME, ExceptionInterceptor.NAME);
        }
    }
}