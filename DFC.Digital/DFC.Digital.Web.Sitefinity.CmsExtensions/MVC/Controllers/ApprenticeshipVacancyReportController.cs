using AutoMapper;
using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Web.Core;
using DFC.Digital.Web.Sitefinity.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DFC.Digital.Data.Model;
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
            var result = reportRepository.JobProfileApprenticeshipVacancyReport();
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
            
            foreach (var rptItem in  reportData)
            {
                var reportItem = new JobProfileApprenticeshipVacancyItemViewModel { JobProfileTitle = rptItem.JobProfile.Title, JoProfileLastModifiedBy = rptItem.JobProfile.LastModifiedBy, JobProfileStatus = rptItem.JobProfile.Status.ToString(), SOC = rptItem.SocCode.SOCCode, SOCDescription = rptItem.SocCode.Description };

                reportItem.Standards = string.Join(",", rptItem.SocCode.Standards == null ? new List<string>() : rptItem.SocCode.Standards.ToList());
                reportItem.Frameworks = string.Join(",", rptItem.SocCode.Frameworks == null ? new List<string>() : rptItem.SocCode.Frameworks.ToList());

                if (rptItem.ApprenticeshipVacancies.Any())
                {
                    reportItem.AV1Title = rptItem.ApprenticeshipVacancies.First().Title;
                    reportItem.AV1DateCreated = rptItem.ApprenticeshipVacancies.First().DateCreated.ToString("dd/MM/yyyy HH:mm:ss");
                   
                }
                if (rptItem.ApprenticeshipVacancies.Count() > 1)
                {
                    reportItem.AV2Title = rptItem.ApprenticeshipVacancies.Skip(1).First().Title;
                    reportItem.AV2DateCreated = rptItem.ApprenticeshipVacancies.Skip(1).First().DateCreated.ToString("dd/MM/yyyy HH:mm:ss");
                }
                reportDataView.Add(reportItem);
            }
            return reportDataView;
        }

        //private IList<ProfileAndApprenticeshipReport> GetDummyData()
        //{
        //    var reportData = new List<ProfileAndApprenticeshipReport>();

        //    for (int ii = 0; ii < 800; ii++)
        //    {
        //        var r = new ProfileAndApprenticeshipReport();
        //        r.JobProfile = new JobProfileReport() { Title = $"Job profile {ii}", Name = $"profile_name{ii}" };
        //        r.SocCode = new SocCode() { SOCCode = $"SOC {ii}", Description = $"This is SOC {ii}" };
        //        var frameworks = new List<string>();
        //        frameworks.Add($"Framework{ii}");
        //        r.Frameworks = frameworks.AsQueryable();

        //        var standards = new List<string>();
        //        standards.Add($"Standards{ii}");
        //        r.Standards = standards.AsQueryable();

        //        var a = new List<ApprenticeshipVacancyReport>();

        //        if ((ii % 2) == 0)
        //        {
        //            a.Add(new ApprenticeshipVacancyReport() { Title = $"Apprenticeship Vacancy {ii}", DateCreated = DateTime.Now.AddDays(-ii) });

        //            if ((ii % 3) == 0)
        //            {
        //                a.Add(new ApprenticeshipVacancyReport() { Title = $"Apprenticeship Vacancy {ii}", DateCreated = DateTime.Now.AddDays(-ii) });
        //            }
        //        }

        //        r.ApprenticeVacancies = a;

        //        reportData.Add(r);
        //    }

        //    return reportData;

        //}
    }
}