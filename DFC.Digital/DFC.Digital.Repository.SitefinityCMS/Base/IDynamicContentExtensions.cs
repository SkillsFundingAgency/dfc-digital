using DFC.Digital.Core;
using DFC.Digital.Core.Interceptors;
using System.Collections.Generic;
using System.Linq;
using Telerik.Sitefinity.DynamicModules.Model;

namespace DFC.Digital.Repository.SitefinityCMS
{
    public interface IDynamicContentExtensions
    {
        [IgnoreInputInInterception]
        [IgnoreOutputInInterception]
        IQueryable<DynamicContent> GetRelatedParentItems(DynamicContent contentItem, string contentTypeName, string providerName);

        [IgnoreInputInInterception]
        [IgnoreOutputInInterception]
        IQueryable<DynamicContent> GetRelatedItems(DynamicContent contentItem, string fieldName, int maximumItemsToReturn = Constants.DefaultMaxRelatedItems);

        [IgnoreInputInInterception]
        [IgnoreOutputInInterception]
        IEnumerable<DynamicContent> GetRelatedSearchItems(DynamicContent contentItem, string fieldName, int maximumItemsToReturn = Constants.DefaultMaxRelatedItems);

        [IgnoreInputInInterception]
        [IgnoreOutputInInterception]
        T GetFieldValue<T>(DynamicContent contentItem, string fieldName);

        [IgnoreInputInInterception]
        void SetFieldValue<T>(DynamicContent contentItem, string fieldName, T value);

        [IgnoreInputInInterception]
        void SetRelatedFieldValue(DynamicContent contentItem, DynamicContent relatedContentItem, string fieldName, float ordinal);

        [IgnoreInputInInterception]
        void DeleteRelatedFieldValues(DynamicContent contentItem, string fieldName);

        [IgnoreInputInInterception]
        string GetFieldStringValue(DynamicContent contentItem, string fieldName);

        [IgnoreInputInInterception]
        string GetFieldChoiceValue(DynamicContent contentItem, string fieldName);

        [IgnoreInputInInterception]
        string GetFieldChoiceLabel(DynamicContent contentItem, string fieldName);

        [IgnoreInputInInterception]
        IQueryable<string> GetRelatedContentUrl(DynamicContent content, string relatedField);
    }
}