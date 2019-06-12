using AutoMapper;
using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Core;
using DFC.Digital.Web.Sitefinity.Core;
using System;
using System.ComponentModel;
using System.Linq;
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
        private readonly ICourseSearchResultsViewModelBullder courseSearchResultsViewModelBuilder;
        private readonly IQueryStringBuilder<CourseSearchFilters> queryStringBuilder;
        private readonly IMapper mapper;

        #endregion

        #region Constructors

        public CourseSearchResultsController(
            IApplicationLogger applicationLogger,
            ICourseSearchService courseSearchService,
            IAsyncHelper asyncHelper,
            ICourseSearchResultsViewModelBullder courseSearchResultsViewModelBuilder,
            IQueryStringBuilder<CourseSearchFilters> queryStringBuilder,
            IMapper mapper)
            : base(applicationLogger)
        {
            this.courseSearchService = courseSearchService;
            this.asyncHelper = asyncHelper;
            this.courseSearchResultsViewModelBuilder = courseSearchResultsViewModelBuilder;
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
        public string CourseSearchResultsPage { get; set; } = "/course-directory/course-search-result";

        [DisplayName("Redirect - Course Details Page")]
        public string CourseDetailsPage { get; set; } = "/course-directory/course-details";

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

        [DisplayName("Filter - Clear filters text")]
        public string ResetFilterText { get; set; } = "Clear filters";

        [DisplayName("Active Filters - Provided by text")]
        public string ActiveFiltersProvidedByText { get; set; } = "Provided by";

        [DisplayName("Active Filters - 'of' text")]
        public string ActiveFiltersOfText { get; set; } = "of";

        [DisplayName("Active Filters - within text")]
        public string ActiveFiltersWithinText { get; set; } = "Within";

        [DisplayName("Active Filters - 16 - 19 year old text")]
        public string ActiveFiltersOnly1619CoursesText { get; set; } = "16-19 year olds";

        [DisplayName("Active Filters - suitable for text")]
        public string ActiveFiltersSuitableForText { get; set; } = "suitable for";

        [DisplayName("Active Filters - starting from text")]
        public string ActiveFiltersStartingFromText { get; set; } = "starting from";

        [DisplayName("Active Filters - courses text")]
        public string ActiveFiltersCoursesText { get; set; } = "courses";

        [DisplayName("Active Filters - Showing text")]
        public string ActiveFiltersShowingText { get; set; } = "Showing";

        [DisplayName("Active Filters - miles text")]
        public string ActiveFiltersMilesText { get; set; } = "miles";

        #endregion

        #region Actions

        [HttpGet]
        public ActionResult Index(CourseSearchFilters courseSearchFilters, CourseSearchProperties courseSearchProperties)
        {
            courseSearchFilters = courseSearchFilters ?? new CourseSearchFilters();
            courseSearchProperties = courseSearchProperties ?? new CourseSearchProperties();

            var viewModel = new CourseSearchResultsViewModel
            {
                CourseFiltersModel = mapper.Map<CourseFiltersViewModel>(courseSearchFilters)
            };

            var cleanCourseName =
                StringManipulationExtension.ReplaceSpecialCharacters(
                    courseSearchFilters.SearchTerm,
                    Constants.CourseSearchInvalidCharactersRegexPattern);

            if (!string.IsNullOrEmpty(cleanCourseName))
            {
                courseSearchProperties.Count = RecordsPerPage;

                courseSearchFilters.SearchTerm = cleanCourseName;

                ReplaceSpecialCharactersOnFreeTextFields(courseSearchFilters);

                courseSearchFilters.LocationRegex = Constants.CourseSearchLocationRegularExpression;

                courseSearchProperties.Filters = courseSearchFilters;

                var response = asyncHelper.Synchronise(() =>
                    courseSearchService.SearchCoursesAsync(cleanCourseName, courseSearchProperties));

                if (response.Courses.Any())
                {
                    foreach (var course in response.Courses)
                    {
                        course.CourseLink = $"{CourseDetailsPage}?{nameof(CourseDetails.CourseId)}={course.CourseId}";
                        viewModel.Courses.Add(new CourseListingViewModel
                        {
                            Course = course,
                            AdvancedLoanProviderLabel = AdvancedLoanProviderLabel,
                            LocationLabel = LocationLabel,
                            ProviderLabel = ProviderLabel,
                            StartDateLabel = StartDateLabel
                        });
                    }

                    SetupResultsViewModel(viewModel, response);
                }

                SetupStartDateDisplayData(viewModel);
                viewModel.ResetFilterUrl = new Uri($"{CourseSearchResultsPage}?{nameof(CourseSearchFilters.SearchTerm)}={viewModel.CourseFiltersModel.SearchTerm}", UriKind.RelativeOrAbsolute);
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

        private static void ReplaceSpecialCharactersOnFreeTextFields(CourseSearchFilters courseSearchFilters)
        {
            courseSearchFilters.Location =
                StringManipulationExtension.ReplaceSpecialCharacters(
                    courseSearchFilters.Location,
                    Constants.CourseSearchInvalidCharactersRegexPattern);
            courseSearchFilters.Provider =
                StringManipulationExtension.ReplaceSpecialCharacters(
                    courseSearchFilters.Provider,
                    Constants.CourseSearchInvalidCharactersRegexPattern);
        }

        private static void SetupStartDateDisplayData(CourseSearchResultsViewModel viewModel)
        {
            if (viewModel.CourseFiltersModel.StartDate == StartDate.SelectDateFrom && !viewModel.CourseFiltersModel.StartDateFrom.Equals(DateTime.MinValue))
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

        private static void PopulateSelectFromDate(CourseFiltersViewModel viewModel)
        {
            if (viewModel.StartDate == StartDate.SelectDateFrom && DateTime.TryParse(
                    $"{viewModel.StartDateYear}-{viewModel.StartDateMonth}-{viewModel.StartDateDay}", out var chosenDate))
            {
                viewModel.StartDateFrom = chosenDate;
            }
        }

        private void SetupResultsViewModel(CourseSearchResultsViewModel viewModel, CourseSearchResult response)
        {
            var pathQuery = Request?.Url?.PathAndQuery;
            if (!string.IsNullOrWhiteSpace(pathQuery) &&
                pathQuery.IndexOf($"&{nameof(CourseSearchResultProperties.Page)}=", StringComparison.InvariantCultureIgnoreCase) > 0)
            {
                pathQuery = pathQuery.Substring(
                    0,
                    pathQuery.IndexOf($"&{nameof(CourseSearchResultProperties.Page)}=", StringComparison.InvariantCultureIgnoreCase));
            }

            courseSearchResultsViewModelBuilder.SetupViewModelPaging(
                viewModel,
                response,
                pathQuery,
                RecordsPerPage);

           viewModel.OrderByLinks = courseSearchResultsViewModelBuilder.GetOrderByLinks(pathQuery, response.ResultProperties.OrderedBy);
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
            viewModel.CourseFiltersModel.LocationRegex = Constants.CourseSearchLocationRegularExpression;
            viewModel.ResetFiltersText = ResetFilterText;
            viewModel.CourseFiltersModel.ActiveFiltersCoursesText = ActiveFiltersCoursesText;
            viewModel.CourseFiltersModel.ActiveFiltersMilesText = ActiveFiltersMilesText;
            viewModel.CourseFiltersModel.ActiveFiltersOfText = ActiveFiltersOfText;
            viewModel.CourseFiltersModel.ActiveFiltersOnly1619CoursesText = ActiveFiltersOnly1619CoursesText;
            viewModel.CourseFiltersModel.ActiveFiltersShowingText = ActiveFiltersShowingText;
            viewModel.CourseFiltersModel.ActiveFiltersWithinText = ActiveFiltersWithinText;
            viewModel.CourseFiltersModel.ActiveFiltersProvidedByText = ActiveFiltersProvidedByText;
            viewModel.CourseFiltersModel.ActiveFiltersStartingFromText = ActiveFiltersStartingFromText;
            viewModel.CourseFiltersModel.ActiveFiltersSuitableForText = ActiveFiltersSuitableForText;
        }

        #endregion
    }
}