using DFC.Digital.Core;
using System.Collections.Generic;
using System.Linq;
using Telerik.Sitefinity.DynamicModules.Model;

namespace DFC.Digital.Repository.SitefinityCMS
{
    public interface IDynamicContentExtensions
    {
        IQueryable<DynamicContent> GetRelatedParentItems(DynamicContent contentItem, string contentTypeName, string providerName);

        IQueryable<DynamicContent> GetRelatedItems(DynamicContent contentItem, string fieldName, int maximumItemsToReturn = Constants.DefaultMaxRelatedItems);

        IEnumerable<DynamicContent> GetRelatedSearchItems(DynamicContent contentItem, string fieldName, int maximumItemsToReturn = Constants.DefaultMaxRelatedItems);

        T GetFieldValue<T>(DynamicContent contentItem, string fieldName);

        void SetFieldValue<T>(DynamicContent contentItem, string fieldName, T value);

        void SetRelatedFieldValue(DynamicContent contentItem, DynamicContent relatedContentItem, string fieldName);

        void DeleteRelatedFieldValues(DynamicContent contentItem, string fieldName);

        string GetFieldStringValue(DynamicContent contentItem, string fieldName);

        string GetFieldChoiceValue(DynamicContent contentItem, string fieldName);

        string GetFieldChoiceLabel(DynamicContent contentItem, string fieldName);

        IQueryable<string> GetRelatedContentUrl(DynamicContent content, string relatedField);
    }
}