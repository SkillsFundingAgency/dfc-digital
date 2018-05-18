using System;
using System.Linq;
using System.Linq.Expressions;
using Telerik.Sitefinity.Taxonomies.Model;

namespace DFC.Digital.Repository.SitefinityCMS.Base
{
    public interface ITaxonomyManagerExtensions
    {
        IQueryable<T> WhereQueryable<T>(IQueryable<T> taxons, Expression<Func<T, bool>> where);

        T Where<T>(IQueryable<T> taxons, Expression<Func<T, bool>> where);
    }
}
