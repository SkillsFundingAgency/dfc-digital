using DFC.Digital.Core.Interceptors;
using DFC.Digital.Data.Model;
using System.Linq;
using Telerik.Sitefinity.DynamicModules.Model;

namespace DFC.Digital.Repository.SitefinityCMS
{
    public interface IRelatedClassificationsRepository
    {
        [IgnoreInputInInterception]
        IQueryable<string> GetRelatedClassifications(DynamicContent content, string relatedField, string taxonomyName);

        [IgnoreInputInInterception]
        IQueryable<TaxonReport> GetRelatedCmsReportClassifications(DynamicContent content, string relatedField, string taxonomyName);
    }
}