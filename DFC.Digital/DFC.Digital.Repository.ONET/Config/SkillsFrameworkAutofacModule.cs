using Autofac;
using Autofac.Extras.DynamicProxy2;
using AutoMapper;
using DFC.Digital.Core.Interceptors;
using DFC.Digital.Repository.ONET.DataModel;

namespace DFC.Digital.Repository.ONET
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

            builder.RegisterType<OnetSkillsFramework>()
                .InstancePerLifetimeScope();

            builder.Register(ctx => new MapperConfiguration(cfg => cfg.AddProfile<SkillsFrameworkMapper>()))
                .InstancePerLifetimeScope();

            builder.Register(ctx => ctx.Resolve<MapperConfiguration>().CreateMapper())
                .As<IMapper>()
                .InstancePerLifetimeScope();
        }
    }
}