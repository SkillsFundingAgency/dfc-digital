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
        public CourseDetailsController(IApplicationLogger loggingService, ICourseSearchService courseSearchService, IAsyncHelper asyncHelper) : base(loggingService)
        {
            this.courseSearchService = courseSearchService;
            this.asyncHelper = asyncHelper;
        }
        #endregion

        #region Public Properties

        [DisplayName("Find a course Page")]
        public string FindAcoursePage { get; set; } = "/find-a-course/home/";

        [DisplayName("Find a course Page")]
        public string CourseDetailsPage { get; set; } = "/find-a-course/course-details";

        [DisplayName("Qualification Details Label")]
        public string QualificationDetailsLabel { get; set; } = "Qualification details";

        [DisplayName("Course Description Label")]
        public string CourseDescriptionLabel { get; set; } = "Course description";

        [DisplayName("No Course Description Message")]
        public string NoCourseDescriptionMessage { get; set; } = "No course description available";

        [DisplayName("Entry Requirements Label")]
        public string EntryRequirementsLabel { get; set; } = "Entry requirements";

        [DisplayName("No Entry Requirements Available Message")]
        public string NoEntryRequirementsAvailableMessage { get; set; } = "No entry requirements available message";

        [DisplayName("Equipment Required Label")]
        public string EquipmentRequiredLabel { get; set; } = "Equipment required";

        [DisplayName("No Equipment Required Message")]
        public string NoEquipmentRequiredMessage { get; set; } = "No equipment required";

        [DisplayName("Assessment Method Label")]
        public string AssessmentMethodLabel { get; set; } = "Assessment method";

        [DisplayName("No Assessment Method Available Message")]
        public string NoAssessmentMethodAvailableMessage { get; set; } = "Not available";

        [DisplayName("Venue Details Label")]
        public string VenueLabel { get; set; } = "Venue";

        [DisplayName("Other Dates And Venues Label")]
        public string OtherDatesAndVenuesLabel { get; set; } = "Other dates and venues";

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

        [DisplayName("Contact Adviser Section")]
        public string ContactAdviserSection { get; set; } = "<div class='app-related-items'><h3 class='govuk-heading-m'>Want to speak to an adviser?</h3><p class='govuk-body'><b>Call</b> 0800 100 900 or <a href='https://nationalcareers.service.gov.uk/webchat/chat/' target='_blank' class='govuk-link govuk-link--no-visited-state'>use webchat</a></p><p class='govuk-hint'>8am to 10pm, 7 days a week</p></div>";

        [DisplayName("Course Details - Attendance Pattern Label")]
        public string AttendancePatternLabel { get; set; } = "Attendance pattern";

        [DisplayName("Course Details - Price Label")]
        public string PriceLabel { get; set; } = "Price";

        [DisplayName("Course Details - Start Date Label")]
        public string StartDateLabel { get; set; } = "Course start date";

        [DisplayName("Course Details - Qualification Level Label")]
        public string QualificationLevelLabel { get; set; } = "Qualification level";

        [DisplayName("Course Details - Qualification Name Label")]
        public string QualificationNameLabel { get; set; } = "Qualification name";

        [DisplayName("Course Details - Awarding Organisation Label")]
        public string AwardingOrganisationLabel { get; set; } = "Awarding organisation";

        [DisplayName("Course Details - Subject Category Label")]
        public string SubjectCategoryLabel { get; set; } = "Subject category";

        [DisplayName("Course Details - Course Web Page Label")]
        public string CourseWebpageLinkLabel { get; set; } = "Course webpage";

        [DisplayName("Course Details - course type Label")]
        public string CourseTypeLabel { get; set; } = "Course type";

        [DisplayName("Course Details - Additional Price Label")]
        public string AdditionalPriceLabel { get; set; } = "Additional price description";

        [DisplayName("Course Details - funding Information Label")]
        public string FundingInformationLabel { get; set; } = "Funding information";

        [DisplayName("Course Details - Supporting Facilities Label")]
        public string SupportingFacilitiesLabel { get; set; } = "Support facilities";

        [DisplayName("Course Details - Language of instruction Label")]
        public string LanguageOfInstructionLabel { get; set; } = "Instruction language";

        [DisplayName("Course Details - Funding Information text")]
        public string FundingInformationText { get; set; } = "Advanced Learner Loans available";

        [DisplayName("Course Details - Funding Information Link")]
        public string FundingInformationLink { get; set; } = "https://www.gov.uk/advanced-learner-loan/overview";

        #endregion

        #region Actions
        public ActionResult Index(string courseId, string oppurtunity, string referralPath)
        {
            var viewModel = new CourseDetailsViewModel();
            if (!string.IsNullOrWhiteSpace(courseId))
            {
                var courseDetails = asyncHelper.Synchronise(() => courseSearchService.GetCourseDetailsAsync(courseId, oppurtunity));
                if (courseDetails != null)
                {
                    viewModel.FindACoursePage = FindAcoursePage;
                    viewModel.CourseDetails = courseDetails;
                    viewModel.ReferralPath = HttpUtility.HtmlDecode(referralPath);
                    viewModel.NoCourseDescriptionMessage = NoCourseDescriptionMessage;
                    viewModel.NoEntryRequirementsAvailableMessage = NoEntryRequirementsAvailableMessage;
                    viewModel.NoEquipmentRequiredMessage = NoEquipmentRequiredMessage;
                    viewModel.NoAssessmentMethodAvailableMessage = NoAssessmentMethodAvailableMessage;
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
                    viewModel.CourseDetailsPage = CourseDetailsPage;
                    viewModel.ContactAdviserSection = ContactAdviserSection;
                    viewModel.QualificationNameLabel = QualificationNameLabel;
                    viewModel.QualificationLevelLabel = QualificationLevelLabel;
                    viewModel.AwardingOrganisationLabel = AwardingOrganisationLabel;
                    viewModel.SubjectCategoryLabel = SubjectCategoryLabel;
                    viewModel.CourseWebpageLinkLabel = CourseWebpageLinkLabel;
                    viewModel.CourseTypeLabel = CourseTypeLabel;
                    viewModel.StartDateLabel = StartDateLabel;
                    viewModel.PriceLabel = PriceLabel;
                    viewModel.AdditionalPriceLabel = AdditionalPriceLabel;
                    viewModel.FundingInformationLabel = FundingInformationLabel;
                    viewModel.AttendancePatternLabel = AttendancePatternLabel;
                    viewModel.SupportingFacilitiesLabel = SupportingFacilitiesLabel;
                    viewModel.FundingInformationLink = FundingInformationLink;
                    viewModel.FundingInformationText = FundingInformationText;
                    viewModel.LanguageOfInstructionLabel = LanguageOfInstructionLabel;
                    return View(viewModel);
                }
            }

            return HttpNotFound();
        }

        #endregion
    }
}