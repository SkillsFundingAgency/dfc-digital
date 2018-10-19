using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Model;

namespace DFC.Digital.Repository.SitefinityCMS.CMSExtensions
{
    public class JobProfileReportConverter : IDynamicModuleConverter<JobProfileReport>
    {
        private readonly IDynamicContentExtensions dynamicContentExtensions;

        public JobProfileReportConverter(IDynamicContentExtensions dynamicContentExtensions)
        {
            this.dynamicContentExtensions = dynamicContentExtensions;
        }

        public JobProfileReport ConvertFrom(DynamicContent content)
        {
            return new JobProfileReport
            {
                Title = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(JobProfile.Title)),
                Name = content.UrlName,
            };
        }
    }
}
