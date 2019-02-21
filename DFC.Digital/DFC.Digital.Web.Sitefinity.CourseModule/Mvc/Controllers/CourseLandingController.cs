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
        public string CourseNameHintText { get; set; } = "For example, Maths or Sports Studies";
        public string CourseNameLabel { get; set; } = "Course name";
        public string ProviderLabel { get; set; } = "Provider Name";
        public string LocationLabel { get; set; } = "Location (optional)";
        public string LocationHintText { get; set; } = "Enter a full postcode. For example, S1 1WB";

        public string QualificationLevelHint { get; set; } = "What qualification levels mean";

        public string QualificationLevelLabel { get; set; } = "Qualification level (optional)";


        // GET: CourseLanding
        public CourseLandingController(IApplicationLogger loggingService) : base(loggingService)
        {
        }

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
    }
}