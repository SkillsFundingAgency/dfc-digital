using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.GenericContent.Model;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.RelatedData;

namespace DFC.Digital.Repository.SitefinityCMS.Modules
{
    public class DynamicContentExtensions : IDynamicContentExtensions
    {
        public IQueryable<DynamicContent> GetRelatedParentItems(DynamicContent contentItem, string contentTypeName, string providerName)
        {
            return contentItem?
                .GetRelatedParentItems(contentTypeName, providerName)
                .OfType<DynamicContent>()
                .Where(d => d.Status == ContentLifecycleStatus.Live && d.Visible);
        }

        public IQueryable<DynamicContent> GetRelatedItems(DynamicContent contentItem, string fieldName, int maximumItemsToReturn)
        {
            return contentItem?
                .GetRelatedItems<DynamicContent>(fieldName)
                .Where(d => d.Status == ContentLifecycleStatus.Live && d.Visible)
                .Take(maximumItemsToReturn);
        }

        public T GetFieldValue<T>(DynamicContent contentItem, string fieldName)
        {
            return contentItem == null ? default(T) : contentItem.GetValue<T>(fieldName);
        }
    }
}
