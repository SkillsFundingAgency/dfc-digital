using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using System.Collections.Generic;
using System.Linq;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.GenericContent.Model;

namespace DFC.Digital.Repository.SitefinityCMS.Modules
{
    public class JobProfilesRelatedCareersRepository : IJobProfileRelatedCareersRepository
    {
        #region Fields

        private const string RelatedCareersFieldName = "RelatedCareerProfiles";
        private const string IsImportedField = "IsImported";
        private readonly IDynamicModuleConverter<JobProfileRelatedCareer> converter;
        private readonly IDynamicModuleRepository<JobProfile> jobprofileRepository;
        private readonly IDynamicContentExtensions dynamicContentExtensions;

        #endregion Fields

        #region Ctor

        public JobProfilesRelatedCareersRepository(IDynamicModuleConverter<JobProfileRelatedCareer> converter, IDynamicModuleRepository<JobProfile> jobprofileRepository, IDynamicContentExtensions dynamicContentExtensions)
        {
            this.converter = converter;
            this.jobprofileRepository = jobprofileRepository;
            this.dynamicContentExtensions = dynamicContentExtensions;
        }

        #endregion Ctor

        #region IJobProfileRepository Implementations

        public IEnumerable<JobProfileRelatedCareer> GetByParentName(string urlName, int maximumItemsToReturn)
        {
            var jobProfile = jobprofileRepository.Get(item => item.UrlName == urlName && item.Status == ContentLifecycleStatus.Live && item.Visible);
            return GetRelatedCareers(jobProfile, maximumItemsToReturn);
        }

        public IEnumerable<JobProfileRelatedCareer> GetByParentNameForPreview(string urlName, int maximumItemsToReturn)
        {
            var jobProfile = jobprofileRepository.Get(item => item.UrlName == urlName && item.Status == ContentLifecycleStatus.Temp);
            return GetRelatedCareers(jobProfile, maximumItemsToReturn);
        }

        public IEnumerable<JobProfileRelatedCareer> GetRelatedCareers(DynamicContent jobProfile, int maximumItemsToReturn)
        {
            var relatedCareerProfilesItem = dynamicContentExtensions.GetRelatedItems(jobProfile, RelatedCareersFieldName, maximumItemsToReturn);

            if (relatedCareerProfilesItem != null)
            {
                if (relatedCareerProfilesItem.Any())
                {
                    return relatedCareerProfilesItem.ToList().Where(x => !dynamicContentExtensions.GetFieldValue<bool>(x, IsImportedField)).Select(item => converter.ConvertFrom(item));
                }
            }

            return null;
        }

        #endregion IJobProfileRepository Implementations
    }
}