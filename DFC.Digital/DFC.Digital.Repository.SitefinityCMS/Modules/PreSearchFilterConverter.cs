using DFC.Digital.Data.Model;
using System;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Model;

namespace DFC.Digital.Repository.SitefinityCMS.Modules
{
    public class PreSearchFilterConverter<T> : IDynamicModuleConverter<T>
        where T : PreSearchFilter, new()
    {
        public T ConvertFrom(DynamicContent content) => new T
        {
            Title = content?.GetDynamicContentItemValue<Lstring>(nameof(PreSearchFilter.Title)),
            Description = content?.GetDynamicContentItemValue<Lstring>(nameof(PreSearchFilter.Description)),
            NotApplicable = content?.GetDynamicContentItemValue<bool>(nameof(PreSearchFilter.NotApplicable)),
            Order = content?.GetDynamicContentItemValue<decimal?>(nameof(PreSearchFilter.Order)),
            UrlName = content?.GetDynamicContentItemValue<Lstring>(nameof(PreSearchFilter.UrlName)),
            Id = content?.GetDynamicContentItemValue<Guid>(nameof(PreSearchFilter.Id))
        };
    }
}