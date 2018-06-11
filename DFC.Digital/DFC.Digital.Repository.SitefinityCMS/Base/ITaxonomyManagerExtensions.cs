using System;
using System.Linq;
using System.Linq.Expressions;

namespace DFC.Digital.Repository.SitefinityCMS
{
    public interface ITaxonomyManagerExtensions
    {
        IQueryable<T> WhereQueryable<T>(IQueryable<T> taxons, Expression<Func<T, bool>> where);

        T Where<T>(IQueryable<T> taxons, Expression<Func<T, bool>> predicate);
    }
}