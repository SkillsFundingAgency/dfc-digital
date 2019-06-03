using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Web.Core;
using DFC.Digital.Web.Sitefinity.Core;
using System;
using System.ComponentModel;
using System.Web;
using System.Web.Mvc;
using Telerik.Sitefinity.Mvc;

namespace DFC.Digital.Web.Sitefinity.CourseModule.Mvc.Controllers
{
    [ControllerToolboxItem(Name = "CourseDetails", Title = "Course Details", SectionName = SitefinityConstants.CustomCoursesSection)]
    public class CourseDetailsController : BaseDfcController
    {
        #region Private Fields
        private readonly ICourseSearchService courseSearchService;
        private readonly IAsyncHelper asyncHelper;

        #endregion

        #region Ctor
        public CourseDetailsController(IWebAppContext webAppContext, IApplicationLogger loggingService, ICourseSearchService courseSearchService, IAsyncHelper asyncHelper) : base(loggingService)
        {
            this.courseSearchService = courseSearchService;
            this.asyncHelper = asyncHelper;
            this.WebAppContext = webAppContext;
        }
        #endregion

        #region Public Properties

        [DisplayName("Find a course Page")]
        public string FindAcoursePage { get; set; } = "/course-directory/home/";

        [DisplayName("No Course Description Message")]
        public string NoCourseDescriptionMessage { get; set; } = "No course description available";

        [DisplayName("No Entry Requirements Available Message")]
        public string NoEntryRequirementsAvailableMessage { get; set; } = "No entry requirements available message";

        [DisplayName("No Equipment Required Message")]
        public string NoEquipmentRequiredMessage { get; set; } = "No equipment required";

        [DisplayName("No Assessment Method Available Message")]
        public string NoAssessmentMethodAvailableMessage { get; set; } = "Not available";

        [DisplayName("No Venue Available Message")]
        public string NoVenueAvailableMessage { get; set; } = "No venue Available";

        [DisplayName("No Other Date Or Venue Available Message")]
        public string NoOtherDateOrVenueAvailableMessage { get; set; } = "No other date or venue available";

        protected IWebAppContext WebAppContext { get; set; }

        public ActionResult Index(string courseId)
        {
            var viewModel = new CourseDetailsViewModel
            {
                FindACoursePage = FindAcoursePage,
            };

            var referralUrl = HttpUtility.HtmlEncode(WebAppContext.RequestQueryString["referralUrl"]);
            if (!string.IsNullOrWhiteSpace(courseId))
            {
                viewModel.CourseDetails = asyncHelper.Synchronise(() => courseSearchService.GetCourseDetailsAsync(courseId, referralUrl));
                viewModel.CourseDetails.BackToResultsUrl = referralUrl;
                viewModel.CourseDetails.NoCourseDescriptionMessage = NoCourseDescriptionMessage;
                viewModel.CourseDetails.NoEntryRequirementsAvailableMessage = NoEntryRequirementsAvailableMessage;
                viewModel.CourseDetails.NoEquipmentRequiredMessage = NoEquipmentRequiredMessage;
                viewModel.CourseDetails.NoAssessmentMethodAvailableMessage = NoAssessmentMethodAvailableMessage;
                viewModel.CourseDetails.NoVenueAvailableMessage = NoVenueAvailableMessage;
                viewModel.CourseDetails.NoOtherDateOrVenueAvailableMessage = NoOtherDateOrVenueAvailableMessage;
            }

            return View(viewModel);
        }

        #endregion
    }
}