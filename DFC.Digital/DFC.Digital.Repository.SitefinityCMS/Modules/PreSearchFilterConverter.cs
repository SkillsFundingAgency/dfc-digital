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
            Title = content?.GetValue<Lstring>(nameof(PreSearchFilter.Title)),
            Description = content?.GetValue<Lstring>(nameof(PreSearchFilter.Description)),
            NotApplicable = content?.GetValue<bool>(nameof(PreSearchFilter.NotApplicable)),
            Order = content?.GetValue<decimal?>(nameof(PreSearchFilter.Order)),
            UrlName = content?.GetValue<Lstring>(nameof(PreSearchFilter.UrlName)),
            Id = content?.GetValue<Guid>(nameof(PreSearchFilter.Id))
        };
    }
}