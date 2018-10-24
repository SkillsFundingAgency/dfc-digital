using DFC.Digital.Core;
using DFC.Digital.Data.Model;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Model;

namespace DFC.Digital.Repository.SitefinityCMS.CMSExtensions
{
    public class JobProfileReportConverter : IDynamicModuleConverter<JobProfileReport>
    {
        private readonly IDynamicModuleConverter<SocCode> socCodeConverter;
        private readonly IDynamicContentExtensions dynamicContentExtensions;

        public JobProfileReportConverter(
            IDynamicModuleConverter<SocCode> socCodeConverter,
            IDynamicContentExtensions dynamicContentExtensions)
        {
            this.socCodeConverter = socCodeConverter;
            this.dynamicContentExtensions = dynamicContentExtensions;
        }

        public JobProfileReport ConvertFrom(DynamicContent content)
        {
            var report = new JobProfileReport();
            report.Title = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(JobProfile.Title));
            report.Name = content.UrlName;

            //var socItem = content.GetValue<DynamicContent>(Constants.SocField);
            //if (socItem != null)
            //{
            //    report.SocCode = socCodeConverter.ConvertFrom(socItem);
            //}
            return report;
        }
    }
}