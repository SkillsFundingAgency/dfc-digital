using DFC.Digital.Core.Interceptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Taxonomies.Model;
using Telerik.Sitefinity.Taxonomies.Web;

namespace DFC.Digital.Repository.SitefinityCMS.Modules
{
    public class RelatedClassificationsRepository : TaxonomyRepository, IRelatedClassificationsRepository
    {
        private readonly IDynamicContentExtensions dynamicContentExtensions;

        public RelatedClassificationsRepository(ITaxonomyManager taxonomyManager, IDynamicContentExtensions dynamicContentExtensions, ITaxonomyManagerExtensions taxonomyManagerExtensions) : base(taxonomyManager, taxonomyManagerExtensions)
        {
            this.dynamicContentExtensions = dynamicContentExtensions;
        }

        public IQueryable<string> GetRelatedClassifications(DynamicContent content, string relatedField, string taxonomyName)
        {
            var relatedClasifications = dynamicContentExtensions.GetFieldValue<IList<Guid>>(content, relatedField);
            var clasifications = GetMany(c => relatedClasifications.Contains(c.Id) && c.Taxonomy.Name == taxonomyName);
            return clasifications.Select(c => $"{c.Title}");
        }
    }
}