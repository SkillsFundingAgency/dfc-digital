using DFC.Digital.Core;
using System.Collections.Generic;
using System.Linq;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.GenericContent.Model;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.RelatedData;
using Telerik.Sitefinity.Taxonomies.Model;

namespace DFC.Digital.Repository.SitefinityCMS.Modules
{
    public class DynamicContentExtensions : IDynamicContentExtensions
    {
        private const string PublishedWorkFlowStatus = "Published";

        public IQueryable<DynamicContent> GetRelatedParentItems(DynamicContent contentItem, string contentTypeName, string providerName)
        {
            return contentItem?
                .GetRelatedParentItems(contentTypeName, providerName)
                .OfType<DynamicContent>()
                .Where(d => d.Status == ContentLifecycleStatus.Live && d.Visible);
        }

        public IQueryable<DynamicContent> GetRelatedItems(DynamicContent contentItem, string fieldName, int maximumItemsToReturn = Constants.DefaultMaxRelatedItems)
        {
            return contentItem?
                .GetRelatedItems<DynamicContent>(fieldName)
                .Where(d => d.Status == ContentLifecycleStatus.Live && d.Visible)
                .Take(maximumItemsToReturn);
        }

        public IEnumerable<DynamicContent> GetRelatedSearchItems(DynamicContent contentItem, string fieldName, int maximumItemsToReturn = Constants.DefaultMaxRelatedItems)
        {
            return contentItem?
                .GetRelatedItems<DynamicContent>(fieldName)
                .Where(d => d.ApprovalWorkflowState == PublishedWorkFlowStatus && (d.Status == ContentLifecycleStatus.Live || d.Status == ContentLifecycleStatus.Master))
                .Take(maximumItemsToReturn).ToList();
        }

        public T GetFieldValue<T>(DynamicContent contentItem, string fieldName)
        {
            return contentItem != null && contentItem.DoesFieldExist(fieldName) ? contentItem.GetValue<T>(fieldName) : default;
        }

        public T GetTaxonFieldValue<T>(Taxon taxonItem, string fieldName)
        {
            return taxonItem != null && taxonItem.DoesFieldExist(fieldName) ? taxonItem.GetValue<T>(fieldName) : default;
        }

        public void SetFieldValue<T>(DynamicContent contentItem, string fieldName, T value)
        {
            if (contentItem.DoesFieldExist(fieldName))
            {
                contentItem.SetValue(fieldName, value);
            }
        }

        public void SetRelatedFieldValue(DynamicContent contentItem, DynamicContent relatedContentItem, string fieldName, float ordinal)
        {
           var item = contentItem.CreateRelation(relatedContentItem, fieldName);
            item.Ordinal = ordinal;
        }

        public void SetRelatedDataSourceContext(IQueryable<DynamicContent> contentItems)
        {
            contentItems.SetRelatedDataSourceContext();
        }

        public string GetFieldStringValue(DynamicContent contentItem, string fieldName)
        {
            if (contentItem != null && contentItem.DoesFieldExist(fieldName))
            {
                string value = contentItem.GetValue<Lstring>(fieldName);
                return value;
            }
            else
            {
                return default;
            }
        }

        public string GetFieldChoiceValue(DynamicContent contentItem, string fieldName)
        {
            if (contentItem != null && contentItem.DoesFieldExist(fieldName))
            {
                string value = contentItem.GetValue<ChoiceOption>(fieldName)?.PersistedValue;
                return value;
            }
            else
            {
                return default;
            }
        }

        public string GetFieldChoiceLabel(DynamicContent contentItem, string fieldName)
        {
            if (contentItem != null && contentItem.DoesFieldExist(fieldName))
            {
                string value = contentItem.GetValue<ChoiceOption>(fieldName)?.Text;
                return value;
            }
            else
            {
                return default;
            }
        }

        public void DeleteRelatedFieldValues(DynamicContent contentItem, string fieldName)
        {
            contentItem.DeleteRelations(fieldName);
        }

        public IQueryable<string> GetRelatedContentUrl(DynamicContent content, string relatedField)
        {
                var relatedContent = GetRelatedItems(content, relatedField, 100);
                return relatedContent?.Select(x => $"{x.UrlName}");
        }
    }
}