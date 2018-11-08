using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Sitefinity.GenericContent.Model;
using Telerik.Sitefinity.RelatedData;

namespace DFC.Digital.Repository.SitefinityCMS.CMSExtensions
{
    public class JobProfileReportRepository : IJobProfileReportRepository
    {
        private readonly IDynamicModuleRepository<JobProfile> jobProfileRepository;

        private readonly IDynamicModuleConverter<JobProfileApprenticeshipVacancyReport> jobProfileApprenticeshipVacancyReportConverter;
        private readonly IDynamicModuleRepository<ApprenticeVacancy> apprenticeVacancyRepository;
        private readonly IDynamicModuleConverter<ApprenticeshipVacancyReport> apprenticeVacancyConverter;

        public JobProfileReportRepository(
            IDynamicModuleRepository<JobProfile> jobProfileRepository,
            IDynamicModuleConverter<JobProfileApprenticeshipVacancyReport> profileAndApprenticeshipReportConverter,
            IDynamicModuleRepository<ApprenticeVacancy> apprenticeVacancyRepository,
            IDynamicModuleConverter<ApprenticeshipVacancyReport> apprenticeVacancyConverter)
        {
            this.jobProfileRepository = jobProfileRepository;
            this.jobProfileApprenticeshipVacancyReportConverter = profileAndApprenticeshipReportConverter;
            this.apprenticeVacancyRepository = apprenticeVacancyRepository;
            this.apprenticeVacancyConverter = apprenticeVacancyConverter;
        }

        public IEnumerable<JobProfileApprenticeshipVacancyReport> JobProfileApprenticeshipVacancyReport()
        {
            var allJobProfiles = jobProfileRepository.GetAll().Where(x => x.Status == ContentLifecycleStatus.Master);
            allJobProfiles.SetRelatedDataSourceContext();

            var allApprenticeVacancies = apprenticeVacancyRepository.GetAll().Where(x => x.Status == ContentLifecycleStatus.Master);
            allApprenticeVacancies.SetRelatedDataSourceContext();

            var apprenticeships = allApprenticeVacancies.Select(a => apprenticeVacancyConverter.ConvertFrom(a)).ToList();
            var profiles = allJobProfiles.Select(j => jobProfileApprenticeshipVacancyReportConverter.ConvertFrom(j)).ToList();

            profiles.Where(p => p.SocCode != null).ToList().ForEach(p => p.ApprenticeshipVacancies = apprenticeships.Where(av => av.SocCode != null && av.SocCode.SOCCode == p.SocCode.SOCCode));

            return profiles;
        }
    }
}