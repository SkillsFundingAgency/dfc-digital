using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Core;
using DFC.Digital.Web.Sitefinity.Core;
using DFC.Digital.Web.Sitefinity.Widgets.Mvc.Models;
using System.Web.Mvc;
using Telerik.Sitefinity.Mvc;

namespace DFC.Digital.Web.Sitefinity.Widgets.Mvc.Controllers
{
    /// <summary>
    /// Custom Widget for customising page titles
    /// </summary>
    /// <seealso cref="DFC.Digital.Web.Core.BaseDfcController" />
    [ControllerToolboxItem(Name = "TrainingCourses", Title = "Training Courses", SectionName = SitefinityConstants.CustomWidgetSection)]
    public class TrainingCoursesController : BaseDfcController
    {
        #region private fields
        private readonly ICourseSearchService courseSearchService;
        private readonly IAsyncHelper asyncHelper;
        #endregion

        #region Constructors

        public TrainingCoursesController(IApplicationLogger applicationLogger, ICourseSearchService courseSearchService, IAsyncHelper asyncHelper) : base(applicationLogger)
        {
            this.courseSearchService = courseSearchService;
            this.asyncHelper = asyncHelper;
        }

        #endregion Constructors

        #region Public Properties

        public int PageCount { get; set; } = 20;
        #endregion

        #region Actions

        [HttpGet]
        public ActionResult Index(string searchTerm, int page = 1)
        {
            var viewModel = new TrainingCourseResultsViewModel();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                var request = new CourseSearchRequest
                {
                    SearchTerm = searchTerm,
                    PageCount = PageCount,
                    PageNumber = page
                };
                viewModel.Courses = asyncHelper.Synchronise(() => courseSearchService.SearchCoursesAsync(request));
            }

            return View("SearchResults", viewModel);
        }

        /// <inheritdoc />
        /// <summary>
        /// Called when a request matches this controller, but no method with the specified action name is found in the controller.
        /// </summary>
        /// <param name="actionName">The name of the attempted action.</param>
        protected override void HandleUnknownAction(string actionName)
        {
            Index(string.Empty).ExecuteResult(ControllerContext);
        }

        #endregion Actions

    }
}