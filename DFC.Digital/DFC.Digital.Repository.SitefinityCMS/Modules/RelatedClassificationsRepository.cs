using DFC.Digital.Repository.SitefinityCMS.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Model;
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
            var relatedClasifications = content?.GetValue<IList<Guid>>(relatedField);
            var clasifications = GetMany(c => relatedClasifications.Contains(c.Id) && c.Taxonomy.Name == taxonomyName);
            return clasifications.Select(c => $"{c.Title}");
        }
    }
}
