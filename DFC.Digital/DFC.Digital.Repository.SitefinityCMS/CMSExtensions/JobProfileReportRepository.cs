using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using System.Collections.Generic;
using System.Linq;
using Telerik.Sitefinity.GenericContent.Model;

namespace DFC.Digital.Repository.SitefinityCMS.CMSExtensions
{
    public class JobProfileReportRepository : IJobProfileReportRepository
    {
        private readonly IDynamicModuleRepository<JobProfile> jobProfileRepository;
        private readonly IDynamicModuleConverter<JobProfileApprenticeshipVacancyReport> jobProfileApprenticeshipVacancyReportConverter;
        private readonly IDynamicModuleRepository<ApprenticeVacancy> apprenticeVacancyRepository;
        private readonly IDynamicModuleConverter<ApprenticeshipVacancyReport> apprenticeVacancyConverter;
        private readonly IDynamicContentExtensions dynamicContentExtensions;

        public JobProfileReportRepository(
            IDynamicModuleRepository<JobProfile> jobProfileRepository,
            IDynamicModuleConverter<JobProfileApprenticeshipVacancyReport> jobProfileApprenticeshipVacancyReportConverter,
            IDynamicModuleRepository<ApprenticeVacancy> apprenticeVacancyRepository,
            IDynamicModuleConverter<ApprenticeshipVacancyReport> apprenticeVacancyConverter,
            IDynamicContentExtensions dynamicContentExtensions)
        {
            this.jobProfileRepository = jobProfileRepository;
            this.jobProfileApprenticeshipVacancyReportConverter = jobProfileApprenticeshipVacancyReportConverter;
            this.apprenticeVacancyRepository = apprenticeVacancyRepository;
            this.apprenticeVacancyConverter = apprenticeVacancyConverter;
            this.dynamicContentExtensions = dynamicContentExtensions;
        }

        public IEnumerable<JobProfileApprenticeshipVacancyReport> GetJobProfileApprenticeshipVacancyReport()
        {
            var allJobProfiles = jobProfileRepository.GetMany(x => x.Status == ContentLifecycleStatus.Master);
            dynamicContentExtensions.SetRelatedDataSourceContext(allJobProfiles);

            var allApprenticeVacancies = apprenticeVacancyRepository.GetMany(x => x.Status == ContentLifecycleStatus.Master);
            dynamicContentExtensions.SetRelatedDataSourceContext(allApprenticeVacancies);

            //CodeReview: Why ToList()
            // To avoid Connection reader error, as the process can be long running sitefinity suggest we store list in memory
            // https://knowledgebase.progress.com/articles/Article/invalid-attempt-to-call-isdbnull-when-reader-is-closed-exception
            var apprenticeships = allApprenticeVacancies.Select(a => apprenticeVacancyConverter.ConvertFrom(a)).ToList();
            var profiles = allJobProfiles.Select(j => jobProfileApprenticeshipVacancyReportConverter.ConvertFrom(j)).ToList();

            profiles.Where(p => p.SocCode != null).ToList().ForEach(p => p.ApprenticeshipVacancies = apprenticeships.Where(av => av.SocCode != null && av.SocCode.Id == p.SocCode.Id));

            return profiles;
        }
    }
}