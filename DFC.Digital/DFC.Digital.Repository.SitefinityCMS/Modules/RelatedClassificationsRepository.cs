using DFC.Digital.Repository.SitefinityCMS.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Taxonomies.Web;

namespace DFC.Digital.Repository.SitefinityCMS.Modules
{
    public class RelatedClassificationsRepository : TaxonomyRepository, IRelatedClassificationsRepository
    {
        public RelatedClassificationsRepository(ITaxonomyManager taxonomyManager) : base(taxonomyManager)
        {
        }

        public IQueryable<string> GetRelatedClassifications(DynamicContent content, string relatedField, string taxonomyName)
        {
            var relatedClasifications = content?.GetValueOrDefault<IList<Guid>>(relatedField);
            var clasifications = GetMany(c => relatedClasifications.Contains(c.Id) && c.Taxonomy.Name == taxonomyName);
            return clasifications.Select(c => $"{c.Title}");
        }
    }
}