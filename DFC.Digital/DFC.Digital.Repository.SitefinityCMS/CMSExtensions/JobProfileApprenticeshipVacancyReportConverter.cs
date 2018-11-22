using DFC.Digital.Core;
using DFC.Digital.Data.Model;
using Telerik.Sitefinity.DynamicModules.Model;

namespace DFC.Digital.Repository.SitefinityCMS.CMSExtensions
{
    public class JobProfileApprenticeshipVacancyReportConverter : IDynamicModuleConverter<JobProfileApprenticeshipVacancyReport>
    {
        private readonly IDynamicModuleConverter<SocCodeReport> socCodeConverter;
        private readonly IDynamicModuleConverter<JobProfileReport> jobProfileReportConverter;
        private readonly IDynamicContentExtensions dynamicContentExtensions;

        public JobProfileApprenticeshipVacancyReportConverter(
            IDynamicModuleConverter<SocCodeReport> socCodeConverter,
            IDynamicModuleConverter<JobProfileReport> jobProfileReportConverter,
            IDynamicContentExtensions dynamicContentExtensions)
        {
            this.socCodeConverter = socCodeConverter;
            this.jobProfileReportConverter = jobProfileReportConverter;
            this.dynamicContentExtensions = dynamicContentExtensions;
        }

        public JobProfileApprenticeshipVacancyReport ConvertFrom(DynamicContent content)
        {
            var avReportItem = new JobProfileApprenticeshipVacancyReport
            {
                JobProfile = jobProfileReportConverter.ConvertFrom(content)
            };

            var socItem = dynamicContentExtensions.GetFieldValue<DynamicContent>(content, Constants.SocField);
            if (socItem != null)
            {
                avReportItem.SocCode = socCodeConverter.ConvertFrom(socItem);
            }

            return avReportItem;
        }
    }
}