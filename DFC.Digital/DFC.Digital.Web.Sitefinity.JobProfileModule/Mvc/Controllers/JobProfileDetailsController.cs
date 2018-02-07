using AutoMapper;
using DFC.Digital.Core.Utilities;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.Core.Interface;
using DFC.Digital.Web.Sitefinity.Core.Utility;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Web.Mvc;
using Telerik.Sitefinity.Mvc;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Controllers
{
    /// <summary>
    /// Custom Widget for the Search box and Search results for Job Profiles
    /// </summary>
    /// <seealso cref="DFC.Digital.Web.Core.Base.BaseDfcController" />
    [ControllerToolboxItem(Name = "JobProfileDetails", Title = "JobProfile Details", SectionName = SitefinityConstants.CustomWidgetSection)]
    public class JobProfileDetailsController : BaseJobProfileWidgetController
    {
        #region Private Fields

        private readonly IMapper mapper;
        private readonly IWebAppContext webAppContext;
        private readonly ISalaryService salaryService;
        private readonly ISalaryCalculator salaryCalculator;
        private readonly IAsyncHelper asyncHelper;

        #endregion Private Fields

        #region Constructors

        public JobProfileDetailsController(
            IWebAppContext webAppContext,
            IJobProfileRepository jobProfileRepository,
            IApplicationLogger applicationLogger,
            ISitefinityPage sitefinityPage,
            IMapper mapper,
            ISalaryService salaryService,
            ISalaryCalculator salaryCalculator,
            IAsyncHelper asyncHelper)
            : base(webAppContext, jobProfileRepository, applicationLogger, sitefinityPage)
        {
            this.mapper = mapper;
            this.webAppContext = webAppContext;
            this.salaryService = salaryService;
            this.salaryCalculator = salaryCalculator;
            this.asyncHelper = asyncHelper;
        }

        #endregion Constructors

        #region Public Properties

        [DisplayName("Salary Text")]
        public string SalaryText { get; set; } = "Average salary";

        [DisplayName("Salary Text (Span)")]
        public string SalaryTextSpan { get; set; } = "(per year)";

        [DisplayName("Text when Salary does not have values")]
        public string SalaryBlankText { get; set; } = "Variable";

        [DisplayName("Text for Salary Starter")]
        public string SalaryStarterText { get; set; } = "Starter";

        [DisplayName("Text for Salary Experienced")]
        public string SalaryExperiencedText { get; set; } = "Experienced";

        [DisplayName("Hours Text")]
        public string HoursText { get; set; } = "Typical hours";

        [DisplayName("Max and Min hours set to blank text")]
        public string MaxAndMinHoursAreBlankText { get; set; } = "Variable";

        [DisplayName("Hours time period")]
        public string HoursTimePeriodText { get; set; } = "per week";

        [DisplayName("Working Pattern Text")]
        public string WorkingPatternText { get; set; } = "You could work"; //"Working Pattern";

        [DisplayName("Working Pattern Span Text")]
        public string WorkingPatternSpanText { get; set; } = string.Empty; //"(you could also work)";

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
        /// <param name="urlname">The urlname.</param>
        /// <returns>Action Result</returns>
        [HttpGet]
        [RelativeRoute("{urlname}")]
        public ActionResult Index(string urlname)
        {
            GetAndSetVocPersonalisationCookie(urlname);

            return BaseIndex(urlname);
        }

        protected override ActionResult GetDefaultView()
        {
            return asyncHelper.Synchronise(() => GetJobProfileDetailsViewAsync());
        }

        protected override ActionResult GetEditorView()
        {
            // Sitefinity cannot handle async very well. So initialising it on current UI thread.
            JobProfileDetailsViewModel model = mapper.Map<JobProfileDetailsViewModel>(CurrentJobProfile);
            return asyncHelper.Synchronise(() => GetJobProfileDetailsViewAsync(model));
        }

        private void GetAndSetVocPersonalisationCookie(string urlname)
        {
            if (!string.IsNullOrWhiteSpace(urlname))
            {
                webAppContext.SetVocCookie(Constants.VocPersonalisationCookieName, new VocSurveyPersonalisation
                {
                    Personalisation = new Dictionary<string, string>
                    {
                        { Constants.LastVisitedJobProfileKey, urlname }
                    }
                });
            }
        }

        /// <summary>
        /// Gets the job profile details view.
        /// </summary>
        /// <param name="model">Job profile view model</param>
        /// <returns>Index View</returns>
        private async Task<ActionResult> GetJobProfileDetailsViewAsync(JobProfileDetailsViewModel model = null)
        {
            if (model == null)
            {
                model = mapper.Map<JobProfileDetailsViewModel>(CurrentJobProfile);
            }

            //Map all the related Jobprofie fieldsin the model
            model.SalaryText = SalaryText;
            model.SalaryTextSpan = SalaryTextSpan;
            model.SalaryBlankText = SalaryBlankText;
            model.SalaryStarterText = SalaryStarterText;
            model.SalaryExperiencedText = SalaryExperiencedText;
            model.HoursText = HoursText;
            model.MaxAndMinHoursAreBlankText = MaxAndMinHoursAreBlankText;
            model.HoursTimePeriodText = HoursTimePeriodText;
            model.WorkingPatternText = WorkingPatternText;
            model.WorkingPatternSpanText = WorkingPatternSpanText;

            if (model.IsLMISalaryFeedOverriden != true)
            {
                model = await PopulateSalaryAsync(model);
            }

            if (model.IsLMISalaryFeedOverriden != true)
            {
                model = await PopulateSalaryAsync(model);
            }

            return View("Index", model);
        }

        private async Task<JobProfileDetailsViewModel> PopulateSalaryAsync(JobProfileDetailsViewModel model)
        {
            var salary = await salaryService.GetSalaryBySocAsync(CurrentJobProfile.SOCCode);

            model.SalaryStarter = salaryCalculator.GetStarterSalary(salary);
            model.SalaryExperienced = salaryCalculator.GetExperiencedSalary(salary);

            return model;
        }

        #endregion Actions
    }
}