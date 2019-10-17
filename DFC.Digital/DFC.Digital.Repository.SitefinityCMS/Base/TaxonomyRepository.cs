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
    public class TaxonomyRepository : ITaxonomyRepository
    {
        private readonly ITaxonomyManagerExtensions taxonomyManagerExtensions;
        private ITaxonomyManager manager;

        public TaxonomyRepository(ITaxonomyManager manager, ITaxonomyManagerExtensions taxonomyManagerExtensions)
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
        public HierarchicalTaxon Get(Expression<Func<HierarchicalTaxon, bool>> where)
        {
            return taxonomyManagerExtensions.Where(manager.GetTaxa<HierarchicalTaxon>(), where);
        }

        [IgnoreOutputInInterception]
        public IQueryable<HierarchicalTaxon> GetAll()
        {
            return manager.GetTaxa<HierarchicalTaxon>();
        }

        [IgnoreOutputInInterception]
        public HierarchicalTaxon GetById(string id)
        {
            return manager.GetTaxon(new Guid(id)) as HierarchicalTaxon;
        }

        [IgnoreInputInInterception]
        [IgnoreOutputInInterception]
        public IQueryable<HierarchicalTaxon> GetMany(Expression<Func<HierarchicalTaxon, bool>> where)
        {
            return taxonomyManagerExtensions.WhereQueryable(manager.GetTaxa<HierarchicalTaxon>(), where);
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