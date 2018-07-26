using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Extras.DynamicProxy2;
using AutoMapper;
using DFC.Digital.Core;
using DFC.Digital.Core.Interceptors;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Repository.ONET.DataModel;
using DFC.Digital.Repository.ONET.Mapper;
using DFC.Digital.Repository.ONET.Query;

namespace DFC.Digital.Repository.ONET.Config
{
    public class SkillsFrameworkAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterAssemblyTypes(ThisAssembly)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(InstrumentationInterceptor.Name, ExceptionInterceptor.Name)
                ;

            builder.Register(ctx => new MapperConfiguration(cfg => cfg.AddProfile<SkillsFrameworkMapper>()))
                .InstancePerLifetimeScope();

            builder.Register(ctx => ctx.Resolve<MapperConfiguration>().CreateMapper())
                .As<IMapper>()
                .InstancePerLifetimeScope();

            builder.RegisterType<OnetSkillsFramework>()
                .InstancePerLifetimeScope()
                .OnActivated(e => e.Instance.Database.Log = message => e.Context.Resolve<IApplicationLogger>().Info(message));
        }
    }
}