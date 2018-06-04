﻿using System.Linq;
using Telerik.Sitefinity.DynamicModules.Model;

namespace DFC.Digital.Repository.SitefinityCMS
{
    public interface IDynamicContentExtensions
    {
        IQueryable<DynamicContent> GetRelatedParentItems(DynamicContent contentItem, string contentTypeName, string providerName);

        IQueryable<DynamicContent> GetRelatedItems(DynamicContent contentItem, string fieldName, int maximumItemsToReturn);

        T GetFieldValue<T>(DynamicContent contentItem, string fieldName);

        string GetFieldStringValue(DynamicContent contentItem, string fieldName);
    }
}