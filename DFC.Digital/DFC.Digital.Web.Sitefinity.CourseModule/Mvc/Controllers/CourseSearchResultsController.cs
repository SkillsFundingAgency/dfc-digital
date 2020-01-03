using AutoMapper;
using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Core;
using DFC.Digital.Web.Sitefinity.Core;
using System;
using System.Linq;
using System.Web.Mvc;
using Telerik.Sitefinity.Mvc;

namespace DFC.Digital.Web.Sitefinity.CourseModule.Mvc.Controllers
{
    [ControllerToolboxItem(Name = "CourseSearchResults", Title = "Course Search Results", SectionName = SitefinityConstants.CustomCoursesSection)]
    public class CourseSearchResultsController : BaseDfcController
    {
        #region private fields

        private const string SearchTermTokenToReplace = "{searchTerm}";
        private readonly ICourseSearchService courseSearchService;
        private readonly IAsyncHelper asyncHelper;
        private readonly ICourseSearchResultsViewModelBullder courseSearchResultsViewModelBuilder;
        private readonly IWebAppContext context;
        private readonly IMapper mapper;

        #endregion

        #region Constructors

        public CourseSearchResultsController(
            IApplicationLogger applicationLogger,
            ICourseSearchService courseSearchService,
            IAsyncHelper asyncHelper,
            ICourseSearchResultsViewModelBullder courseSearchResultsViewModelBuilder,
            IWebAppContext context,
            IMapper mapper)
            : base(applicationLogger)
        {
            this.courseSearchService = courseSearchService;
            this.asyncHelper = asyncHelper;
            this.courseSearchResultsViewModelBuilder = courseSearchResultsViewModelBuilder;
            this.context = context;
            this.mapper = mapper;
        }

        #endregion Constructors

        #region Public Properties
        public string PageTitle { get; set; } = "Search";

        public int RecordsPerPage { get; set; } = 20;

        public string CourseSearchResultsPage { get; set; } = "/find-a-course/course-search-result";

        public string CourseDetailsPage { get; set; } = "/find-a-course/course-details";

        public string NoTrainingCoursesFoundText { get; set; } = "<p class=\"govuk-body\">We didn't find any results for {searchTerm} with the active filters you've applied. Try searching again.</p><p class=\"govuk-body\">You could:</p><ul class=\"list list-bullet govuk-body\">    <li>check your spelling</li>    <li>change the start date</li>    <li>check your location or postcode</li>    <li>change your filters</li>    <li>try different search terms</li></ul>";

        public string SearchForCourseNameText { get; set; } = "Course name";

        public string OrderByText { get; set; } = "Sort by:";

        public string RelevanceOrderByText { get; set; } = "Relevance";

        public string DistanceOrderByText { get; set; } = "Distance";

        public string StartDateOrderByText { get; set; } = "Start date";

        public string LocationLabel { get; set; } = "Location:";

        public string ProviderLabel { get; set; } = "Provider:";

        public string AdvancedLoanProviderLabel { get; set; } = "Advanced Learner Loans offered by this Provider:";

        public string StartDateLabel { get; set; } = "Start date:";

        public string Only1619CoursesText { get; set; } = "Suitable for 16-19 year olds";

        public string StartDateExampleText { get; set; } = "For example, 01 01 2020";

        public string CourseHoursSectionText { get; set; } = "Course hours";

        public string StartDateSectionText { get; set; } = "Start date";

        public string CourseTypeSectionText { get; set; } = "Course type";

        public string ApplyFiltersText { get; set; } = "Apply filters";

        public string WithinText { get; set; } = "Within";

        public string ResetFilterText { get; set; } = "Clear filters";

        public string ActiveFiltersProvidedByText { get; set; } = "Provided by";

        public string ActiveFiltersOfText { get; set; } = "of";

        public string ActiveFiltersWithinText { get; set; } = "Within";

        public string ActiveFiltersOnly1619CoursesText { get; set; } = "16-19 year olds";

        public string ActiveFiltersSuitableForText { get; set; } = "suitable for";

        public string ActiveFiltersStartingFromText { get; set; } = "starting from";

        public string ActiveFiltersCoursesText { get; set; } = "courses";

        public string ActiveFiltersShowingText { get; set; } = "Showing";

        public string ActiveFiltersMilesText { get; set; } = "miles";

        public string FilterProviderLabel { get; set; } = "Course provider";

        public string FilterLocationLabel { get; set; } = "Location";

        public string Only1619CoursesSectionText { get; set; } = "Age suitability";

        #endregion

        #region Actions

        [HttpGet]
        public ActionResult Index(CourseFiltersViewModel filtersInput, CourseSearchProperties inputSearchProperties)
        {
            var cleanedSearchTerm = filtersInput.SearchTerm.ReplaceSpecialCharacters(Constants.CourseSearchInvalidCharactersRegexPattern);
            var courseSearchResults = new CourseSearchResultsViewModel
            {
                CourseFiltersModel = filtersInput,
                ResetFilterUrl = new Uri($"{CourseSearchResultsPage}?{nameof(CourseSearchFilters.SearchTerm)}={filtersInput.SearchTerm}", UriKind.RelativeOrAbsolute),
                NoTrainingCoursesFoundText = NoTrainingCoursesFoundText.Replace(SearchTermTokenToReplace, $"'{filtersInput.SearchTerm}'"),
            };

            if (!filtersInput.IsDistanceLocation)
            {
                filtersInput.Town = filtersInput.Postcode;
                filtersInput.Postcode = null;
            }

            if (!string.IsNullOrEmpty(cleanedSearchTerm))
            {
                //create a new object if invoked from landing page
                var courseSearchProperties = inputSearchProperties ?? new CourseSearchProperties();
                courseSearchProperties.Count = RecordsPerPage;
                courseSearchProperties.Filters = mapper.Map<CourseSearchFilters>(filtersInput);
                courseSearchProperties.Filters.DistanceSpecified = filtersInput.IsDistanceLocation && (filtersInput.Distance > 0);
                ReplaceSpecialCharactersOnFreeTextFields(courseSearchProperties.Filters);

                var combinedDate = $"{filtersInput.StartDateYear}/{filtersInput.StartDateMonth}/{filtersInput.StartDateDay}";
                if (DateTime.TryParse(combinedDate, out DateTime result))
                {
                    courseSearchProperties.Filters.StartDateFrom = result;
                }

                var response = asyncHelper.Synchronise(() => courseSearchService.SearchCoursesAsync(courseSearchProperties));
                if (response.Courses.Any())
                {
                    foreach (var course in response.Courses)
                    {
                        course.CourseLink = $"{CourseDetailsPage}?{nameof(CourseDetails.CourseId)}={course.CourseId}&runId={course.RunId}&referralPath={context.GetUrlEncodedPathAndQuery()}";
                        courseSearchResults.Courses.Add(new CourseListingViewModel
                        {
                            Course = course,
                            AdvancedLoanProviderLabel = AdvancedLoanProviderLabel,
                            LocationLabel = LocationLabel,
                            ProviderLabel = ProviderLabel,
                            StartDateLabel = StartDateLabel
                        });
                    }

                    SetupResultsViewModel(courseSearchResults, response);
                }

                SetupStartDateDisplayData(courseSearchResults);
            }

            SetupWidgetLabelsAndTextDefaults(courseSearchResults);
            return View("SearchResults", courseSearchResults);
        }

        #endregion Actions

        #region private Methods

        private static void ReplaceSpecialCharactersOnFreeTextFields(CourseSearchFilters courseSearchFilters)
        {
            courseSearchFilters.SearchTerm = courseSearchFilters.SearchTerm.ReplaceSpecialCharacters(Constants.CourseSearchInvalidCharactersRegexPattern);
            courseSearchFilters.Postcode = courseSearchFilters.Postcode.ReplaceSpecialCharacters(Constants.CourseSearchInvalidCharactersRegexPattern);
            courseSearchFilters.Town = courseSearchFilters.Town.ReplaceSpecialCharacters(Constants.CourseSearchInvalidCharactersRegexPattern);
            courseSearchFilters.Provider = courseSearchFilters.Provider.ReplaceSpecialCharacters(Constants.CourseSearchInvalidCharactersRegexPattern);
        }

        private static void SetupStartDateDisplayData(CourseSearchResultsViewModel viewModel)
        {
            var combinedDate = $"{viewModel.CourseFiltersModel.StartDateYear}/{viewModel.CourseFiltersModel.StartDateMonth}/{viewModel.CourseFiltersModel.StartDateDay}";
            if (DateTime.TryParse(combinedDate, out DateTime result))
            {
                viewModel.CourseFiltersModel.StartDateFrom = result;
                viewModel.CourseFiltersModel.StartDateDay = result.Day.ToString();
                viewModel.CourseFiltersModel.StartDateMonth = result.Month.ToString();
                viewModel.CourseFiltersModel.StartDateYear = result.Year.ToString();
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
            courseSearchResultsViewModelBuilder.SetupViewModelPaging(
                viewModel,
                response,
                CourseSearchResultsPage,
                RecordsPerPage);

            viewModel.OrderByLinks = courseSearchResultsViewModelBuilder.GetOrderByLinks(CourseSearchResultsPage, response.ResultProperties.OrderedBy);
        }

        private void SetupWidgetLabelsAndTextDefaults(CourseSearchResultsViewModel viewModel)
        {
            viewModel.PageTitle = PageTitle;
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
            viewModel.CourseFiltersModel.FilterLocationLabel = FilterLocationLabel;
            viewModel.CourseFiltersModel.FilterProviderLabel = FilterProviderLabel;
            viewModel.CourseFiltersModel.Only1619CoursesSectionText = Only1619CoursesSectionText;
        }

        #endregion
    }
}