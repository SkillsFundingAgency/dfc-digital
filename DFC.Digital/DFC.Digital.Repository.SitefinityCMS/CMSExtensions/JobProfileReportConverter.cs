﻿using AutoMapper;
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
            var cmsReportItem = cmsReportItemConverter.ConvertFrom(content);
            var jpReport = cmsReportItem is null ? new JobProfileReport() : mapper.Map<JobProfileReport>(cmsReportItem);
            return jpReport;
        }
    }
}