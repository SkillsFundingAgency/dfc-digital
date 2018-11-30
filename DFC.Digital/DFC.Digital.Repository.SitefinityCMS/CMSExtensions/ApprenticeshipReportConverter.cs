using AutoMapper;
using DFC.Digital.Data.Model;
using Telerik.Sitefinity.DynamicModules.Model;

namespace DFC.Digital.Repository.SitefinityCMS.CMSExtensions
{
    public class ApprenticeshipReportConverter : IDynamicModuleConverter<ApprenticeshipVacancyReport>
    {
        private const string SocCodePropertyName = "SOCCode";
        private readonly IDynamicModuleConverter<SocCodeReport> socCodeReportConverter;
        private readonly IDynamicContentExtensions dynamicContentExtensions;
        private readonly IDynamicModuleConverter<CmsReportItem> cmsReportItemConverter;
        private readonly IMapper mapper;

        public ApprenticeshipReportConverter(
            IDynamicModuleConverter<SocCodeReport> socCodeReportConverter,
            IDynamicContentExtensions dynamicContentExtensions,
            IDynamicModuleConverter<CmsReportItem> cmsReportItemConverter,
            IMapper mapper)
        {
            this.socCodeReportConverter = socCodeReportConverter;
            this.dynamicContentExtensions = dynamicContentExtensions;
            this.mapper = mapper;
            this.cmsReportItemConverter = cmsReportItemConverter;
        }

        public ApprenticeshipVacancyReport ConvertFrom(DynamicContent content)
        {
            var cmsReportItem = cmsReportItemConverter.ConvertFrom(content);
            var avReport = cmsReportItem is null ? new ApprenticeshipVacancyReport() : mapper.Map<ApprenticeshipVacancyReport>(cmsReportItem);
            var socItem = dynamicContentExtensions.GetFieldValue<DynamicContent>(content, SocCodePropertyName);
            if (socItem != null)
            {
                avReport.SocCode = socCodeReportConverter.ConvertFrom(socItem);
            }

            return avReport;
        }
    }
}