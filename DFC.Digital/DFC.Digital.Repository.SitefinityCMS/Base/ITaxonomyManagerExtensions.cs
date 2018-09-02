using DFC.Digital.Core.Interceptors;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DFC.Digital.Repository.SitefinityCMS
{
    public interface ITaxonomyManagerExtensions
    {
        [IgnoreInputInInterception]
        [IgnoreOutputInInterception]
        IQueryable<T> WhereQueryable<T>(IQueryable<T> taxons, Expression<Func<T, bool>> where);

        [IgnoreInputInInterception]
        [IgnoreOutputInInterception]
        T Where<T>(IQueryable<T> taxons, Expression<Func<T, bool>> predicate);
    }
}