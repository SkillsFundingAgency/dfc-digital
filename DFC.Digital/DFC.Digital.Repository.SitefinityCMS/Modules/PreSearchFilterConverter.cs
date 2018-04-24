using DFC.Digital.Data.Model;
using System;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Model;

namespace DFC.Digital.Repository.SitefinityCMS.Modules
{
    public class PreSearchFilterConverter<T> : IDynamicModuleConverter<T>
        where T : PreSearchFilter, new()
    {
        private readonly IDynamicContentExtensions dynamicContentExtensions;

        public PreSearchFilterConverter(IDynamicContentExtensions dynamicContentExtensions)
        {
            this.dynamicContentExtensions = dynamicContentExtensions;
        }

        public T ConvertFrom(DynamicContent content) => new T
        {
            Title = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(PreSearchFilter.Title)),
            Description = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(PreSearchFilter.Description)),
            NotApplicable = dynamicContentExtensions.GetFieldValue<bool>(content, nameof(PreSearchFilter.NotApplicable)),
            Order = dynamicContentExtensions.GetFieldValue<decimal?>(content, nameof(PreSearchFilter.Order)),
            UrlName = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(PreSearchFilter.UrlName)),
            Id = dynamicContentExtensions.GetFieldValue<Guid>(content, nameof(PreSearchFilter.Id)),
        };
    }
}