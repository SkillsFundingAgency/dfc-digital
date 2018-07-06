
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.Repository.ONET.Config
{
    using Autofac;
    using AutoMapper;
    using Data.Interfaces;

    public class SkillsFrameworkAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterAssemblyTypes(ThisAssembly).AsImplementedInterfaces();

            var profileTypes = typeof(SkillsFrameworkAutofacModule).Assembly.GetTypes().Where(x => typeof(Profile).IsAssignableFrom(x)).ToArray();
            builder.RegisterTypes(profileTypes).As<Profile>();
            builder.Register(c => new MapperConfiguration(cfg =>
            {
                //add your profiles (either resolve from container or however else you acquire them)
                var profiles = c.Resolve<IEnumerable<Profile>>();
                foreach(var profile in profiles)
                {
                    cfg.AddProfile(profile);
                }
            }));
            builder.Register(ctx => ctx.Resolve<MapperConfiguration>().CreateMapper()).As<IMapper>();
        }
    }
}
