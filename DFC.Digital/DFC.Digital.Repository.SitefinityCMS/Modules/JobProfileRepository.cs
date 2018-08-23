using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Repository.SitefinityCMS.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.GenericContent.Model;

namespace DFC.Digital.Repository.SitefinityCMS
{
    public class JobProfileRepository : IJobProfileRepository
    {
        #region Fields

        private const string RelatedSkillField = "RelatedSkills";
        private const string UpdateComment = "Updated via the SkillsFramework import process";
        private readonly IDynamicModuleRepository<JobProfile> repository;
        private readonly IDynamicModuleRepository<SocSkillMatrix> socSkillRepository;
        private readonly IDynamicModuleConverter<JobProfile> converter;
        private readonly IDynamicModuleConverter<JobProfileOverloadForWhatItTakes> converterForWITOnly;
        private readonly IDynamicModuleConverter<JobProfileOverloadForSearch> converterForSearchableFieldsOnly;
        private readonly IDynamicContentExtensions dynamicContentExtensions;
        private Dictionary<string, JobProfile> cachedJobProfiles = new Dictionary<string, JobProfile>();

        #endregion Fields

        #region Ctor

        public JobProfileRepository(
            IDynamicModuleRepository<JobProfile> repository,
            IDynamicModuleConverter<JobProfile> converter,
            IDynamicContentExtensions dynamicContentExtensions,
            IDynamicModuleRepository<SocSkillMatrix> socSkillRepository,
            IDynamicModuleConverter<JobProfileOverloadForWhatItTakes> converterLight,
            IDynamicModuleConverter<JobProfileOverloadForSearch> converterForSearchableFieldsOnly)
        {
            this.repository = repository;
            this.converter = converter;
            this.dynamicContentExtensions = dynamicContentExtensions;
            this.socSkillRepository = socSkillRepository;
            this.converterForWITOnly = converterLight;
            this.converterForSearchableFieldsOnly = converterForSearchableFieldsOnly;
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
            var content = repository.Get(item => item.UrlName == urlName && item.Status == (isPublishing ? ContentLifecycleStatus.Master : ContentLifecycleStatus.Live));
            return converterForSearchableFieldsOnly.ConvertFrom(content);
        }

        public IEnumerable<JobProfileOverloadForWhatItTakes> GetLiveJobProfiles()
        {
            var jobProfiles = repository.GetMany(item => item.Status == ContentLifecycleStatus.Live && item.Visible).ToList();

            if (jobProfiles.Any())
            {
                var jobProfileOverloadForWhatItTakesList = new List<JobProfileOverloadForWhatItTakes>();
                    foreach (var jobProfile in jobProfiles)
                    {
                        jobProfileOverloadForWhatItTakesList.Add(converterForWITOnly.ConvertFrom(jobProfile));
                    }

                    return jobProfileOverloadForWhatItTakesList;
            }

            return Enumerable.Empty<JobProfileOverloadForWhatItTakes>();
        }

        public RepoActionResult UpdateSocSkillMatrices(JobProfileOverloadForWhatItTakes jobProfile, IEnumerable<SocSkillMatrix> socSkillMatrices)
        {
            var jobprofile = repository.Get(item =>
                item.UrlName == jobProfile.UrlName && item.Status == ContentLifecycleStatus.Live && item.Visible);

            var skillMatrices = socSkillMatrices as IList<SocSkillMatrix> ?? socSkillMatrices.ToList();
            if (jobprofile != null && skillMatrices.Any())
            {
                var master = repository.GetMaster(jobprofile);

                dynamicContentExtensions.DeleteRelatedFieldValues(master, RelatedSkillField);

                foreach (var socSkillMatrix in skillMatrices)
                {
                    var relatedSocSkillItem = socSkillRepository.Get(d => d.Status == ContentLifecycleStatus.Master && d.UrlName == socSkillMatrix.SfUrlName);
                    if (relatedSocSkillItem != null)
                    {
                        dynamicContentExtensions.SetRelatedFieldValue(master, relatedSocSkillItem, RelatedSkillField);
                    }
                }

                dynamicContentExtensions.SetFieldValue(master, nameof(JobProfile.DigitalSkillsLevel), jobProfile.DigitalSkillsLevel);

                repository.Commit();

                repository.Update(master, UpdateComment);

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