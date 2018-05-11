using AutoMapper;
using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.Core;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Telerik.Sitefinity.Mvc;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Controllers
{
    /// <summary>
    /// Custom Widget for the Search box and Search results for Job Profiles
    /// </summary>
    /// <seealso cref="DFC.Digital.Web.Core.BaseDfcController" />
    [ControllerToolboxItem(Name = "JobProfileDetails", Title = "JobProfile Details", SectionName = SitefinityConstants.CustomWidgetSection)]
    public class JobProfileDetailsController : BaseJobProfileWidgetController
    {
        #region Private Fields

        private readonly IMapper mapper;
        private readonly ISearchQueryService<JobProfileIndex> searchQueryService;
        private readonly IAsyncHelper asyncHelper;

        #endregion Private Fields

        #region Constructors

        public JobProfileDetailsController(
            IWebAppContext webAppContext,
            IJobProfileRepository jobProfileRepository,
            IApplicationLogger applicationLogger,
            ISitefinityPage sitefinityPage,
            IMapper mapper,
            IAsyncHelper asyncHelper,
            ISearchQueryService<JobProfileIndex> searchService)
            : base(webAppContext, jobProfileRepository, applicationLogger, sitefinityPage)
        {
            this.mapper = mapper;
            this.asyncHelper = asyncHelper;
            this.searchQueryService = searchService;
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
        public string SalaryContextLink { get; set; } = "#";

        [DisplayName("Text for salary context link")]
        public string SalaryContextText { get; set; } = "What does this mean?";

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
            return asyncHelper.Synchronise(() => GetJobProfileDetailsViewAsync());
        }

        protected override ActionResult GetEditorView()
        {
            // Sitefinity cannot handle async very well. So initialising it on current UI thread.
            var model = mapper.Map<JobProfileDetailsViewModel>(CurrentJobProfile);
            return asyncHelper.Synchronise(() => GetJobProfileDetailsViewAsync(model));
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
            model.SalaryContextLink = SalaryContextLink;
            model.SalaryContextText = SalaryContextText;
            model.HoursText = HoursText;
            model.MaxAndMinHoursAreBlankText = MaxAndMinHoursAreBlankText;
            model.HoursTimePeriodText = HoursTimePeriodText;
            model.WorkingPatternText = WorkingPatternText;
            model.WorkingPatternSpanText = WorkingPatternSpanText;

            if (model.IsLMISalaryFeedOverriden != true)
            {
                model = await PopulateSalaryAsync(model);
            }

            return View("Index", model);
        }

        private async Task<JobProfileDetailsViewModel> PopulateSalaryAsync(JobProfileDetailsViewModel model)
        {
            var properties = new SearchProperties
            {
                FilterBy = $"{nameof(JobProfileIndex.UrlName)} eq '{model.UrlName.Replace("'", "''")}'"
            };

            var jobProfileSearchResult = await searchQueryService.SearchAsync(model.Title, properties);

            var jobProfileIndexItem = jobProfileSearchResult.Results.FirstOrDefault()?.ResultItem;
            if (jobProfileIndexItem != null)
            {
                model.SalaryStarter = jobProfileIndexItem.SalaryStarter;
                model.SalaryExperienced = jobProfileIndexItem.SalaryExperienced;
            }

            return model;
        }

        #endregion Actions
    }
}