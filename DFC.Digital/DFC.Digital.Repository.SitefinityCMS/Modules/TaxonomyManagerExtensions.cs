﻿using DFC.Digital.Repository.SitefinityCMS.Base;
using System;
using System.Linq;
using System.Linq.Expressions;
using Telerik.Sitefinity.Taxonomies.Model;

namespace DFC.Digital.Repository.SitefinityCMS.Modules
{
    public class TaxonomyManagerExtensions : ITaxonomyManagerExtensions
    {
        public IQueryable<T> WhereQueryable<T>(IQueryable<T> taxons, Expression<Func<T, bool>> where)
        {
            return taxons.Where(where);
        }
    }
}
