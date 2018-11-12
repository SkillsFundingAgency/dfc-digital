using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.Taxonomies.Model;
using Telerik.Sitefinity.Taxonomies.Web;

namespace DFC.Digital.Repository.SitefinityCMS.Modules
{
    public class RelatedClassificationsRepository : TaxonomyRepository, IRelatedClassificationsRepository
    {
        private const string LarsCodeFieldName = "UrlName";
        private const string TitleFieldName = "Title";
        private readonly IDynamicContentExtensions dynamicContentExtensions;

        public RelatedClassificationsRepository(ITaxonomyManager taxonomyManager, IDynamicContentExtensions dynamicContentExtensions, ITaxonomyManagerExtensions taxonomyManagerExtensions) : base(taxonomyManager, taxonomyManagerExtensions)
        {
            this.dynamicContentExtensions = dynamicContentExtensions;
        }

        public IQueryable<string> GetRelatedClassifications(DynamicContent content, string relatedField, string taxonomyName)
        {
            var clasifications = Clasifications(content, relatedField, taxonomyName);
            return clasifications.Select(c => $"{c.Title}");
        }

        public IQueryable<TaxonReport> GetRelatedCmsReportClassifications(DynamicContent content, string relatedField, string taxonomyName)
        {
            var clasifications = Clasifications(content, relatedField, taxonomyName);
            return clasifications.Select(c => new TaxonReport { Title = c.Title, LarsCode = c.UrlName });
        }

        private IQueryable<Taxon> Clasifications(DynamicContent content, string relatedField, string taxonomyName)
        {
            var relatedClasifications = dynamicContentExtensions.GetFieldValue<IList<Guid>>(content, relatedField);
            var clasifications = GetMany(c => relatedClasifications.Contains(c.Id) && c.Taxonomy.Name == taxonomyName);
            return clasifications;
        }
    }
}