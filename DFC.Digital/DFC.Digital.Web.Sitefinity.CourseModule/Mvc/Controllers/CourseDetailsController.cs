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
        private readonly IBuildQueryStringService buildQueryStringService;

        #endregion

        #region Ctor
        public CourseDetailsController(IWebAppContext webAppContext, IApplicationLogger loggingService, ICourseSearchService courseSearchService, IBuildQueryStringService buildQueryStringService, IAsyncHelper asyncHelper) : base(loggingService)
        {
            this.courseSearchService = courseSearchService;
            this.asyncHelper = asyncHelper;
            this.WebAppContext = webAppContext;
            this.buildQueryStringService = buildQueryStringService;
        }
        #endregion

        #region Public Properties

        [DisplayName("Find a course Page")]
        public string FindAcoursePage { get; set; } = "/course-directory/home/";

        [DisplayName("Find a course Page")]
        public string CourseDetailsUrlPath { get; set; } = "/course-directory/course-details";

        [DisplayName("Qualification Details Label")]
        public string QualificationDetailsLabel { get; set; } = "1. Qualification details";

        [DisplayName("Course Description Label")]
        public string CourseDescriptionLabel { get; set; } = "2. Course description";

        [DisplayName("No Course Description Message")]
        public string NoCourseDescriptionMessage { get; set; } = "No course description available";

        [DisplayName("Entry Requirements Label")]
        public string EntryRequirementsLabel { get; set; } = "3. Entry requirements";

        [DisplayName("No Entry Requirements Available Message")]
        public string NoEntryRequirementsAvailableMessage { get; set; } = "No entry requirements available message";

        [DisplayName("Equipment Required Label")]
        public string EquipmentRequiredLabel { get; set; } = "4. Equipment required";

        [DisplayName("No Equipment Required Message")]
        public string NoEquipmentRequiredMessage { get; set; } = "No equipment required";

        [DisplayName("Assessment Method Label")]
        public string AssessmentMethodLabel { get; set; } = "5. Assessment method";

        [DisplayName("No Assessment Method Available Message")]
        public string NoAssessmentMethodAvailableMessage { get; set; } = "Not available";

        [DisplayName("Venue Details Label")]
        public string VenueLabel { get; set; } = "6. Venue";

        [DisplayName("No Venue Available Message")]
        public string NoVenueAvailableMessage { get; set; } = "No venue Available";

        [DisplayName("Other Dates And Venues Label")]
        public string OtherDatesAndVenuesLabel { get; set; } = "7. Other dates and venues";

        [DisplayName("No Other Date Or Venue Available Message")]
        public string NoOtherDateOrVenueAvailableMessage { get; set; } = "No other date or venue available";

        [DisplayName("Provider Details Label")]
        public string ProviderLabel { get; set; } = "Provider details";

        [DisplayName("Employer Satisfaction Label")]
        public string EmployerSatisfactionLabel { get; set; } = "out of 10 for employer satisfaction";

        [DisplayName("Learner Satisfaction Label")]
        public string LearnerSatisfactionLabel { get; set; } = "out of 10 for learner satisfaction";

        [DisplayName("Provider Performance Label")]
        public string ProviderPerformanceLabel { get; set; } = "Provider performance information";

        protected IWebAppContext WebAppContext { get; set; }

        public ActionResult Index(string courseId, string oppurtunityId, string referralUrl)
        {
            var viewModel = new CourseDetailsViewModel
            {
                FindACoursePage = FindAcoursePage,
            };

            if (!string.IsNullOrWhiteSpace(courseId))
            {
                viewModel.CourseDetails = asyncHelper.Synchronise(() => courseSearchService.GetCourseDetailsAsync(courseId, oppurtunityId));
                viewModel.ReferralUrl = referralUrl;
                viewModel.NoCourseDescriptionMessage = NoCourseDescriptionMessage;
                viewModel.NoEntryRequirementsAvailableMessage = NoEntryRequirementsAvailableMessage;
                viewModel.NoEquipmentRequiredMessage = NoEquipmentRequiredMessage;
                viewModel.NoAssessmentMethodAvailableMessage = NoAssessmentMethodAvailableMessage;
                viewModel.NoVenueAvailableMessage = NoVenueAvailableMessage;
                viewModel.NoOtherDateOrVenueAvailableMessage = NoOtherDateOrVenueAvailableMessage;

                viewModel.QualificationDetailsLabel = QualificationDetailsLabel;
                viewModel.CourseDescriptionLabel = CourseDescriptionLabel;
                viewModel.EntryRequirementsLabel = EntryRequirementsLabel;
                viewModel.EquipmentRequiredLabel = EquipmentRequiredLabel;
                viewModel.AssessmentMethodLabel = AssessmentMethodLabel;
                viewModel.VenueLabel = VenueLabel;
                viewModel.OtherDatesAndVenuesLabel = OtherDatesAndVenuesLabel;
                viewModel.ProviderLabel = ProviderLabel;
                viewModel.EmployerSatisfactionLabel = EmployerSatisfactionLabel;
                viewModel.LearnerSatisfactionLabel = LearnerSatisfactionLabel;
                viewModel.ProviderPerformanceLabel = ProviderPerformanceLabel;
                viewModel.CourseDetailsUrlPath = CourseDetailsUrlPath;
            }

            return View(viewModel);
        }

        #endregion
    }
}