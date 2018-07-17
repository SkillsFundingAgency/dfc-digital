using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.GenericContent.Model;
using Telerik.Sitefinity.Model;

namespace DFC.Digital.Repository.SitefinityCMS
{
    public class JobProfileRepository : IJobProfileRepository
    {
        #region Fields

        private const string RelatedSkillField = "RelatedSkills";
        private readonly IDynamicModuleRepository<JobProfile> repository;
        private readonly IDynamicModuleRepository<SocSkillMatrix> socSkillRepository;
        private readonly IDynamicModuleConverter<JobProfile> converter;
        private readonly IDynamicContentExtensions dynamicContentExtensions;
        private Dictionary<string, JobProfile> cachedJobProfiles = new Dictionary<string, JobProfile>();

        #endregion Fields

        #region Ctor

        public JobProfileRepository(IDynamicModuleRepository<JobProfile> repository, IDynamicModuleConverter<JobProfile> converter, IDynamicContentExtensions dynamicContentExtensions, IDynamicModuleRepository<SocSkillMatrix> socSkillRepository)
        {
            this.repository = repository;
            this.converter = converter;
            this.dynamicContentExtensions = dynamicContentExtensions;
            this.socSkillRepository = socSkillRepository;
        }

        #endregion Ctor

        #region IJobProfileRepository Implementations

        /// <summary>
        /// Returns a jobprofile for normal front end view.
        /// Only live profiles are returned.
        /// </summary>
        /// <param name="urlName">URL of the jobprofile to return</param>
        /// <returns>JobProfile</returns>
        public JobProfile GetByUrlName(string urlName)
        {
            var key = urlName.ToLower();
            if (!cachedJobProfiles.ContainsKey(key))
            {
                var jobProfile = ConvertDynamicContent(repository.Get(item =>
                    item.UrlName == urlName && item.Status == ContentLifecycleStatus.Live && item.Visible == true));
                cachedJobProfiles.Add(key, jobProfile);
            }

            return cachedJobProfiles[key]?.IsImported == true ? null : cachedJobProfiles[key];
        }

        /// <summary>
        /// Returns a jobprofile for Preview mode to a user logged into the backed.
        /// Profiles that are not live are still returned
        /// </summary>
        /// <param name="urlName">URL of the jobprofile to return</param>
        /// <returns>JobProfile</returns>
        public JobProfile GetByUrlNameForPreview(string urlName)
        {
            return ConvertDynamicContent(repository.Get(item => item.UrlName == urlName && item.Status == ContentLifecycleStatus.Temp));
        }

        public JobProfile GetByUrlNameForSearchIndex(string urlName, bool isPublishing)
        {
            return ConvertDynamicContent(repository.Get(item => item.UrlName == urlName && item.Status == (isPublishing ? ContentLifecycleStatus.Master : ContentLifecycleStatus.Live)));
        }

        public IEnumerable<JobProfile> GetLiveJobProfiles()
        {
            var jobProfiles = repository.GetMany(item => item.Status == ContentLifecycleStatus.Live && item.Visible == true).ToList();

            if (jobProfiles.Any())
            {
                return jobProfiles.Select(item => converter.ConvertFrom(item));
            }

            return Enumerable.Empty<JobProfile>();
        }

        public RepoActionResult UpdateDigitalSkill(JobProfile jobProfile)
        {
            var jobprofile = repository.Get(item =>
                item.Visible && item.Status == ContentLifecycleStatus.Live && item.UrlName == jobProfile.UrlName);

            if (jobprofile != null)
            {
                var master = repository.GetMaster(jobprofile);

                var temp = repository.GetTemp(master);

                dynamicContentExtensions.SetFieldValue(temp, nameof(JobProfile.DigitalSkillsLevel), jobProfile.DigitalSkillsLevel);

                var updatedMaster = repository.CheckinTemp(temp);

                repository.Update(updatedMaster);
                repository.Commit();
            }

            return new RepoActionResult { Success = true };
        }

        public RepoActionResult UpdateSocSkillMatrices(JobProfile jobProfile, IEnumerable<SocSkillMatrix> socSkillMatrices)
        {
            var jobprofile = repository.Get(item =>
                item.Visible && item.Status == ContentLifecycleStatus.Live && item.UrlName == jobProfile.UrlName);

            if (jobprofile != null)
            {
                var master = repository.GetMaster(jobprofile);

                var temp = repository.GetTemp(master);

                dynamicContentExtensions.DeleteRelatedFieldValues(temp, RelatedSkillField);

                foreach (var socSkillMatrix in socSkillMatrices)
                {
                    var relatedSocSkillItem = socSkillRepository.Get(d => d.Status == ContentLifecycleStatus.Live && d.UrlName == socSkillMatrix.SfUrlName);
                    if (relatedSocSkillItem != null)
                    {
                        dynamicContentExtensions.SetRelatedFieldValue(temp, relatedSocSkillItem, RelatedSkillField);
                    }
                }

                var updatedMaster = repository.CheckinTemp(temp);

                repository.Update(updatedMaster);
                repository.Commit();
            }

            return new RepoActionResult { Success = true };
        }

        public Type GetContentType()
        {
            return repository.GetContentType();
        }

        public string GetProviderName()
        {
            return repository.GetProviderName();
        }

        private JobProfile ConvertDynamicContent(DynamicContent dynamicContent)
        {
            if (dynamicContent != null)
            {
                return converter.ConvertFrom(dynamicContent);
            }

            return null;
        }

        #endregion IJobProfileRepository Implementations
    }
}