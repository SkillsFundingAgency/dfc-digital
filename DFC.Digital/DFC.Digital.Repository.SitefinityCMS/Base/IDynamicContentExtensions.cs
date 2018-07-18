using DFC.Digital.Core;
using System.Linq;
using Telerik.Sitefinity.DynamicModules.Model;

namespace DFC.Digital.Repository.SitefinityCMS
{
    public interface IDynamicContentExtensions
    {
        IQueryable<DynamicContent> GetRelatedParentItems(DynamicContent contentItem, string contentTypeName, string providerName);

        IQueryable<DynamicContent> GetRelatedItems(DynamicContent contentItem, string fieldName, int maximumItemsToReturn = Constants.DefaultMaxRelatedItems);

        T GetFieldValue<T>(DynamicContent contentItem, string fieldName);

        string GetFieldStringValue(DynamicContent contentItem, string fieldName);

        IQueryable<string> GetRelatedContentUrl(DynamicContent content, string relatedField);
    }
}