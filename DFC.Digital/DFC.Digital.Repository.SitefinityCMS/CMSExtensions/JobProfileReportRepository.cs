using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using System.Linq;
using Telerik.Sitefinity.RelatedData;

namespace DFC.Digital.Repository.SitefinityCMS.CMSExtensions
{
    public class JobProfileReportRepository : IJobProfileReportRepository
    {
        private readonly IDynamicModuleRepository<JobProfile> jobProfileRepository;
        private readonly IDynamicModuleConverter<JobProfileReport> jpReportConverter;
        private readonly IDynamicModuleRepository<SocCode> socCodeRepository;
        private readonly IDynamicModuleConverter<ProfileAndApprenticeshipReport> profileAndApprenticeshipReportConverter;
        private readonly IDynamicContentExtensions dynamicContentExtensions;
        private readonly IDynamicModuleRepository<ApprenticeVacancy> apprenticeVacancyRepository;
        private readonly IDynamicModuleConverter<ApprenticeshipVacancyReport> apprenticeVacancyConverter;
        private readonly ITaxonomyRepository taxonomyRepository;

        public JobProfileReportRepository(
            IDynamicModuleRepository<JobProfile> jobProfileRepository,
            IDynamicModuleConverter<JobProfileReport> jpReportConverter,
            IDynamicModuleRepository<SocCode> socCodeRepository,
            IDynamicModuleConverter<ProfileAndApprenticeshipReport> profileAndApprenticeshipReportConverter,
            IDynamicContentExtensions dynamicContentExtensions,
            IDynamicModuleRepository<ApprenticeVacancy> apprenticeVacancyRepository,
            IDynamicModuleConverter<ApprenticeshipVacancyReport> apprenticeVacancyConverter,
            ITaxonomyRepository taxonomyRepository)
        {
            this.jobProfileRepository = jobProfileRepository;
            this.jpReportConverter = jpReportConverter;
            this.socCodeRepository = socCodeRepository;
            this.profileAndApprenticeshipReportConverter = profileAndApprenticeshipReportConverter;
            this.dynamicContentExtensions = dynamicContentExtensions;
            this.apprenticeVacancyRepository = apprenticeVacancyRepository;
            this.apprenticeVacancyConverter = apprenticeVacancyConverter;
            this.taxonomyRepository = taxonomyRepository;
        }

        public IQueryable<ProfileAndApprenticeshipReport> GetApprenticeshipVacancyReport()
        {
            var allJobProfiles = jobProfileRepository.GetAll();
            allJobProfiles.SetRelatedDataSourceContext();

            var allApprenticeVacancies = apprenticeVacancyRepository.GetAll();
            allApprenticeVacancies.SetRelatedDataSourceContext();

            //var apprenticeships = allApprenticeVacancies.Select(a => apprenticeVacancyConverter.ConvertFrom(a)).ToList();
            var apprenticeships = allApprenticeVacancies.Select(a => apprenticeVacancyConverter.ConvertFrom(a));

            var profiles = allJobProfiles.Select(j => profileAndApprenticeshipReportConverter.ConvertFrom(j));

            foreach (ProfileAndApprenticeshipReport p in profiles)
            {
                if (p.SocCode != null)
                {
                    p.ApprenticeVacancies = apprenticeships.Where(a => (a.SocCode != null || p.SocCode != null) && (a.SocCode.SOCCode == p.SocCode.SOCCode)).ToList();
                }
            }

            return profiles;
        }
    }
}