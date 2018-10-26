using DFC.Digital.Core;
using DFC.Digital.Data.Model;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Model;

namespace DFC.Digital.Repository.SitefinityCMS.CMSExtensions
{
    public class ApprenticeshipReportConverter : IDynamicModuleConverter<ApprenticeshipVacancyReport>
    {
        private readonly IDynamicModuleConverter<SocCode> socCodeConverter;
        private readonly IDynamicContentExtensions dynamicContentExtensions;

        public ApprenticeshipReportConverter(
            IDynamicModuleConverter<SocCode> socCodeConverter,
            IDynamicContentExtensions dynamicContentExtensions)
        {
            this.socCodeConverter = socCodeConverter;
            this.dynamicContentExtensions = dynamicContentExtensions;
        }

        public ApprenticeshipVacancyReport ConvertFrom(DynamicContent content)
        {
            var reportItem = new ApprenticeshipVacancyReport();
            reportItem.Title = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(ApprenticeshipVacancyReport.Title));
            reportItem.Name = content.UrlName;
            var socItem = dynamicContentExtensions.GetFieldValue<DynamicContent>(content, "SOCCode");
            if (socItem != null)
            {
                reportItem.SocCode = socCodeConverter.ConvertFrom(socItem);
            }

            return reportItem;
        }
    }
}