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
            Title = content?.GetValueOrDefault<Lstring>(nameof(PreSearchFilter.Title)),
            Description = content?.GetValueOrDefault<Lstring>(nameof(PreSearchFilter.Description)),
            NotApplicable = content?.GetValueOrDefault<bool>(nameof(PreSearchFilter.NotApplicable)),
            Order = content?.GetValueOrDefault<decimal?>(nameof(PreSearchFilter.Order)),
            UrlName = content?.GetValueOrDefault<Lstring>(nameof(PreSearchFilter.UrlName)),
            Id = content?.GetValueOrDefault<Guid>(nameof(PreSearchFilter.Id))
        };
    }
}