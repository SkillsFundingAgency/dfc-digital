using AutoMapper;
using DFC.Digital.Data.Model;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Model;

namespace DFC.Digital.Repository.SitefinityCMS.Modules
{
    public class SocCodeReportConverter : IDynamicModuleConverter<SocCodeReport>
    {
        private const string ApprenticeshipFrameworks = "apprenticeshipframeworks";
        private const string ApprenticeshipFrameworksTaxonomyName = "apprenticeship-frameworks";
        private const string ApprenticeshipStandardsRelatedField = "apprenticeshipstandards";
        private const string ApprenticeshipStandardsTaxonomyName = "apprenticeship-standards";
        private readonly IDynamicContentExtensions dynamicContentExtensions;
        private readonly IRelatedClassificationsRepository relatedClassificationsRepository;
        private readonly IDynamicModuleConverter<CmsReportItem> cmsReportItemConverter;
        private readonly IMapper mapper;

        public SocCodeReportConverter(
            IDynamicContentExtensions dynamicContentExtensions,
            IRelatedClassificationsRepository relatedClassificationsRepository,
            IDynamicModuleConverter<CmsReportItem> cmsReportItemConverter,
            IMapper mapper)
        {
            this.dynamicContentExtensions = dynamicContentExtensions;
            this.relatedClassificationsRepository = relatedClassificationsRepository;
            this.mapper = mapper;
            this.cmsReportItemConverter = cmsReportItemConverter;
        }

        public SocCodeReport ConvertFrom(DynamicContent content)
        {
            var socCodeReport = new SocCodeReport();
            var cmsReportItem = cmsReportItemConverter.ConvertFrom(content);
            if (cmsReportItem != null)
            {
                socCodeReport = mapper.Map<SocCodeReport>(cmsReportItem);
            }

            socCodeReport.Description = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(SocCode.Description));
            socCodeReport.SOCCode = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(SocCode.SOCCode));
            socCodeReport.Frameworks = relatedClassificationsRepository.GetRelatedCmsReportClassifications(content, ApprenticeshipFrameworks, ApprenticeshipFrameworksTaxonomyName);
            socCodeReport.Standards = relatedClassificationsRepository.GetRelatedCmsReportClassifications(content, ApprenticeshipStandardsRelatedField, ApprenticeshipStandardsTaxonomyName);
            return socCodeReport;
        }
    }
}