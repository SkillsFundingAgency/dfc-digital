using System.Linq;
using Telerik.Sitefinity.DynamicModules.Model;

namespace DFC.Digital.Repository.SitefinityCMS
{
    public interface IRelatedClassificationsRepository
    {
        IQueryable<string> GetRelatedClassifications(DynamicContent content, string relatedField, string taxonomyName);
    }
}