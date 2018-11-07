using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;

namespace DFC.Digital.Web.Sitefinity.CmsExtensions
{
    public class ApprenticeshipVacancyReportViewModel
    {
        public TimeSpan ExecutionTime { get; set; }
        public IEnumerable<JobProfileApprenticeshipVacancyReport> ReportData { get; set; }
    }
}