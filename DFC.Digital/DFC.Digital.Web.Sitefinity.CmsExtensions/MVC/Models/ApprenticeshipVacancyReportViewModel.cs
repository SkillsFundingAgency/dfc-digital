using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DFC.Digital.Data.Model;

namespace DFC.Digital.Web.Sitefinity.CmsExtensions
{
    public class ApprenticeshipVacancyReportViewModel
    {
        public TimeSpan ExecutionTime { get; set; }
        public IQueryable<ApprenticeshipVacancyReport> ReportData { get; set; }
    }
}