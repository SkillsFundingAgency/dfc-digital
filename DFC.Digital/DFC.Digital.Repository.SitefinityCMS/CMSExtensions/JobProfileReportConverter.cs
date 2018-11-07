using AutoMapper;
using DFC.Digital.Data.Model;
using Telerik.Sitefinity.DynamicModules.Model;

namespace DFC.Digital.Repository.SitefinityCMS.CMSExtensions
{
    public class JobProfileReportConverter : IDynamicModuleConverter<JobProfileReport>
    {
        private readonly IDynamicModuleConverter<CmsReportItem> cmsReportItemConverter;
        private readonly IMapper mapper;

        public JobProfileReportConverter(
           IDynamicModuleConverter<CmsReportItem> cmsReportItemConverter,
            IMapper mapper)
        {
            this.cmsReportItemConverter = cmsReportItemConverter;
            this.mapper = mapper;
        }

        public JobProfileReport ConvertFrom(DynamicContent content)
        {
            var jpReport = new JobProfileReport();

            var cmsReportItem = cmsReportItemConverter.ConvertFrom(content);
            if (cmsReportItem != null)
            {
                jpReport = mapper.Map<JobProfileReport>(cmsReportItem);
                jpReport.Name = jpReport.UrlName;
            }

            return jpReport;
        }
    }
}