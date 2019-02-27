using System.ComponentModel;
using System.Web.Mvc;
using DFC.Digital.Core;
using DFC.Digital.Web.Core;
using DFC.Digital.Web.Sitefinity.Core;
using DFC.Digital.Web.Sitefinity.CourseModule.Mvc.Models;
using Telerik.Sitefinity.Mvc;

namespace DFC.Digital.Web.Sitefinity.CourseModule.Mvc.Controllers
{
    [ControllerToolboxItem(Name = "CourseLanding", Title = "Courses Landing", SectionName = SitefinityConstants.CustomCoursesSection)]
    public class CourseLandingController : BaseDfcController
    {
        private readonly ICourseSearchConverter courseSearchConverter;

        [DisplayName("Course Name Hint Text")]
        public string CourseNameHintText { get; set; } = "For example, Maths or Sports Studies";

        [DisplayName("Course Name Label")]
        public string CourseNameLabel { get; set; } = "Course name";

        [DisplayName("Provider Label")]
        public string ProviderLabel { get; set; } = "Provider Name";

        [DisplayName("Location Label")]
        public string LocationLabel { get; set; } = "Location (optional)";

        [DisplayName("Location Hint Text")]
        public string LocationHintText { get; set; } = "Enter a full postcode. For example, S1 1WB";

        [DisplayName("Qualification Level Hint")]
        public string QualificationLevelHint { get; set; } = "What qualification levels mean";

        [DisplayName("Qualification Level Label")]
        public string QualificationLevelLabel { get; set; } = "Qualification level (optional)";

        [DisplayName("Training Courses Results Page")]
        public string CourseSearchResultsPage { get; set; } = "/courses-search-results";

        [DisplayName("Location Post Code Regex")]
        public string LocationRegex { get; set; } = @"^[A-Za-z0-9-.\(\)\/\\\s]*$";

        [DisplayName("Dfe 1619 Funded Text")]
        public string Dfe1619FundedText { get; set; } = "Only show courses suitable for 16-19 year olds";



        // GET: CourseLanding
        public CourseLandingController(IApplicationLogger loggingService, ICourseSearchConverter courseSearchConverter) : base(loggingService)
        {
            this.courseSearchConverter = courseSearchConverter;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View(new CourseLandingViewModel
            {
                CourseNameHintText = CourseNameHintText,
                CourseNameLabel = CourseNameLabel,
                LocationLabel = LocationLabel,
                ProviderLabel = ProviderLabel,
                QualificationLevelHint = QualificationLevelHint,
                QualificationLevelLabel = QualificationLevelLabel,
                LocationHintText = LocationHintText
            });
        }

        [HttpPost]
        public ActionResult Index(CourseLandingViewModel model)
        {
            if (!string.IsNullOrWhiteSpace(model.SearchTerm))
            {
                return Redirect(courseSearchConverter.BuildSearchRedirectPathAndQueryString(CourseSearchResultsPage, model, LocationRegex));
            }

            return View(new CourseLandingViewModel
            {
                CourseNameHintText = CourseNameHintText,
                CourseNameLabel = CourseNameLabel,
                LocationLabel = LocationLabel,
                ProviderLabel = ProviderLabel,
                QualificationLevelHint = QualificationLevelHint,
                QualificationLevelLabel = QualificationLevelLabel,
                LocationHintText = LocationHintText
            });
        }
    }
}