using DFC.Digital.Data.Interfaces;
using System;
using Telerik.Sitefinity.Taxonomies.Model;

namespace DFC.Digital.Repository.SitefinityCMS
{
    public interface ITaxonomyRepository : IRepository<Taxon>, IDisposable
    {
    }
}