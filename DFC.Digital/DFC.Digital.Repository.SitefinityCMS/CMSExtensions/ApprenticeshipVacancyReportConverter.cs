using DFC.Digital.Core;
using DFC.Digital.Data.Model;
using Telerik.Sitefinity.DynamicModules.Model;

namespace DFC.Digital.Repository.SitefinityCMS.CMSExtensions
{
    public class ApprenticeshipVacancyReportConverter : IDynamicModuleConverter<ApprenticeshipVacancyReport>
    {
        private const string ApprenticeshipFrameworks = "apprenticeshipframeworks";
        private const string ApprenticeshipFrameworksTaxonomyName = "apprenticeship-frameworks";
        private const string ApprenticeshipStandardsRelatedField = "apprenticeshipstandards";
        private const string ApprenticeshipStandardsTaxonomyName = "apprenticeship-standards";

        private readonly IDynamicModuleConverter<SocCode> socCodeConverter;
        private readonly IDynamicModuleConverter<JobProfileReport> jobProfileReportConverter;
        private readonly IRelatedClassificationsRepository relatedClassificationsRepository;
        private readonly IDynamicContentExtensions dynamicContentExtensions;

        public ApprenticeshipVacancyReportConverter(
            IDynamicModuleConverter<SocCode> socCodeConverter,
            IDynamicModuleConverter<JobProfileReport> jobProfileReportConverter,
            IRelatedClassificationsRepository relatedClassificationsRepository,
            IDynamicContentExtensions dynamicContentExtensions)
        {
            this.socCodeConverter = socCodeConverter;
            this.jobProfileReportConverter = jobProfileReportConverter;
            this.relatedClassificationsRepository = relatedClassificationsRepository;
            this.dynamicContentExtensions = dynamicContentExtensions;
        }

        public ApprenticeshipVacancyReport ConvertFrom(DynamicContent content)
        {
            var avReportItem = new ApprenticeshipVacancyReport
            {
                JobProfile = jobProfileReportConverter.ConvertFrom(content)
            };

            var socItem = dynamicContentExtensions.GetFieldValue<DynamicContent>(content, Constants.SocField);
            if (socItem != null)
            {
                avReportItem.SocCode = socCodeConverter.ConvertFrom(socItem);

                //avReportItem.SocCode = socCodeConverter.ConvertFrom(content);
                avReportItem.Frameworks = relatedClassificationsRepository.GetRelatedClassifications(socItem, ApprenticeshipFrameworks, ApprenticeshipFrameworksTaxonomyName);
                avReportItem.Standards = relatedClassificationsRepository.GetRelatedClassifications(socItem, ApprenticeshipStandardsRelatedField, ApprenticeshipStandardsTaxonomyName);
            }

            return avReportItem;
        }
    }
}