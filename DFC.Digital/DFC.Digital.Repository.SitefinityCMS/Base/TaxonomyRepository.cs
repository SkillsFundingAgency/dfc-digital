using DFC.Digital.Core.Interceptors;
using System;
using System.Linq;
using System.Linq.Expressions;
using Telerik.Sitefinity.Taxonomies.Model;
using Telerik.Sitefinity.Taxonomies.Web;

namespace DFC.Digital.Repository.SitefinityCMS
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public abstract class TaxonomyRepository : ITaxonomyRepository
    {
        private readonly ITaxonomyManagerExtensions taxonomyManagerExtensions;
        private ITaxonomyManager manager;

        protected TaxonomyRepository(ITaxonomyManager manager, ITaxonomyManagerExtensions taxonomyManagerExtensions)
        {
            this.manager = manager;
            this.taxonomyManagerExtensions = taxonomyManagerExtensions;
        }

        [IgnoreInputInInterception]
        public void Add(Taxon entity)
        {
            throw new NotImplementedException();
        }

        [IgnoreInputInInterception]
        public void Delete(Taxon entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Expression<Func<Taxon, bool>> where)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        [IgnoreInputInInterception]
        [IgnoreOutputInInterception]
        public Taxon Get(Expression<Func<Taxon, bool>> where)
        {
            return taxonomyManagerExtensions.Where(manager.GetTaxa<Taxon>(), where);
        }

        [IgnoreOutputInInterception]
        public IQueryable<Taxon> GetAll()
        {
            return manager.GetTaxa<Taxon>();
        }

        [IgnoreOutputInInterception]
        public Taxon GetById(string id)
        {
            return manager.GetTaxon(new Guid(id)) as Taxon;
        }

        [IgnoreInputInInterception]
        [IgnoreOutputInInterception]
        public IQueryable<Taxon> GetMany(Expression<Func<Taxon, bool>> where)
        {
            return taxonomyManagerExtensions.WhereQueryable(manager.GetTaxa<Taxon>(), where);
        }

        [IgnoreInputInInterception]
        public void Update(Taxon entity)
        {
            throw new NotImplementedException();
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