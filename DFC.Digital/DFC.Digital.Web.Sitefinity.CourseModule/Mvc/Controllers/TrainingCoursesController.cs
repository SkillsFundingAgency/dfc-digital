using System;
using System.Web;
using System.Web.Mvc;
using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Core;
using DFC.Digital.Web.Sitefinity.Core;
using Telerik.Sitefinity.Mvc;

namespace DFC.Digital.Web.Sitefinity.CourseModule.Mvc.Controllers
{
    /// <summary>
    /// Custom Widget for customising page titles
    /// </summary>
    /// <seealso cref="DFC.Digital.Web.Core.BaseDfcController" />
    [ControllerToolboxItem(Name = "TrainingCourses", Title = "Training Courses Results", SectionName = SitefinityConstants.CustomCoursesSection)]
    public class TrainingCoursesController : BaseDfcController
    {
        #region private fields
        private readonly ICourseSearchService courseSearchService;
        private readonly IAsyncHelper asyncHelper;
        private readonly ICourseSearchConverter courseSearchConverter;
        #endregion

        #region Constructors

        public TrainingCoursesController(IApplicationLogger applicationLogger, ICourseSearchService courseSearchService, IAsyncHelper asyncHelper, ICourseSearchConverter courseSearchConverter) : base(applicationLogger)
        {
            this.courseSearchService = courseSearchService;
            this.asyncHelper = asyncHelper;
            this.courseSearchConverter = courseSearchConverter;
        }

        #endregion Constructors

        #region Public Properties

        public int RecordsPerPage { get; set; } = 20;

        public string CourseSearchResultsPage { get; set; } = "/courses-search-results";

        public string LocationRegex { get; set; } = @"^[A-Za-z0-9-.\(\)\/\\\s]*$";
        #endregion

        #region Actions

        [HttpGet]
        public ActionResult Index(string searchTerm, string attendance, string studymode, string qualificationLevel, string distance, string dfe1619Funded, string pattern, int page = 1)
        {
            var viewModel = new TrainingCourseResultsViewModel {SearchTerm = searchTerm};

            if (!string.IsNullOrEmpty(searchTerm))
            {
                var request = courseSearchConverter.GetCourseSearchRequest(searchTerm, RecordsPerPage, attendance, studymode, qualificationLevel, distance, dfe1619Funded, pattern, page);

                var response = asyncHelper.Synchronise(() => courseSearchService.SearchCoursesAsync(request));
                viewModel.Courses = response.Courses;
                courseSearchConverter.SetupPaging(viewModel, response, searchTerm, RecordsPerPage, CourseSearchResultsPage);
            }

            return View("SearchResults", viewModel);
        }

        [HttpPost]
        public ActionResult Index(TrainingCourseResultsViewModel viewModel)
        {
            if (!string.IsNullOrWhiteSpace(viewModel.SearchTerm))
            {
                return Redirect(courseSearchConverter.BuildRedirectPathAndQueryString(CourseSearchResultsPage, viewModel, LocationRegex));
            }

            return View("SearchResults",  new TrainingCourseResultsViewModel { SearchTerm = viewModel.SearchTerm });
        }

        /// <inheritdoc />
        /// <summary>
        /// Called when a request matches this controller, but no method with the specified action name is found in the controller.
        /// </summary>
        /// <param name="actionName">The name of the attempted action.</param>
        protected override void HandleUnknownAction(string actionName)
        {
            Index(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty).ExecuteResult(ControllerContext);
        }

        #endregion Actions  

    }
}