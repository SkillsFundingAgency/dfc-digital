using Autofac;
using Autofac.Extras.NLog;
using AutoMapper;
using DFC.Digital.Core;
using DFC.Digital.Core.Configuration;
using DFC.Digital.Data.Interfaces; using DFC.Digital.Core;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Config;
using System;

namespace DFC.Digital.Service.AzureSearch.IntegrationTests.Config
{
    internal class AutofacIntegrationTestModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterModule<NLogModule>();
            builder.RegisterModule<CoreAutofacModule>();
            builder.RegisterType<JobProfileIntegrationTestIndex>().As<ISearchIndexConfig>();
            builder.Register(ctx =>
            {
                var inMemoryConfigProvider = new InMemoryConfigurationProvider();
                inMemoryConfigProvider.Add(TransientFaultHandlingStrategy.TimeoutKey, TimeSpan.FromSeconds(5));
                return inMemoryConfigProvider;
            }).SingleInstance();

            builder.Register(ctx => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<JobProfilesAutoMapperProfile>();
            }));

            builder.Register(ctx => ctx.Resolve<MapperConfiguration>().CreateMapper()).As<IMapper>();
        }
    }
}