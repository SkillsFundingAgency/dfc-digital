using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using System.Collections.Generic;
using System.Linq;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.GenericContent.Model;
using Telerik.Sitefinity.RelatedData;

namespace DFC.Digital.Repository.SitefinityCMS.Modules
{
    internal class JobProfilesRelatedCareersRepository : IJobProfileRelatedCareersRepository
    {
        #region Fields

        private readonly IDynamicModuleConverter<JobProfileRelatedCareer> converter;
        private readonly IDynamicModuleRepository<JobProfile> jobprofileRepository;

        #endregion Fields

        #region Ctor

        public JobProfilesRelatedCareersRepository(IDynamicModuleConverter<JobProfileRelatedCareer> converter, IDynamicModuleRepository<JobProfile> jobprofileRepository)
        {
            this.converter = converter;
            this.jobprofileRepository = jobprofileRepository;
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
            if (jobProfile != null)
            {
                IQueryable<DynamicContent> relatedCareerProfilesItem = jobProfile.GetRelatedItems<DynamicContent>("RelatedCareerProfiles")
                    .Where(d => d.Status == ContentLifecycleStatus.Live && d.Visible).Take(maximumItemsToReturn);

                if (relatedCareerProfilesItem != null)
                {
                    if (relatedCareerProfilesItem.Any())
                    {
                        return relatedCareerProfilesItem.Select(item => converter.ConvertFrom(item));
                    }
                }
            }

            return null;
        }

        #endregion IJobProfileRepository Implementations
    }
}