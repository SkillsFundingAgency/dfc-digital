using DFC.Digital.Data.Interfaces;
using System;
using System.Linq;
using System.Linq.Expressions;
using Telerik.Sitefinity.Taxonomies.Model;
using Telerik.Sitefinity.Taxonomies.Web;

namespace DFC.Digital.Repository.SitefinityCMS
{
    public abstract class TaxonomyRepository : IRepository<Taxon>, IDisposable
    {
        private ITaxonomyManager manager;

        protected TaxonomyRepository(ITaxonomyManager manager)
        {
            this.manager = manager;
        }

        public void Add(Taxon entity)
        {
            throw new NotImplementedException();
        }

        public Taxon CreateEntity()
        {
            throw new NotImplementedException();
        }

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

        public Taxon Get(Expression<Func<Taxon, bool>> where)
        {
            return manager.GetTaxa<Taxon>().SingleOrDefault(where);
        }

        public IQueryable<Taxon> GetAll()
        {
            return manager.GetTaxa<Taxon>();
        }

        public Taxon GetById(string id)
        {
            return manager.GetTaxon(new Guid(id)) as Taxon;
        }

        public IQueryable<Taxon> GetMany(Expression<Func<Taxon, bool>> where)
        {
            return manager.GetTaxa<Taxon>().Where(where);
        }

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