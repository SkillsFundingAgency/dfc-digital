using DFC.Digital.Data.Interfaces;
using System;
using System.Linq.Expressions;
using Telerik.Sitefinity.Taxonomies.Model;

namespace DFC.Digital.Repository.SitefinityCMS
{
    public interface ITaxonomyRepository : IQueryRepository<HierarchicalTaxon>, IDisposable
    {
    }
}