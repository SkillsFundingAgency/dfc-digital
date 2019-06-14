using DFC.Digital.Core;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Core;
using DFC.Digital.Web.Sitefinity.Core;
using System.ComponentModel;
using System.Web.Mvc;
using Telerik.Sitefinity.Mvc;

namespace DFC.Digital.Web.Sitefinity.CourseModule.Mvc.Controllers
{
    [ControllerToolboxItem(Name = "CourseLanding", Title = "Courses Landing", SectionName = SitefinityConstants.CustomCoursesSection)]
    public class CourseLandingController : BaseDfcController
    {
        #region private Fields
        private readonly IQueryStringBuilder<CourseSearchFilters> queryStringBuilder;
        #endregion

        #region Ctor
        public CourseLandingController(IApplicationLogger loggingService, IQueryStringBuilder<CourseSearchFilters> queryStringBuilder) : base(loggingService)
        {
            this.queryStringBuilder = queryStringBuilder;
        }
        #endregion

        #region Public Properties
        [DisplayName("Course Name Hint Text")]
        public string CourseNameHintText { get; set; } = "For example, Maths or Sports Studies";

        [DisplayName("Course Name Label")]
        public string CourseNameLabel { get; set; } = "Course name";

        [DisplayName("Provider Label")]
        public string ProviderLabel { get; set; } = "Provider name (optional)";

        [DisplayName("Provider Hint Text")]

        public string ProviderHintText { get; set; } = "For example, Sheffield College.";

        [DisplayName("Location Label")]
        public string LocationLabel { get; set; } = "Location (optional)";

        [DisplayName("Location Hint Text")]
        public string LocationHintText { get; set; } = "Enter a town or postcode. For example, Birmingham.";

        [DisplayName("Training Courses Results Page")]
        public string CourseSearchResultsPage { get; set; } = "/course-directory/course-search-result";

        [DisplayName("Location Post Code Regex")]
        public string LocationRegex { get; set; } = @"^([bB][fF][pP][oO]\s{0,1}[0-9]{1,4}|[gG][iI][rR]\s{0,1}0[aA][aA]|[a-pr-uwyzA-PR-UWYZ]([0-9]{1,2}|([a-hk-yA-HK-Y][0-9]|[a-hk-yA-HK-Y][0-9]([0-9]|[abehmnprv-yABEHMNPRV-Y]))|[0-9][a-hjkps-uwA-HJKPS-UW])\s{0,1}[0-9][abd-hjlnp-uw-zABD-HJLNP-UW-Z]{2})$";

        [DisplayName("Dfe 1619 Funded Text")]
        public string Dfe1619FundedText { get; set; } = "Only show courses suitable for 16-19 year olds";

        [DisplayName("Submit button Text")]
        public string SubmitText { get; set; } = "Find a course";
        #endregion

        #region Action
        [HttpGet]
        public ActionResult Index()
        {
            var model = new CourseLandingViewModel();
            AddWidgetProperties(model);
            return View("Index", model);
        }

        [HttpPost]
        public ActionResult Index(CourseSearchFilters model)
        {
            if (model == null)
            {
                return Redirect(CourseSearchResultsPage);
            }

            model.LocationRegex = LocationRegex;

            return Redirect(queryStringBuilder.BuildPathAndQueryString(CourseSearchResultsPage, model));
        }

        #endregion

        private void AddWidgetProperties(CourseLandingViewModel model)
        {
            model.CourseNameHintText = CourseNameHintText;
            model.CourseNameLabel = CourseNameLabel;
            model.LocationLabel = LocationLabel;
            model.ProviderLabel = ProviderLabel;
            model.ProviderNameHintText = ProviderHintText;
            model.LocationHintText = LocationHintText;
            model.Dfe1619FundedText = Dfe1619FundedText;
            model.SubmitText = SubmitText;
        }
    }
}