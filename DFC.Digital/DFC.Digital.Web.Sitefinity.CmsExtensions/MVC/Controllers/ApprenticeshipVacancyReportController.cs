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
            watch.Stop();
            var avvm = new ApprenticeshipVacancyReportViewModel
            {
                ReportData = result,
                ExecutionTime = watch.Elapsed,
            };

            return View(avvm);
        }
    }
}