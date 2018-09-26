using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using System.Collections.Generic;
using System.Linq;
using Telerik.Sitefinity.GenericContent.Model;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.RelatedData;

namespace DFC.Digital.Repository.SitefinityCMS.Modules
{
    public class JobProfileSocCodeRepository : IJobProfileSocCodeRepository
    {
        #region Fields
        private const string UpdateComment = "Updated via the SkillsFramework import process";
        private readonly IDynamicModuleRepository<SocCode> repository;
        private readonly IDynamicModuleRepository<JobProfile> jpRepository;
        private readonly IDynamicModuleConverter<ApprenticeVacancy> converter;
        private readonly IDynamicModuleConverter<SocCode> socCodeConverter;
        private readonly IDynamicContentExtensions dynamicContentExtensions;
        private readonly IDynamicModuleConverter<JobProfileOverloadForWhatItTakes> converterLight;

        #endregion Fields

        #region Ctor

        public JobProfileSocCodeRepository(IDynamicModuleRepository<SocCode> repository, IDynamicModuleConverter<ApprenticeVacancy> converter, IDynamicModuleConverter<SocCode> socCodeConverter, IDynamicContentExtensions dynamicContentExtensions, IDynamicModuleConverter<JobProfileOverloadForWhatItTakes> converterLight, IDynamicModuleRepository<JobProfile> jpRepository)
        {
            this.repository = repository;
            this.converter = converter;
            this.dynamicContentExtensions = dynamicContentExtensions;
            this.socCodeConverter = socCodeConverter;
            this.converterLight = converterLight;
            this.jpRepository = jpRepository;
        }

        #endregion Ctor

        #region JobProfileSocCodeRepository Implementations

        public IQueryable<ApprenticeVacancy> GetApprenticeVacanciesBySocCode(string socCode)
        {
            var socCodeItem = repository.Get(item => item.Visible && item.Status == ContentLifecycleStatus.Live && item.GetValue<string>(nameof(SocCode.SOCCode)) == socCode);
            var vacancies = dynamicContentExtensions.GetRelatedParentItems(socCodeItem, DynamicTypes.JobProfileApprenticeshipContentType, repository.GetProviderName());

            if (vacancies.Any())
            {
                return vacancies.Select(item => converter.ConvertFrom(item));
            }

            return Enumerable.Empty<ApprenticeVacancy>().AsQueryable();
        }

        public IEnumerable<JobProfileOverloadForWhatItTakes> GetLiveJobProfilesBySocCode(string socCode)
        {
            var socCodeItem = repository.Get(item => item.Status == ContentLifecycleStatus.Live && item.GetValue<string>(nameof(SocCode.SOCCode)) == socCode);

            if (socCodeItem != null)
            {
                var jobProfiles = dynamicContentExtensions.GetRelatedParentItems(socCodeItem, DynamicTypes.JobprofileContentType, repository.GetProviderName());

                if (jobProfiles.Any())
                {
                    var jobProfileOverloadForWhatItTakesList = new List<JobProfileOverloadForWhatItTakes>();
                    foreach (var jobProfile in jobProfiles)
                    {
                        var isCheckedOut = jpRepository.IsCheckedOut(jobProfile);

                        jobProfileOverloadForWhatItTakesList.Add(isCheckedOut ? new JobProfileOverloadForWhatItTakes { UrlName = dynamicContentExtensions.GetFieldValue<Lstring>(jobProfile, nameof(JobProfile.UrlName)), Locked = true } : converterLight.ConvertFrom(jobProfile));
                    }

                    return jobProfileOverloadForWhatItTakesList;
                }
            }

            return Enumerable.Empty<JobProfileOverloadForWhatItTakes>().AsQueryable();
        }

        public RepoActionResult UpdateSocOccupationalCode(SocCode socCode)
        {
            var socCodeItem = repository.Get(item => item.Visible && item.Status == ContentLifecycleStatus.Live && item.UrlName == socCode.UrlName);

            if (socCodeItem != null)
            {
                var master = repository.GetMaster(socCodeItem);

                var temp = repository.GetTemp(master);

                dynamicContentExtensions.SetFieldValue(temp, nameof(SocCode.ONetOccupationalCode), socCode.ONetOccupationalCode);

                var updatedMaster = repository.CheckinTemp(temp);

                repository.Publish(updatedMaster, UpdateComment);
                repository.Commit();
            }

            return new RepoActionResult { Success = true };
        }

        public SocCode GetBySocCode(string socCode)
        {
            var socCodeItem = repository.Get(item => item.Visible && item.Status == ContentLifecycleStatus.Live && item.GetValue<string>(nameof(SocCode.SOCCode)) == socCode);

            return socCodeItem != null ? socCodeConverter.ConvertFrom(socCodeItem) : null;
        }

        public IQueryable<SocCode> GetSocCodes()
        {
            var socCodes = repository.GetMany(item => item.Visible && item.Status == ContentLifecycleStatus.Live);

            if (socCodes.Any())
            {
                return socCodes.Select(item => socCodeConverter.ConvertFrom(item));
            }

            return Enumerable.Empty<SocCode>().AsQueryable();
        }

        #endregion JobProfileSocCodeRepository Implementations
    }
}