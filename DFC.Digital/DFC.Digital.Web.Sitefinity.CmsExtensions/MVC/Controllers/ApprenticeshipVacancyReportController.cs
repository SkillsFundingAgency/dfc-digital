using AutoMapper;
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
        private readonly IMapper mapper;

        public ApprenticeshipVacancyReportController(IApplicationLogger loggingService, IJobProfileReportRepository reportRepository, IMapper mapper) : base(loggingService)
        {
            this.reportRepository = reportRepository;
            this.mapper = mapper;
        }

        // GET: AVReport
        public ActionResult Index()
        {
            var watch = Stopwatch.StartNew();
            var result = reportRepository.GetApprenticeshipVacancyReport();
            //var result = GetDummyData().AsQueryable();
            watch.Stop();
            var avvm = new JobProfileApprenticeshipVacancyReportViewModel
            {
                ReportData = CreateReportDataView(result),
                ReportName = $"JobProfilesApprenticeships{DateTime.Now.ToString("ddMMyyyy")}", 
                ExecutionTime = watch.Elapsed,
            };

            return View(avvm);
        }

        private IEnumerable<JobProfileApprenticeshipVacancyItemViewModel> CreateReportDataView(IQueryable<ProfileAndApprenticeshipReport> reportData)
        {
            var reportDataView = new List<JobProfileApprenticeshipVacancyItemViewModel>();
            
            foreach (ProfileAndApprenticeshipReport r in  reportData)
            {
                var reportItem = new JobProfileApprenticeshipVacancyItemViewModel { JobProfileTitle = r.JobProfile.Title, SOC = r.SocCode.SOCCode, SOCDescription = r.SocCode.Description };

                reportItem.Standards = string.Join(",", r.Standards == null ? new List<string>() : r.Standards.ToList());
                reportItem.Frameworks = string.Join(",", r.Frameworks == null ? new List<string>() : r.Frameworks.ToList());

                if (r.ApprenticeVacancies.Count > 0)
                {
                    reportItem.AV1Title = r.ApprenticeVacancies[0].Title;
                    reportItem.AV1DateCreated = r.ApprenticeVacancies[0].DateCreated.ToString("dd/MM/yyyy HH:mm:ss");
                }
                if (r.ApprenticeVacancies.Count > 1)
                {
                    reportItem.AV2Title = r.ApprenticeVacancies[1].Title;
                    reportItem.AV2DateCreated = r.ApprenticeVacancies[1].DateCreated.ToString("dd/MM/yyyy HH:mm:ss");
                }
                reportDataView.Add(reportItem);
            }
            return reportDataView;
        }

        private IList<ProfileAndApprenticeshipReport> GetDummyData()
        {
            var reportData = new List<ProfileAndApprenticeshipReport>();

            for (int ii = 0; ii < 800; ii++)
            {
                var r = new ProfileAndApprenticeshipReport();
                r.JobProfile = new JobProfileReport() { Title = $"Job profile {ii}", Name = $"profile_name{ii}" };
                r.SocCode = new SocCode() { SOCCode = $"SOC {ii}", Description = $"This is SOC {ii}" };
                var frameworks = new List<string>();
                frameworks.Add($"Framework{ii}");
                r.Frameworks = frameworks.AsQueryable();

                var standards = new List<string>();
                standards.Add($"Standards{ii}");
                r.Standards = standards.AsQueryable();

                var a = new List<ApprenticeshipVacancyReport>();

                if ((ii % 2) == 0)
                {
                    a.Add(new ApprenticeshipVacancyReport() { Title = $"Apprenticeship Vacancy {ii}", DateCreated = DateTime.Now.AddDays(-ii) });

                    if ((ii % 3) == 0)
                    {
                        a.Add(new ApprenticeshipVacancyReport() { Title = $"Apprenticeship Vacancy {ii}", DateCreated = DateTime.Now.AddDays(-ii) });
                    }
                }

                r.ApprenticeVacancies = a;

                reportData.Add(r);
            }

            return reportData;

        }
    }
}