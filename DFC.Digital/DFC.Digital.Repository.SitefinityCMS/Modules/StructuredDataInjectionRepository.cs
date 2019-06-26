using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using System;
using System.Linq;
using Telerik.Sitefinity.GenericContent.Model;

namespace DFC.Digital.Repository.SitefinityCMS.Modules
{
    public class StructuredDataInjectionRepository : IStructuredDataInjectionRepository
    {
        #region Fields
        private readonly IDynamicModuleRepository<StructuredDataInjection> structuredDataDynamicModuleRepository;
        private readonly IDynamicModuleConverter<StructuredDataInjection> converter;
        private readonly IDynamicModuleRepository<JobProfile> jobprofileRepository;
        private readonly IDynamicContentExtensions dynamicContentExtensions;
        #endregion Fields

        public StructuredDataInjectionRepository(IDynamicContentExtensions dynamicContentExtensions, IDynamicModuleRepository<JobProfile> jobprofileRepository, IDynamicModuleRepository<StructuredDataInjection> structuredDataDynamicModuleRepository, IDynamicModuleConverter<StructuredDataInjection> converter)
        {
            this.structuredDataDynamicModuleRepository = structuredDataDynamicModuleRepository;
            this.converter = converter;
            this.jobprofileRepository = jobprofileRepository;
            this.dynamicContentExtensions = dynamicContentExtensions;
        }

        public StructuredDataInjection GetByJobProfileUrlName(string urlName)
        {
            var jobProfile = jobprofileRepository.Get(item => item.UrlName == urlName && item.Status == ContentLifecycleStatus.Live && item.Visible);

            if (jobProfile != null)
            {
                var structuredDataItems = dynamicContentExtensions.GetRelatedParentItems(jobProfile, DynamicTypes.StructuredDataInjectionContentType, structuredDataDynamicModuleRepository.GetProviderName());

                if (structuredDataItems.Any())
                {
                    var structuredDataItem = converter.ConvertFrom(structuredDataItems.First());
                    structuredDataItem.JobProfileLinkName = urlName;
                    return structuredDataItem;
                }
            }

            return null;
        }

        public Type GetContentType()
        {
            return structuredDataDynamicModuleRepository.GetContentType();
        }

        public string GetProviderName()
        {
            return structuredDataDynamicModuleRepository.GetProviderName();
        }
    }
}
