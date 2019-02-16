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
        #endregion

        #region Constructors

        public TrainingCoursesController(IApplicationLogger applicationLogger, ICourseSearchService courseSearchService, IAsyncHelper asyncHelper) : base(applicationLogger)
        {
            this.courseSearchService = courseSearchService;
            this.asyncHelper = asyncHelper;
        }

        #endregion Constructors

        #region Public Properties

        public int RecordsPerPage { get; set; } = 20;

        public string CourseSearchResultsPage { get; set; } = "/courses-search-results";
        #endregion

        #region Actions

        [HttpGet]
        public ActionResult Index(string searchTerm, int page = 1)
        {
            var viewModel = new TrainingCourseResultsViewModel { SearchTerm = searchTerm };

            if (!string.IsNullOrEmpty(searchTerm))
            {
                var request = new CourseSearchRequest
                {
                    SearchTerm = searchTerm,
                    RecordsPerPage = RecordsPerPage,
                    PageNumber = page
                };

                var response = asyncHelper.Synchronise(() => courseSearchService.SearchCoursesAsync(request));
                viewModel.Courses = response.Courses;
                SetupPaging(viewModel, response, searchTerm);
            }

            return View("SearchResults", viewModel);
        }

        [HttpPost]
        public ActionResult Index(TrainingCourseResultsViewModel viewModel)
        {
            if (!string.IsNullOrWhiteSpace(viewModel.SearchTerm))
            {
                return Redirect($"{CourseSearchResultsPage}?searchTerm={GetUrlEncodedString(viewModel.SearchTerm)}");
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
            Index(string.Empty).ExecuteResult(ControllerContext);
        }

        private static string GetUrlEncodedString(string input)
        {
            return !string.IsNullOrWhiteSpace(input) ? HttpUtility.UrlEncode(input) : string.Empty;
        }

        private void SetupPaging(TrainingCourseResultsViewModel viewModel, CourseSearchResponse response, string searchTerm)
        {
            viewModel.RecordsPerPage = RecordsPerPage;
            viewModel.CurrentPageNumber = response.CurrentPage;
            viewModel.TotalPagesCount = response.TotalPages;
            viewModel.ResultsCount = response.TotalResultCount;

            if (viewModel.TotalPagesCount > 1 && viewModel.TotalPagesCount >= viewModel.CurrentPageNumber)
            {
                viewModel.PaginationViewModel.HasPreviousPage = viewModel.CurrentPageNumber > 1;
                viewModel.PaginationViewModel.HasNextPage = viewModel.CurrentPageNumber < viewModel.TotalPagesCount;
                viewModel.PaginationViewModel.NextPageUrl = new Uri($"{CourseSearchResultsPage}?searchTerm={HttpUtility.UrlEncode(searchTerm)}&page={viewModel.CurrentPageNumber + 1}", UriKind.RelativeOrAbsolute);
                viewModel.PaginationViewModel.NextPageUrlText = $"{viewModel.CurrentPageNumber + 1} of {viewModel.TotalPagesCount}";

                if (viewModel.CurrentPageNumber > 1)
                {
                    viewModel.PaginationViewModel.PreviousPageUrl = new Uri($"{CourseSearchResultsPage}?searchTerm={HttpUtility.UrlEncode(searchTerm)}{(viewModel.CurrentPageNumber == 2 ? string.Empty : $"&page={viewModel.CurrentPageNumber - 1}")}", UriKind.RelativeOrAbsolute);
                    viewModel.PaginationViewModel.PreviousPageUrlText = $"{viewModel.CurrentPageNumber - 1} of {viewModel.TotalPagesCount}";
                }
            }
        }

        #endregion Actions

    }
}