using System;
using System.Collections.Generic;


namespace DFC.Digital.Web.Sitefinity.CmsExtensions
{
    public class JobProfileApprenticeshipVacancyReportViewModel
    {
        public TimeSpan ExecutionTime { get; set; }
        public IEnumerable<JobProfileApprenticeshipVacancyItemViewModel> ReportData { get; set; }
    }
}