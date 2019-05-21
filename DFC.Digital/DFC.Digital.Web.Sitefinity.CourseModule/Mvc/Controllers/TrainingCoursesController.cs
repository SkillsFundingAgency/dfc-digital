﻿using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Core;
using DFC.Digital.Web.Sitefinity.Core;
using DFC.Digital.Web.Sitefinity.CourseModule.Mvc.Models;
using System;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using Telerik.Sitefinity.Mvc;

namespace DFC.Digital.Web.Sitefinity.CourseModule.Mvc.Controllers
{
    [ControllerToolboxItem(Name = "TrainingCourses", Title = "Training Courses Results", SectionName = SitefinityConstants.CustomCoursesSection)]
    public class TrainingCoursesController : BaseDfcController
    {
        #region private fields
        private readonly ICourseSearchService courseSearchService;
        private readonly IAsyncHelper asyncHelper;
        private readonly ICourseSearchViewModelService courseSearchViewModelService;
        private readonly IBuildQueryStringService buildQueryStringService;

        #endregion

        #region Constructors

        public TrainingCoursesController(IApplicationLogger applicationLogger, ICourseSearchService courseSearchService, IAsyncHelper asyncHelper, ICourseSearchViewModelService courseSearchViewModelService, IBuildQueryStringService buildQueryStringService) : base(applicationLogger)
        {
            this.courseSearchService = courseSearchService;
            this.asyncHelper = asyncHelper;
            this.courseSearchViewModelService = courseSearchViewModelService;
            this.buildQueryStringService = buildQueryStringService;
        }

        #endregion Constructors

        #region Public Properties

        [DisplayName("Page Title")]
        public string PageTitle { get; set; } = "Find a course";

        [DisplayName("Filter Course By Text")]
        public string FilterCourseByText { get; set; } = "Filtering courses by:";

        [DisplayName("Records Per Page")]
        public int RecordsPerPage { get; set; } = 20;

        [DisplayName("Training Courses Results Page")]
        public string CourseSearchResultsPage { get; set; } = "/courses-search-results";

        [DisplayName("Course Details Page")]
        public string CourseDetailsPage { get; set; } = "/course-details";

        [DisplayName("No Training Courses Text")]
        public string NoTrainingCoursesFoundText { get; set; } = "No training courses found";

        [DisplayName("Order by Text")]
        public string OrderByText { get; set; } = "Ordered by";

        [DisplayName("Relevance Order By Text")]
        public string RelevanceOrderByText { get; set; } = "relevance";

        [DisplayName("StartDate Order By Text")]
        public string DistanceOrderByText { get; set; } = "distance";

        [DisplayName("StartDate Order By Text")]
        public string StartDateOrderByText { get; set; } = "start date";

        [DisplayName("Course Listing Location Label")]
        public string LocationLabel { get; set; } = "Location:";

        [DisplayName("Course Listing Provider Label")]
        public string ProviderLabel { get; set; } = "Provider:";

        [DisplayName("Course Listing Advanced Learner Provider Label")]
        public string AdvancedLoanProviderLabel { get; set; } = "Advanced Learner Loans offered by this Provider:";

        [DisplayName("Course Listing Start Date Label")]
        public string StartDateLabel { get; set; } = "Start date:";

        [DisplayName("Location Post Code Regex")]
        public string LocationRegex { get; set; } = @"([Gg][Ii][Rr] 0[Aa]{2})|((([A-Za-z][0-9]{1,2})|(([A-Za-z][A-Ha-hJ-Yj-y][0-9]{1,2})|(([A-Za-z][0-9][A-Za-z])|([A-Za-z][A-Ha-hJ-Yj-y][0-9][A-Za-z]?))))\s?[0-9][A-Za-z]{2})";

        //[DisplayName("Attendance Modes Source")]
        //public string AttendanceModesSource { get; set; } = "Show All: 0, Classroom: 1, Work-based: 2, Online/Distance learning : 3";

        //[DisplayName("Study Modes Source")]
        //public string StudyModesSource { get; set; } = "Show All: 0, Full Time: 1, Part Time: 2, Flexible : 3";

        //[DisplayName("Attendance Pattern Modes Source")]
        //public string AttendancePatternModesSource { get; set; } = "Show All: 0,  Normal working hours: 1, Day release/Block release: 2, Evening/Weekend : 3";

        //[DisplayName("Qualification Levels Source")]
        //public string QualificationLevelSource { get; set; } = "All: 0, Entry Level: 1, Level 1: 2, Level 2 : 3, Level 3 : 4, Level 4 : 5, Level 5 : 6, Level 6 : 7, Level 7 : 8, Level 8 : 9";

        //[DisplayName("Age Suitability Source")]
        //public string AgeSuitabilitySource { get; set; } = "All Ages: 0,  16 to 19 year-olds: 1619, 19+ year-olds: 19plus";

        //[DisplayName("Distance Source")]
        //public string DistanceSource { get; set; } = "1 mile: 1,3 miles: 3, 5 miles: 5, 10 miles : 10, 15 miles : 15, 20 miles: 20, National : national";

        //[DisplayName("Start Date Source")]
        //public string StartDateSource { get; set; } = "Anytime: 1,  From today: 2, Select date from: 3";
        #endregion

        #region Actions

        [HttpGet]
        public ActionResult Index(string searchTerm, string attendance, string studymode, string qualificationLevel, string distance, string pattern, string location, string sortBy, string startDate, string provider, bool dfe1619Funded = false, int page = 1)
        {
            var viewModel = new TrainingCourseResultsViewModel { SearchTerm = searchTerm };

            if (!string.IsNullOrEmpty(searchTerm))
            {
                var courseSearchProperties = new CourseSearchProperties
                {
                    PageNumber = page,
                    RecordsPerPage = RecordsPerPage,
                    CourseSearchSortBy = GetSortBy(sortBy)
                };

                float.TryParse(distance, out var localDistance);
                var searchFilters = new CourseSearchFilters
                {
                    Attendance = attendance,
                    StudyMode = studymode,
                    Only1619Courses = dfe1619Funded,
                    Distance = localDistance,
                    AttendancePattern = pattern,
                    Location = location,
                    ProviderKeyword = provider
                };

                var response = asyncHelper.Synchronise(() => courseSearchService.SearchCoursesAsync(searchTerm, courseSearchProperties, searchFilters));
                if (response.Courses.Any())
                {
                    foreach (var course in response.Courses)
                    {
                        course.CourseUrl = $"{CourseDetailsPage}?courseid={course.CourseId}";
                        viewModel.Courses.Add(new CourseListingViewModel
                        {
                            Course = course,
                            AdvancedLoanProviderLabel = AdvancedLoanProviderLabel,
                            LocationLabel = LocationLabel,
                            ProviderLabel = ProviderLabel,
                            StartDateLabel = StartDateLabel
                        });
                    }

                    var pathQuery = Request?.Url?.PathAndQuery;
                    if (!string.IsNullOrWhiteSpace(pathQuery) && pathQuery.ToLowerInvariant().IndexOf("&page=", StringComparison.InvariantCultureIgnoreCase) > 0)
                    {
                        pathQuery = pathQuery.Substring(0, pathQuery.ToLowerInvariant().IndexOf("&page=", StringComparison.InvariantCultureIgnoreCase));
                    }

                    courseSearchViewModelService.SetupPaging(viewModel, response, pathQuery, RecordsPerPage, CourseSearchResultsPage);
                    SetupSearchLinks(searchTerm, viewModel, pathQuery, response.CourseSearchSortBy);
                }

               // SetupFilterDisplayData(attendance, studymode, qualificationLevel, distance, dfe1619Funded, pattern, location, startDate, provider, viewModel);
            }

            SetupWidgetDefaults(viewModel);
            return View("SearchResults", viewModel);
        }

        [HttpPost]
        public ActionResult Index(TrainingCourseResultsViewModel viewModel)
        {
            if (!string.IsNullOrWhiteSpace(viewModel.SearchTerm))
            {
                return Redirect(buildQueryStringService.BuildRedirectPathAndQueryString(CourseSearchResultsPage, viewModel, LocationRegex));
            }

            SetupWidgetDefaults(viewModel);

            return View("SearchResults",  viewModel);
        }

        /// <inheritdoc />
        /// <summary>
        /// Called when a request matches this controller, but no method with the specified action name is found in the controller.
        /// </summary>
        /// <param name="actionName">The name of the attempted action.</param>
        protected override void HandleUnknownAction(string actionName)
        {
            Index(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty).ExecuteResult(ControllerContext);
        }

        //private void SetupFilterLists(string attendance, string studyMode, string qualificationLevel, string pattern, string distance, string dfe1619Funded, string location, string startDate, string providerKeyword, TrainingCourseResultsViewModel viewModel)
        //{
        //    viewModel.CourseFiltersModel.Location = location;
        //    viewModel.CourseFiltersModel.ProviderKeyword = providerKeyword;
        //    viewModel.CourseFiltersModel.AttendanceSelectedList =
        //        courseSearchConverter.GetFilterSelectItems(
        //            $"{nameof(CourseFiltersModel)}.{nameof(CourseFiltersModel.AttendanceMode)}", AttendanceModesSource.Split(','), attendance);

        //    viewModel.CourseFiltersModel.PatternSelectedList =
        //        courseSearchConverter.GetFilterSelectItems(
        //            $"{nameof(CourseFiltersModel)}.{nameof(CourseFiltersModel.AttendancePattern)}", AttendancePatternModesSource.Split(','), pattern);

        //    viewModel.CourseFiltersModel.QualificationSelectedList =
        //        courseSearchConverter.GetFilterSelectItems(
        //            $"{nameof(CourseFiltersModel)}.{nameof(CourseFiltersModel.QualificationLevel)}", QualificationLevelSource.Split(','), qualificationLevel);

        //    viewModel.CourseFiltersModel.StudyModeSelectedList =
        //        courseSearchConverter.GetFilterSelectItems(
        //            $"{nameof(CourseFiltersModel)}.{nameof(CourseFiltersModel.StudyMode)}", StudyModesSource.Split(','), studyMode);

        //    viewModel.CourseFiltersModel.DistanceSelectedList =
        //        courseSearchConverter.GetFilterSelectItems(
        //            $"{nameof(CourseFiltersModel)}.{nameof(CourseFiltersModel.Distance)}", DistanceSource.Split(','), distance);

        //    viewModel.CourseFiltersModel.AgeSuitabilitySelectedList =
        //        courseSearchConverter.GetFilterSelectItems(
        //            $"{nameof(CourseFiltersModel)}.{nameof(CourseFiltersModel.AgeSuitability)}", AgeSuitabilitySource.Split(','), dfe1619Funded);

        //    viewModel.CourseFiltersModel.StartDateSelectedList =
        //        courseSearchConverter.GetFilterSelectItems(
        //            $"{nameof(CourseFiltersModel)}.{nameof(CourseFiltersModel.StartDate)}", StartDateSource.Split(','), startDate);
        //}
        private void SetupSearchLinks(string searchTerm, TrainingCourseResultsViewModel viewModel, string pathQuery, CourseSearchOrderBy sortBy)
        {
            viewModel.OrderByLinks = courseSearchViewModelService.GetOrderByLinks(pathQuery, sortBy);
            viewModel.ResetFilterUrl = new Uri($"{CourseSearchResultsPage}?searchterm={searchTerm}", UriKind.RelativeOrAbsolute);
        }

        private void SetupWidgetDefaults(TrainingCourseResultsViewModel viewModel)
        {
            viewModel.PageTitle = PageTitle;
            viewModel.FilterCourseByText = FilterCourseByText;
            viewModel.NoTrainingCoursesFoundText = NoTrainingCoursesFoundText;
            viewModel.OrderByLinks.OrderByText = OrderByText;
            viewModel.OrderByLinks.DistanceOrderByText = DistanceOrderByText;
            viewModel.OrderByLinks.StartDateOrderByText = StartDateOrderByText;
            viewModel.OrderByLinks.RelevanceOrderByText = RelevanceOrderByText;
        }

        //private void SetupFilterDisplayData(string attendance, string studyMode, string qualificationLevel, string distance, string dfe1619Funded, string pattern, string location, string startDate, string provider, TrainingCourseResultsViewModel viewModel)
        //{
        //    SetupFilterLists(attendance, studyMode, qualificationLevel, pattern, distance, dfe1619Funded, location, startDate, provider, viewModel);

        //    viewModel.ActiveFilterOptions =
        //        courseSearchConverter.GetActiveFilterOptions(viewModel.CourseFiltersModel);
        //}
        #endregion Actions

        private CourseSearchOrderBy GetSortBy(string sortBy)
        {
            if (string.IsNullOrWhiteSpace(sortBy))
            {
                return CourseSearchOrderBy.Relevance;
            }

            switch (sortBy.ToLower())
            {
                case "distance":
                    return CourseSearchOrderBy.Distance;
                case "startdate":
                    return CourseSearchOrderBy.StartDate;
                default:
                    return CourseSearchOrderBy.Relevance;
            }
        }
    }
}