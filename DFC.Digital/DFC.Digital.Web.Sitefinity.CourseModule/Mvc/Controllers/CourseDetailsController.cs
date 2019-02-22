using System.Web.Mvc;
using DFC.Digital.Core;
using DFC.Digital.Web.Core;
using DFC.Digital.Web.Sitefinity.Core;
using Telerik.Sitefinity.Mvc;

namespace DFC.Digital.Web.Sitefinity.CourseModule.Mvc.Controllers
{
    [ControllerToolboxItem(Name = "CourseDetails", Title = "Course Details", SectionName = SitefinityConstants.CustomCoursesSection)]
    public class CourseDetailsController : BaseDfcController
    {
        // GET: CourseDetails
        public CourseDetailsController(IApplicationLogger loggingService) : base(loggingService)
        {
        }

        public ActionResult Index()
        {
            return View( new CourseDetailsViewModel());
        }
    }
}