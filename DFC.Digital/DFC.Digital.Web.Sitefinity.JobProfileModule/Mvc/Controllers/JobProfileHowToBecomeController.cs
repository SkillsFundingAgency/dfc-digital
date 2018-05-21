using AutoMapper;
using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.Core;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Telerik.Sitefinity.Mvc;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Controllers
{
    /// <summary>
    /// Custom Widget for the Search box and Search results for Job Profiles
    /// </summary>
    /// <seealso cref="DFC.Digital.Web.Core.BaseDfcController" />
    [ControllerToolboxItem(Name = "JobProfileHowToBecome", Title = "JobProfile HowToBecome", SectionName = SitefinityConstants.CustomWidgetSection)]
    public class JobProfileHowToBecomeController : BaseJobProfileWidgetController
    {
        #region Private Fields

        private readonly IMapper mapper;

        #endregion Private Fields

        #region Constructors

        public JobProfileHowToBecomeController(
            IWebAppContext webAppContext,
            IJobProfileRepository jobProfileRepository,
            IApplicationLogger applicationLogger,
            ISitefinityPage sitefinityPage,
            IMapper mapper)
            : base(webAppContext, jobProfileRepository, applicationLogger, sitefinityPage)
        {
            this.mapper = mapper;
        }

        #endregion Constructors

        #region Public Properties

        [DisplayName("Salary Text")]
        public string SalaryText { get; set; } = "Average salary";

        [DisplayName("Salary Text (Span)")]
        public string SalaryTextSpan { get; set; } = "(per year)";

        [DisplayName("Text when Salary does not have values. If you change this value, you will also need to change the reciprocal value in JobProfileSearchBox widget on 'Search results' page.")]
        public string SalaryBlankText { get; set; } = "Variable";

        [DisplayName("Text for Salary Starter")]
        public string SalaryStarterText { get; set; } = "Starter";

        [DisplayName("Text for Salary Experienced")]
        public string SalaryExperiencedText { get; set; } = "Experienced";

        [DisplayName("Link for salary context")]
        public string SalaryContextLink { get; set; }

        [DisplayName("Text for salary context link")]
        public string SalaryContextText { get; set; }

        [DisplayName("Hours Text")]
        public string HoursText { get; set; } = "Typical hours";

        [DisplayName("Max and Min hours set to blank text")]
        public string MaxAndMinHoursAreBlankText { get; set; } = "Variable";

        [DisplayName("Hours time period")]
        public string HoursTimePeriodText { get; set; }

        [DisplayName("Working Pattern Text")]
        public string WorkingPatternText { get; set; } = "You could work";

        [DisplayName("Working Pattern Span Text")]
        public string WorkingPatternSpanText { get; set; }

        #endregion Public Properties

        #region Actions

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>Action Result</returns>
        [HttpGet]
        [RelativeRoute("")]
        public ActionResult Index()
        {
            return BaseIndex();
        }

        /// <summary>
        /// Indexes the specified urlname.
        /// </summary>
        /// <param name="urlName">The urlname.</param>
        /// <returns>Action Result</returns>
        [HttpGet]
        [RelativeRoute("{urlName}")]
        public ActionResult Index(string urlName)
        {
            return BaseIndex(urlName);
        }

        protected override ActionResult GetDefaultView()
        {
            return GetJobProfileDetailsView();
        }

        protected override ActionResult GetEditorView()
        {
            // Sitefinity cannot handle async very well. So initialising it on current UI thread.
            var model = mapper.Map<JobProfileHowToBecomeViewModel>(CurrentJobProfile);
            return GetJobProfileDetailsView(model);
        }

        /// <summary>
        /// Gets the job profile details view.
        /// </summary>
        /// <param name="model">Job profile view model</param>
        /// <returns>Index View</returns>
        private ActionResult GetJobProfileDetailsView(JobProfileHowToBecomeViewModel model = null)
        {
            if (model == null)
            {
                model = mapper.Map<JobProfileHowToBecomeViewModel>(CurrentJobProfile);
            }

            //Map all the related Jobprofie fieldsin the model
            model.SalaryText = SalaryText;
            model.SalaryTextSpan = SalaryTextSpan;
            model.SalaryBlankText = SalaryBlankText;
            model.SalaryStarterText = SalaryStarterText;
            model.SalaryExperiencedText = SalaryExperiencedText;
            model.SalaryContextLink = SalaryContextLink;
            model.SalaryContextText = SalaryContextText;
            model.HoursText = HoursText;
            model.MaxAndMinHoursAreBlankText = MaxAndMinHoursAreBlankText;
            model.HoursTimePeriodText = HoursTimePeriodText;
            model.WorkingPatternText = WorkingPatternText;
            model.WorkingPatternSpanText = WorkingPatternSpanText;

            return View("Index", model);
        }

        #endregion Actions
    }
}