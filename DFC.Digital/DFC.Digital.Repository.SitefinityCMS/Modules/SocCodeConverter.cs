using DFC.Digital.Data.Model;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Model;

namespace DFC.Digital.Repository.SitefinityCMS.Modules
{
    public class SocCodeConverter : IDynamicModuleConverter<SocCode>
    {
        private readonly IDynamicContentExtensions dynamicContentExtensions;

        public SocCodeConverter(IDynamicContentExtensions dynamicContentExtensions)
        {
            this.dynamicContentExtensions = dynamicContentExtensions;
        }

        public SocCode ConvertFrom(DynamicContent content)
        {
            return new SocCode
            {
                Title = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(SocCode.Title)),
                SOCCode = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(SocCode.SOCCode)),
                ONetOccupationalCode = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(SocCode.ONetOccupationalCode)),
                UrlName = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(SocCode.UrlName))
            };
        }
    }
}