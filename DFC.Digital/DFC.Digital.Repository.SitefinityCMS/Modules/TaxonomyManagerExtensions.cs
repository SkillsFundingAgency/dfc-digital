using System;
using System.Linq;
using System.Linq.Expressions;

namespace DFC.Digital.Repository.SitefinityCMS.Modules
{
    public class TaxonomyManagerExtensions : ITaxonomyManagerExtensions
    {
        public IQueryable<T> WhereQueryable<T>(IQueryable<T> taxons, Expression<Func<T, bool>> where)
        {
            return taxons.Where(where);
        }

        public T Where<T>(IQueryable<T> taxons, Expression<Func<T, bool>> predicate)
        {
            return taxons.SingleOrDefault(predicate);
        }
    }
}