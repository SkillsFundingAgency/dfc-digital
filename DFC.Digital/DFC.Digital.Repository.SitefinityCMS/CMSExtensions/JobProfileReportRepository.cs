using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using System.Linq;

namespace DFC.Digital.Repository.SitefinityCMS.CMSExtensions
{
    public class JobProfileReportRepository : IJobProfileReportRepository
    {
        private readonly IDynamicModuleRepository<JobProfile> jobProfileRepository;
        private readonly IDynamicModuleConverter<JobProfileReport> jpReportConverter;
        private readonly IDynamicModuleRepository<SocCode> socCodeRepository;
        private readonly IDynamicModuleConverter<StandardAndFrameworkReport> standardAndFrameworkConverter;
        private readonly IDynamicContentExtensions dynamicContentExtensions;

        public JobProfileReportRepository(
            IDynamicModuleRepository<JobProfile> jobProfileRepository,
            IDynamicModuleConverter<JobProfileReport> jpReportConverter,
            IDynamicModuleRepository<SocCode> socCodeRepository,
            IDynamicModuleConverter<StandardAndFrameworkReport> standardAndFrameworkConverter,
            IDynamicContentExtensions dynamicContentExtensions)
        {
            this.jobProfileRepository = jobProfileRepository;
            this.jpReportConverter = jpReportConverter;
            this.socCodeRepository = socCodeRepository;
            this.standardAndFrameworkConverter = standardAndFrameworkConverter;
            this.dynamicContentExtensions = dynamicContentExtensions;
        }

        public IQueryable<StandardAndFrameworkReport> GetStandardsAndFrameworksReport()
        {
            var allJobProfiles = jobProfileRepository.GetAll();
            var allJpReports = allJobProfiles.Select(j => jpReportConverter.ConvertFrom(j));
            var allSocCodes = socCodeRepository.GetAll();
            var stdAndFrameworks = allSocCodes.Select(s => standardAndFrameworkConverter.ConvertFrom(s));
            foreach (var item in stdAndFrameworks)
            {
                item.JobProfile = allJpReports.SingleOrDefault(j => j.SocCodeId == item.SocCode.Id);
            }

            return stdAndFrameworks;
        }
    }
}