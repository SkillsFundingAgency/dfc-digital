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

        public string AttendanceModesSource { get; set; } = "Show All: 0, Classroom: 1, Work-bases: 2, Online/Distance learning : 3";

        public string StudyModesSource { get; set; } = "Show All: 0, Full Time: 1, Part Time: 2, Flexible : 3";

        public string AttendancePatternModesSource { get; set; } = "Show All: 0,  Normal working hours: 1, Day release/Block release: 2, Evening/Weekend : 3";

        public string QualificationLevelSource { get; set; } = "All: 0, Entry Level: 1, Level 1: 2, Level 2 : 3, Level 3 : 4, Level 4 : 5, Level 5 : 6, Level 6 : 7, Level 7 : 8, Level 8 : 9";

        public string AgeSuitabiltySource { get; set; } = "All Ages: 0,  16 to 19 year-olds: 1619, 19+ year-olds: 19plus";

        public string DistanceSource { get; set; } = "1 mile: 1,3 miles: 3, 5 miles: 5, 10 miles : 10, 15 miles : 15, 20 miles: 20, National : national";

        #endregion

        #region Actions

        [HttpGet]
        public ActionResult Index(string searchTerm, string attendance, string studymode, string qualificationLevel, string distance, string dfe1619Funded, string pattern, string location, int page = 1)
        {
            var viewModel = new TrainingCourseResultsViewModel {SearchTerm = searchTerm};

            if (!string.IsNullOrEmpty(searchTerm))
            {
                var request = courseSearchConverter.GetCourseSearchRequest(searchTerm, RecordsPerPage, attendance, studymode, qualificationLevel, distance, dfe1619Funded, pattern, location, page);

                var response = asyncHelper.Synchronise(() => courseSearchService.SearchCoursesAsync(request));
                viewModel.Courses = response.Courses;
                courseSearchConverter.SetupPaging(viewModel, response, searchTerm, RecordsPerPage, CourseSearchResultsPage);

                SetupFilterLists(attendance, studymode, qualificationLevel, pattern, distance, dfe1619Funded, location, viewModel);
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
            Index(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty).ExecuteResult(ControllerContext);
        }

        private void SetupFilterLists(string attendance, string studymode, string qualificationLevel, string pattern, string distance, string dfe1619Funded, string location,
            TrainingCourseResultsViewModel viewModel)
        {
            viewModel.CourseFiltersModel.Location = location;
            viewModel.CourseFiltersModel.AttendanceSelectedList =
                courseSearchConverter.GetFilterSelectItems(
                    $"{nameof(CourseFiltersModel)}.{nameof(CourseFiltersModel.AttendanceMode)}",
                    AttendanceModesSource.Split(','), attendance);

            viewModel.CourseFiltersModel.PatternSelectedList =
                courseSearchConverter.GetFilterSelectItems(
                    $"{nameof(CourseFiltersModel)}.{nameof(CourseFiltersModel.AttendancePattern)}",
                    AttendancePatternModesSource.Split(','), pattern);

            viewModel.CourseFiltersModel.QualificationSelectedList =
                courseSearchConverter.GetFilterSelectItems(
                    $"{nameof(CourseFiltersModel)}.{nameof(CourseFiltersModel.QualificationLevel)}",
                    QualificationLevelSource.Split(','), qualificationLevel);

            viewModel.CourseFiltersModel.StudyModeSelectedList =
                courseSearchConverter.GetFilterSelectItems(
                    $"{nameof(CourseFiltersModel)}.{nameof(CourseFiltersModel.StudyMode)}", StudyModesSource.Split(','),
                    studymode);

            viewModel.CourseFiltersModel.DistanceSelectedList =
                courseSearchConverter.GetFilterSelectItems(
                    $"{nameof(CourseFiltersModel)}.{nameof(CourseFiltersModel.Distance)}",
                    DistanceSource.Split(','), distance);

            viewModel.CourseFiltersModel.AgeSuitabilitySelectedList =
                courseSearchConverter.GetFilterSelectItems(
                    $"{nameof(CourseFiltersModel)}.{nameof(CourseFiltersModel.AgeSuitability)}", AgeSuitabiltySource.Split(','),
                    dfe1619Funded);
        }

        #endregion Actions  

    }
}