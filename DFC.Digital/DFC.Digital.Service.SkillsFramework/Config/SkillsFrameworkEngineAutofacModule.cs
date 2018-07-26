using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extras.DynamicProxy2;
using DFC.Digital.Core.Interceptors;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Repository.ONET.Query;

namespace DFC.Digital.Service.SkillsFramework.Config
{
    using Autofac;

    public class SkillsFrameworkEngineAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterAssemblyTypes(ThisAssembly).AsImplementedInterfaces();


        }
    }
}
