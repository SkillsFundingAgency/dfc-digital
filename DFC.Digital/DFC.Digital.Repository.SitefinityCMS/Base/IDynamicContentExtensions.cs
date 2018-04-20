using System.Linq;
using Telerik.Sitefinity.DynamicModules.Model;

namespace DFC.Digital.Repository.SitefinityCMS
{
    public interface IDynamicContentExtensions
    {
        IQueryable<DynamicContent> GetRelatedParentItems(DynamicContent content, string contentTypeName, string providerName);
    }
}