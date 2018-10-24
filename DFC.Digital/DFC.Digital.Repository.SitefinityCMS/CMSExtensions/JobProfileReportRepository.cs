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
        private readonly IDynamicModuleConverter<ApprenticeshipVacancyReport> apprenticeshipVacancyReportConverter;
        private readonly IDynamicContentExtensions dynamicContentExtensions;
        private readonly ITaxonomyRepository taxonomyRepository;

        public JobProfileReportRepository(
            IDynamicModuleRepository<JobProfile> jobProfileRepository,
            IDynamicModuleConverter<JobProfileReport> jpReportConverter,
            IDynamicModuleRepository<SocCode> socCodeRepository,
            IDynamicModuleConverter<ApprenticeshipVacancyReport> apprenticeshipVacancyReportConverter,
            IDynamicContentExtensions dynamicContentExtensions,
            ITaxonomyRepository taxonomyRepository)
        {
            this.jobProfileRepository = jobProfileRepository;
            this.jpReportConverter = jpReportConverter;
            this.socCodeRepository = socCodeRepository;
            this.apprenticeshipVacancyReportConverter = apprenticeshipVacancyReportConverter;
            this.dynamicContentExtensions = dynamicContentExtensions;
            this.taxonomyRepository = taxonomyRepository;
        }

        public IQueryable<ApprenticeshipVacancyReport> GetApprenticeshipVacancyReport()
        {
            var allJobProfiles = jobProfileRepository.GetAll();
            allJobProfiles.SetRelatedDataSourceContext();

            //var allJpReports = allJobProfiles.Select(j => jpReportConverter.ConvertFrom(j));

            //var allSocCodes = socCodeRepository.GetAll();
            //allSocCodes.SetRelatedDataSourceContext();
            //var allAVReports = allSocCodes.Select(s => apprenticeshipVacancyReportConverter.ConvertFrom(s));

            //foreach (var item in allAVReports)
            //{
            //    item.JobProfile = allJpReports.Single(j => j.SocCode.SOCCode == item.SocCode.SOCCode);
            //}
            return allJobProfiles.Select(j => apprenticeshipVacancyReportConverter.ConvertFrom(j));
        }
    }
}