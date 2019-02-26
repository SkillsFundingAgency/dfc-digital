using System.Web.Mvc;
using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Web.Core;
using DFC.Digital.Web.Sitefinity.Core;
using Telerik.Sitefinity.Mvc;

namespace DFC.Digital.Web.Sitefinity.CourseModule.Mvc.Controllers
{
    [ControllerToolboxItem(Name = "CourseDetails", Title = "Course Details", SectionName = SitefinityConstants.CustomCoursesSection)]
    public class CourseDetailsController : BaseDfcController
    {

        private readonly ICourseSearchService courseSearchService;
        private readonly IAsyncHelper asyncHelper;

        // GET: CourseDetails
        public CourseDetailsController(IApplicationLogger loggingService, ICourseSearchService courseSearchService, IAsyncHelper asyncHelper) : base(loggingService)
        {
            this.courseSearchService = courseSearchService;
            this.asyncHelper = asyncHelper;
        }

        public ActionResult Index(string courseId)
        {
            var viewModel = new CourseDetailsViewModel();
            if (!string.IsNullOrWhiteSpace(courseId))
            {
                viewModel.CourseDetails = asyncHelper.Synchronise(() => courseSearchService.GetCourseDetails(courseId));
            }
            
            return View(viewModel);
        }
    }
}