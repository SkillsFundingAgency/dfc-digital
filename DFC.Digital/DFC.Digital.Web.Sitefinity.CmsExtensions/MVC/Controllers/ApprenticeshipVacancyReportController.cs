using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Core;
using DFC.Digital.Web.Sitefinity.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using Telerik.Sitefinity.Mvc;

namespace DFC.Digital.Web.Sitefinity.CmsExtensions.MVC.Controllers
{
    [ControllerToolboxItem(Name = "ApprenticeshipVacancyReport", Title = "Apprenticeship Vacancy Report Widget", SectionName = SitefinityConstants.CustomReportsWidgetSection)]
    public class ApprenticeshipVacancyReportController : BaseDfcController
    {
        private readonly IJobProfileReportRepository reportRepository;

        public ApprenticeshipVacancyReportController(IApplicationLogger loggingService, IJobProfileReportRepository reportRepository) : base(loggingService)
        {
            this.reportRepository = reportRepository;
        }

        // GET: AVReport
        public ActionResult Index()
        {
            var watch = Stopwatch.StartNew();
            var result = reportRepository.GetJobProfileApprenticeshipVacancyReport();
            watch.Stop();
            var avvm = new JobProfileApprenticeshipVacancyReportViewModel
            {
                ReportData = CreateReportDataView(result),
                ReportName = $"JobProfilesApprenticeships{DateTime.Now.ToString("ddMMyyyy")}",
                ExecutionTime = watch.Elapsed,
            };

            return View(avvm);
        }

        private IEnumerable<JobProfileApprenticeshipVacancyItemViewModel> CreateReportDataView(IEnumerable<JobProfileApprenticeshipVacancyReport> reportData)
        {
            var reportDataView = new List<JobProfileApprenticeshipVacancyItemViewModel>();

            foreach (var rptItem in reportData)
            {
                var reportItem = new JobProfileApprenticeshipVacancyItemViewModel
                {
                    JobProfileTitle = rptItem.JobProfile.Title,
                    JoProfileLastModifiedBy = rptItem.JobProfile.LastModifiedBy,
                    JobProfileStatus = rptItem.JobProfile.Status.ToString(),
                    SOC = rptItem.SocCode.SOCCode,
                    SOCDescription = rptItem.SocCode.Description,
                    JobProfileLink = rptItem.JobProfile.Name
                };

                reportItem.Standards = string.Join(",", rptItem.SocCode.Standards is null ? Enumerable.Empty<string>().AsQueryable() : rptItem.SocCode.Standards.Select(s => $"{s.Title}-({s.LarsCode})"));
                reportItem.Frameworks = string.Join(",", rptItem.SocCode.Frameworks is null ? Enumerable.Empty<string>().AsQueryable() : rptItem.SocCode.Frameworks.Select(s => $"{s.Title}-({s.LarsCode})"));

                if (rptItem.ApprenticeshipVacancies.Any())
                {
                    reportItem.AV1Title = rptItem.ApprenticeshipVacancies.First().Title;
                    reportItem.AV1LastModified = rptItem.ApprenticeshipVacancies.First().LastModified.ToString();
                }

                if (rptItem.ApprenticeshipVacancies.Count() > 1)
                {
                    reportItem.AV2Title = rptItem.ApprenticeshipVacancies.Skip(1).First().Title;
                    reportItem.AV2LastModified = rptItem.ApprenticeshipVacancies.Skip(1).First().LastModified.ToString();
                }

                reportDataView.Add(reportItem);
            }

            return reportDataView;
        }
    }
}