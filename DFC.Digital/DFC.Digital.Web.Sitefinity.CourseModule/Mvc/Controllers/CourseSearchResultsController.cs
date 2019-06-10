using AutoMapper;
using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Core;
using DFC.Digital.Web.Sitefinity.Core;
using System;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Telerik.Sitefinity.Mvc;

namespace DFC.Digital.Web.Sitefinity.CourseModule.Mvc.Controllers
{
    [ControllerToolboxItem(Name = "CourseSearchResults", Title = "Course Search Results", SectionName = SitefinityConstants.CustomCoursesSection)]
    public class CourseSearchResultsController : BaseDfcController
    {
        #region private fields
        private readonly ICourseSearchService courseSearchService;
        private readonly IAsyncHelper asyncHelper;
        private readonly ICourseSearchViewModelService courseSearchViewModelService;
        private readonly IQueryStringBuilder<CourseSearchFilters> queryStringBuilder;
        private readonly IMapper mapper;

        #endregion

        #region Constructors

        public CourseSearchResultsController(
            IApplicationLogger applicationLogger,
            ICourseSearchService courseSearchService,
            IAsyncHelper asyncHelper,
            ICourseSearchViewModelService courseSearchViewModelService,
            IQueryStringBuilder<CourseSearchFilters> queryStringBuilder,
            IMapper mapper)
            : base(applicationLogger)
        {
            this.courseSearchService = courseSearchService;
            this.asyncHelper = asyncHelper;
            this.courseSearchViewModelService = courseSearchViewModelService;
            this.queryStringBuilder = queryStringBuilder;
            this.mapper = mapper;
        }

        #endregion Constructors

        #region Public Properties

        [DisplayName("* Page Title")]
        public string PageTitle { get; set; } = "Find a course";

        [DisplayName("Filter Course By Text")]
        public string FilterCourseByText { get; set; } = "Filtering courses by:";

        [DisplayName("* Records Per Page")]
        public int RecordsPerPage { get; set; } = 20;

        [DisplayName("Redirect - Course Search Results Page")]
        public string CourseSearchResultsPage { get; set; } = "/courses-search-results";

        [DisplayName("Redirect - Course Details Page")]
        public string CourseDetailsPage { get; set; } = "/course-details";

        [DisplayName("* No Training Courses Text")]
        public string NoTrainingCoursesFoundText { get; set; } = "No training courses found";

        [DisplayName("* Search for course name Text")]
        public string SearchForCourseNameText { get; set; } = "Course name";

        [DisplayName("Order by Text")]
        public string OrderByText { get; set; } = "Ordered by";

        [DisplayName("Order By - Relevance Text")]
        public string RelevanceOrderByText { get; set; } = "Relevance";

        [DisplayName("Order By - StartDate Text")]
        public string DistanceOrderByText { get; set; } = "Distance";

        [DisplayName("Order By - StartDate Text")]
        public string StartDateOrderByText { get; set; } = "Start date";

        [DisplayName("Course Listing Location Label")]
        public string LocationLabel { get; set; } = "Location:";

        [DisplayName("Course Listing Provider Label")]
        public string ProviderLabel { get; set; } = "Provider:";

        [DisplayName("Course Listing Advanced Learner Provider Label")]
        public string AdvancedLoanProviderLabel { get; set; } = "Advanced Learner Loans offered by this Provider:";

        [DisplayName("Course Listing Start Date Label")]
        public string StartDateLabel { get; set; } = "Start date:";

        [DisplayName("Regex - Location Post Code")]
        public string LocationRegex { get; set; } = @"^([bB][fF][pP][oO]\s{0,1}[0-9]{1,4}|[gG][iI][rR]\s{0,1}0[aA][aA]|[a-pr-uwyzA-PR-UWYZ]([0-9]{1,2}|([a-hk-yA-HK-Y][0-9]|[a-hk-yA-HK-Y][0-9]([0-9]|[abehmnprv-yABEHMNPRV-Y]))|[0-9][a-hjkps-uwA-HJKPS-UW])\s{0,1}[0-9][abd-hjlnp-uw-zABD-HJLNP-UW-Z]{2})$";

        [DisplayName("Regex - Allowed Characters")]
        public string InvalidCharactersRegexPattern { get; set; } = "(?:[^a-z0-9 ]|(?<=['\"])s)";

        [DisplayName("Filter - Only 1619 Courses Text")]
        public string Only1619CoursesText { get; set; } = "Suitable for 16-19 year olds";

        [DisplayName("Filter - Start Date exemplar text")]
        public string StartDateExampleText { get; set; } = "For example, 01 01 2020";

        [DisplayName("Filter - Course Hours section text")]
        public string CourseHoursSectionText { get; set; } = "Course Hours";

        [DisplayName("Filter - Start Date section text")]
        public string StartDateSectionText { get; set; } = "Start date";

        [DisplayName("Filter - Course Type section text")]
        public string CourseTypeSectionText { get; set; } = "Course Type";

        [DisplayName("Filter -Apply Filter button text")]
        public string ApplyFiltersText { get; set; } = "Apply Filters";

        [DisplayName("Filter - Distance within text")]
        public string WithinText { get; set; } = "Within";

        #endregion

        #region Actions

        [HttpGet]
        public ActionResult Index(CourseSearchFilters courseSearchFilters, CourseSearchProperties courseSearchProperties)
        {
            var viewModel = new CourseSearchResultsViewModel
            {
                CourseFiltersModel = mapper.Map<CourseFiltersViewModel>(courseSearchFilters)
            };

            var cleanCourseName =
                StringManipulationExtension.ReplaceSpecialCharacters(
                    courseSearchFilters.SearchTerm,
                    InvalidCharactersRegexPattern);

            if (!string.IsNullOrEmpty(cleanCourseName))
            {
                var courseSearchProps = courseSearchProperties ?? new CourseSearchProperties();
                courseSearchProps.Count = RecordsPerPage;

                courseSearchFilters.SearchTerm = cleanCourseName;

                ReplaceSpecialCharacters(courseSearchFilters);

                GetDistanceSpecified(courseSearchFilters, viewModel);

                courseSearchProps.Filters = courseSearchFilters;

                var response = asyncHelper.Synchronise(() =>
                    courseSearchService.SearchCoursesAsync(cleanCourseName, courseSearchProps));

                if (response.Courses.Any())
                {
                    foreach (var course in response.Courses)
                    {
                        course.CourseUrl = $"{CourseDetailsPage}?{nameof(CourseDetails.CourseId)}={course.CourseId}";
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
                    if (!string.IsNullOrWhiteSpace(pathQuery) &&
                        pathQuery.IndexOf($"&{nameof(CourseSearchResultProperties.Page)}=", StringComparison.InvariantCultureIgnoreCase) > 0)
                    {
                        pathQuery = pathQuery.Substring(
                            0,
                            pathQuery.IndexOf($"&{nameof(CourseSearchResultProperties.Page)}=", StringComparison.InvariantCultureIgnoreCase));
                    }

                    courseSearchViewModelService.SetupViewModelPaging(
                        viewModel,
                        response,
                        pathQuery,
                        RecordsPerPage);

                    SetupSearchLinks(viewModel, pathQuery, response.ResultProperties.OrderedBy);
                }

                SetupStartDateDisplayData(viewModel);
            }

            viewModel.NoTrainingCoursesFoundText =
                string.IsNullOrWhiteSpace(viewModel.CourseFiltersModel.SearchTerm)
                    ? string.Empty
                    : NoTrainingCoursesFoundText;

            SetupWidgetLabelsAndTextDefaults(viewModel);
            return View("SearchResults", viewModel);
        }

        [HttpPost]
        public ActionResult Index(CourseFiltersViewModel viewModel)
        {
            PopulateSelectFromDate(viewModel);

            return Redirect(queryStringBuilder.BuildPathAndQueryString(CourseSearchResultsPage, viewModel));
        }

        /// <inheritdoc />
        /// <summary>
        /// Called when a request matches this controller, but no method with the specified action name is found in the controller.
        /// </summary>
        /// <param name="actionName">The name of the attempted action.</param>
        protected override void HandleUnknownAction(string actionName)
        {
            Index(new CourseSearchFilters(), new CourseSearchProperties()).ExecuteResult(ControllerContext);
        }

        #endregion Actions

        #region private Methods

        private static void PopulateSelectFromDate(CourseFiltersViewModel viewModel)
        {
            viewModel.Distance = viewModel.IsDistanceLocation ? viewModel.Distance : default(float);
            if (viewModel.StartDate == StartDate.SelectDateFrom && DateTime.TryParse(
                    $"{viewModel.StartDateYear}-{viewModel.StartDateMonth}-{viewModel.StartDateDay}", out var chosenDate))
            {
                viewModel.StartDateFrom = chosenDate;
            }
        }

        private void SetupSearchLinks(CourseSearchResultsViewModel viewModel, string pathQuery, CourseSearchOrderBy sortBy)
        {
            viewModel.OrderByLinks = courseSearchViewModelService.GetOrderByLinks(pathQuery, sortBy);
            viewModel.ResetFilterUrl = new Uri($"{CourseSearchResultsPage}?{nameof(CourseSearchFilters.SearchTerm)}={viewModel.CourseFiltersModel.SearchTerm}", UriKind.RelativeOrAbsolute);
        }

        private void SetupWidgetLabelsAndTextDefaults(CourseSearchResultsViewModel viewModel)
        {
            viewModel.PageTitle = PageTitle;
            viewModel.FilterCourseByText = FilterCourseByText;
            viewModel.OrderByLinks.OrderByText = OrderByText;
            viewModel.OrderByLinks.DistanceOrderByText = DistanceOrderByText;
            viewModel.OrderByLinks.StartDateOrderByText = StartDateOrderByText;
            viewModel.OrderByLinks.RelevanceOrderByText = RelevanceOrderByText;
            viewModel.CourseFiltersModel.WithinText = WithinText;
            viewModel.CourseFiltersModel.Only1619CoursesText = Only1619CoursesText;
            viewModel.CourseFiltersModel.StartDateExampleText = StartDateExampleText;
            viewModel.CourseFiltersModel.StartDateSectionText = StartDateSectionText;
            viewModel.CourseFiltersModel.CourseHoursSectionText = CourseHoursSectionText;
            viewModel.CourseFiltersModel.CourseTypeSectionText = CourseTypeSectionText;
            viewModel.CourseFiltersModel.ApplyFiltersText = ApplyFiltersText;
            viewModel.SearchForCourseNameText = SearchForCourseNameText;
            viewModel.CourseFiltersModel.LocationRegex = LocationRegex;
        }

        private void SetupStartDateDisplayData(CourseSearchResultsViewModel viewModel)
        {
            if (!viewModel.CourseFiltersModel.StartDateFrom.Equals(DateTime.MinValue))
            {
                viewModel.CourseFiltersModel.StartDateDay = viewModel.CourseFiltersModel.StartDateFrom.Day.ToString();
                viewModel.CourseFiltersModel.StartDateMonth = viewModel.CourseFiltersModel.StartDateFrom.Month.ToString();
                viewModel.CourseFiltersModel.StartDateYear = viewModel.CourseFiltersModel.StartDateFrom.Year.ToString();
            }
            else
            {
                viewModel.CourseFiltersModel.StartDateDay = DateTime.Now.Day.ToString();
                viewModel.CourseFiltersModel.StartDateMonth = DateTime.Now.Month.ToString();
                viewModel.CourseFiltersModel.StartDateYear = DateTime.Now.Year.ToString();
            }
        }

        private void GetDistanceSpecified(CourseSearchFilters courseSearchFilters, CourseSearchResultsViewModel viewModel)
        {
            viewModel.CourseFiltersModel.LocationRegex = LocationRegex;
            courseSearchFilters.DistanceSpecified = viewModel.CourseFiltersModel.IsDistanceLocation;
        }

        private void ReplaceSpecialCharacters(CourseSearchFilters courseSearchFilters)
        {
            courseSearchFilters.Location =
                StringManipulationExtension.ReplaceSpecialCharacters(
                    courseSearchFilters.Location,
                    InvalidCharactersRegexPattern);
            courseSearchFilters.Provider =
                StringManipulationExtension.ReplaceSpecialCharacters(
                    courseSearchFilters.Provider,
                    InvalidCharactersRegexPattern);
        }
        #endregion
    }
}