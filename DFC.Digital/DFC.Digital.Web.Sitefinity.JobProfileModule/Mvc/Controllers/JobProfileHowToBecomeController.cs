using AutoMapper;
using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Web.Sitefinity.Core;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models;
using System.Linq;
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
        public string MainSectionTitle { get; set; } = "How to become";

        public string SectionId { get; set; } = "HowToBecome";

        public string SubsectionUniversity { get; set; } = "University";

        public string SubsectionUniversityRequirements { get; set; } = "Requirements";

        public string SubsectionUniversityMoreInformation { get; set; } = "More information";

        public string SubsectionCollege { get; set; } = "College";

        public string SubsectionCollegeRequirements { get; set; } = "Requirements";

        public string SubsectionCollegeMoreInformation { get; set; } = "More information";

        public string SubsectionApprenticeship { get; set; } = "Apprenticeship";

        public string SubsectionApprenticeshipRequirements { get; set; } = "Requirements";

        public string SubsectionApprenticeshipMoreInformation { get; set; } = "More information";

        public string SubsectionWork { get; set; } = "Work";

        public string SubsectionVolunteering { get; set; } = "Volunteering";

        public string SubsectionDirectApplication { get; set; } = "Direct Application";

        public string SubsectionOtherRoutes { get; set; } = "Other Routes";

        public string SubsectionMoreInfo { get; set; } = "More information";

        public string SubsectionMoreInfoRegistration { get; set; } = "Registration";

        public string SubsectionMoreInfoRegistrationOpeningText { get; set; }

        public string SubsectionMoreInfoTips { get; set; } = "Career tips";

        public string SubsectionMoreInfoBodies { get; set; } = "Professional and industry bodies";

        public string SubsectionMoreInfoFurtherInfo { get; set; } = "Further information";

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
            return GetJobProfileDetailsView();
        }

        private ActionResult GetJobProfileDetailsView()
        {
           var model = LoadViewModel();

            return View("Index", model);
        }

        private JobProfileHowToBecomeViewModel LoadViewModel()
        {
            var model = mapper.Map<JobProfileHowToBecomeViewModel>(this);

            if (CurrentJobProfile != null)
            {
                model.HowToBecomeText = CurrentJobProfile.HowToBecome;
                model.HowToBecome = CurrentJobProfile.HowToBecomeData;
                model.DynamicTitle = $"{MainSectionTitle} {GetDynamicTitle(false)}".Trim();
            }
            else
            {
                model.DynamicTitle = $"{MainSectionTitle}".Trim();
            }

            return model;
        }

        #endregion Actions

    }
}