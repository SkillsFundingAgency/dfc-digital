using DFC.Digital.Core.Interceptors;
using System.Linq;
using Telerik.Sitefinity.DynamicModules.Model;

namespace DFC.Digital.Repository.SitefinityCMS
{
    public interface IRelatedClassificationsRepository
    {
        [IgnoreInputInInterception]
        IQueryable<string> GetRelatedClassifications(DynamicContent content, string relatedField, string taxonomyName);
    }
}