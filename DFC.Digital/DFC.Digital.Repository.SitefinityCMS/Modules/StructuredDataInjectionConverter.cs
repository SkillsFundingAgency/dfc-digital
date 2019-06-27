using DFC.Digital.Data.Model;
using System;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Model;

namespace DFC.Digital.Repository.SitefinityCMS.Modules
{
    public class StructuredDataInjectionConverter : IDynamicModuleConverter<StructuredDataInjection>
    {
        private readonly IDynamicContentExtensions dynamicContentExtensions;

        public StructuredDataInjectionConverter(IDynamicContentExtensions dynamicContentExtensions)
        {
            this.dynamicContentExtensions = dynamicContentExtensions;
        }

        public StructuredDataInjection ConvertFrom(DynamicContent content)
        {
            return new StructuredDataInjection
            {
                Title = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(StructuredDataInjection.Title)),
                Script = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(StructuredDataInjection.Script)),
                DataType = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(StructuredDataInjection.DataType))
            };
        }
    }
}
