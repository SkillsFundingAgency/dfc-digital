using Autofac;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.Repository.SitefinityCMS.Modules
{
    public class PreSearchFiltersFactory : IPreSearchFiltersFactory
    {
        private readonly ILifetimeScope lifetimeScope;

        public PreSearchFiltersFactory(ILifetimeScope lifetimeScope)
        {
            this.lifetimeScope = lifetimeScope;
        }

        public IPreSearchFiltersRepository<T> GetRepository<T>()
            where T : PreSearchFilter, new()
        {
            return lifetimeScope.Resolve<IPreSearchFiltersRepository<T>>();
        }
    }
}
