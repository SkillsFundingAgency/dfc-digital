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

        public T ConvertFrom(DynamicContent content)
        {
            var title = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(PreSearchFilter.Title));
            return new T
            {
                Title = title,
                Description = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(PreSearchFilter.Description)),
                NotApplicable = title.Equals("None", StringComparison.OrdinalIgnoreCase),
                Order = title.Equals("None", StringComparison.OrdinalIgnoreCase) ? 1 : 2,
                UrlName = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(PreSearchFilter.UrlName)),
                Id = dynamicContentExtensions.GetFieldValue<Guid>(content, nameof(PreSearchFilter.Id)),
            };
        }
    }
}
