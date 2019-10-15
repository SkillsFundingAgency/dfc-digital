using DFC.Digital.Data.Interfaces;
using System;
using System.Linq.Expressions;
using Telerik.Sitefinity.Taxonomies.Model;

namespace DFC.Digital.Repository.SitefinityCMS
{
    public interface IHierarchicalTaxonomyRepository : IQueryRepository<HierarchicalTaxon>, IDisposable
    {
    }
}