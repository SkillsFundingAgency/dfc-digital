using DFC.Digital.Core.Interceptors;
using DFC.Digital.Data.Interfaces;
using System;
using System.Linq;
using System.Linq.Expressions;
using Telerik.Sitefinity.Taxonomies.Model;
using Telerik.Sitefinity.Taxonomies.Web;

namespace DFC.Digital.Repository.SitefinityCMS
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public abstract class FlatTaxonomyRepository : IFlatTaxonomyRepository
    {
        private readonly ITaxonomyManagerExtensions taxonomyManagerExtensions;
        private ITaxonomyManager manager;

        protected FlatTaxonomyRepository(ITaxonomyManager manager, ITaxonomyManagerExtensions taxonomyManagerExtensions)
        {
            this.manager = manager;
            this.taxonomyManagerExtensions = taxonomyManagerExtensions;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        [IgnoreInputInInterception]
        [IgnoreOutputInInterception]
        public FlatTaxon Get(Expression<Func<FlatTaxon, bool>> where)
        {
            return taxonomyManagerExtensions.Where(manager.GetTaxa<FlatTaxon>(), where);
        }

        [IgnoreOutputInInterception]
        public IQueryable<FlatTaxon> GetAll()
        {
            return manager.GetTaxa<FlatTaxon>();
        }

        [IgnoreOutputInInterception]
        public FlatTaxon GetById(string id)
        {
            return manager.GetTaxon(new Guid(id)) as FlatTaxon;
        }

        [IgnoreInputInInterception]
        [IgnoreOutputInInterception]
        public IQueryable<FlatTaxon> GetMany(Expression<Func<FlatTaxon, bool>> where)
        {
            return taxonomyManagerExtensions.WhereQueryable(manager.GetTaxa<FlatTaxon>(), where);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (manager != null)
                {
                    manager.Dispose();
                    manager = null;
                }
            }
        }
    }
}