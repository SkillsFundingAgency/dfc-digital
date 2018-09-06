using AutoMapper;
using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Core;
using DFC.Digital.Web.Sitefinity.Core;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Telerik.Sitefinity.Mvc;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Controllers
{
    /// <summary>
    /// Custom Widget for the Search box and Search results for Job Profiles
    /// </summary>
    /// <seealso cref="DFC.Digital.Web.CoreDfcController" />
    [ControllerToolboxItem(Name = "JobProfileSearchBox", Title = "JobProfile SearchBox", SectionName = SitefinityConstants.CustomWidgetSection)]
    public class JobProfileSearchBoxController : BaseDfcController
    {
        #region Private Fields

        /// <summary>
        /// The search service
        /// </summary>
        private readonly ISearchQueryService<JobProfileIndex> searchQueryService;

        private readonly IWebAppContext webAppContext;
        private readonly IMapper mapper;
        private readonly IAsyncHelper asyncHelper;
        private readonly ISpellcheckService spellcheckService;

        #endregion Private Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="JobProfileSearchBoxController"/> class.
        /// Constructor JobProfileSearchBoxController
        /// </summary>
        /// <param name="searchService">searchService</param>
        /// <param name="webAppContext">webAppContext</param>
        /// <param name="mapper">mapper</param>
        /// <param name="applicationLogger">applicationLogger</param>
        /// <param name="asyncHelper">asyncHelper</param>
        /// <param name="spellcheckService">spellCheckService</param>
        public JobProfileSearchBoxController(ISearchQueryService<JobProfileIndex> searchService, IWebAppContext webAppContext, IMapper mapper, IApplicationLogger applicationLogger, IAsyncHelper asyncHelper, ISpellcheckService spellcheckService) : base(applicationLogger)
        {
            this.searchQueryService = searchService;
            this.webAppContext = webAppContext;
            this.mapper = mapper;
            this.asyncHelper = asyncHelper;
            this.spellcheckService = spellcheckService;
        }

        #endregion Constructors

        #region Public Properties

        [TypeConverter(typeof(EnumConverter))]
        [DisplayName("Current Page Mode : 'Landing' or 'JobProfile' (Display only search box) or 'SearchResults' (Display both search box and results")]
        public SearchWidgetPageMode CurrentPageMode { get; set; } = SearchWidgetPageMode.Landing;

        /// <summary>
        /// Gets or sets the placeholder text.
        /// </summary>
        /// <value>
        /// The placeholder text.
        /// </value>
        [DisplayName("Place holder text")]
        public string PlaceholderText { get; set; } = "Enter a job title or keywords";

        /// <summary>
        /// Gets or sets the header text.
        /// </summary>
        /// <value>
        /// The header text.
        /// </value>
        [DisplayName("Header text {Only JobProfile Page)")]
        public string HeaderText { get; set; } = "Search further careers";

        /// <summary>
        /// Gets or sets message to be displayed when there are no results
        /// </summary>
        [DisplayName("No results found message")]
        public string NoResultsMessage { get; set; } = "0 results found - try again using a different job title";

        /// <summary>
        /// Gets or sets the search results page.
        /// </summary>
        /// <value>
        /// The search results page.
        /// </value>
        [DisplayName("Search Results Page")]
        public string SearchResultsPage { get; set; } = "/search-results";

        /// <summary>
        /// Gets or sets the size of the page.
        /// </summary>
        /// <value>
        /// The size of the page.
        /// </value>
        [DisplayName("Results Page Size")]
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// Gets or sets the job profile details page.
        /// </summary>
        /// <value>
        /// The job profile details page.
        /// </value>
        [DisplayName("Job Profile Details Page")]
        public string JobProfileDetailsPage { get; set; } = "/job-profiles/";

        [DisplayName("Jobprofile Category Page")]
        public string JobProfileCategoryPage { get; set; } = "/job-categories/";

        /// <summary>
        /// Gets or sets the AutoComplete Minimum Characters
        /// </summary>
        /// <value>
        /// The AutoComplete Minimum Characters.
        /// </value>
        [DisplayName("Character limit to invoke/activate autocomplete")]
        public int AutoCompleteMinimumCharacters { get; set; } = 2;

        /// <summary>
        /// Gets or sets the Maximum Number ff displayed Suggestions.
        /// </summary>
        /// <value>
        /// The Maximum Number ff displayed Suggestions.
        /// </value>
        [DisplayName("Maximum number Job Profiles displayed in autocomplete drop down")]
        public int MaximumNumberOfDisplayedSuggestions { get; set; } = 5;

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets the Fuzzy AutoComplete
        /// </summary>
        /// <value>
        /// The Fuzzy AutoComplete
        /// </value>
        [DisplayName("Switch for FuzzyAutoComplete matching (True/False)")]
        public bool UseFuzzyAutoCompleteMatching { get; set; } = true;

        [DisplayName("Text when Salary does not have values.  If you change this value, you will also need to change the reciprocal value in JobProfileDetails widget on 'Job profiles' page.")]
        public string SalaryBlankText { get; set; } = "Variable";

        #endregion Public Properties

        #region Actions

        /// <summary>
        /// Searches the results.
        /// </summary>
        /// <param name="searchTerm">The keyword.</param>
        /// <param name="jobProfileUrl">jp</param>
        /// <returns>searchresult</returns>
        [HttpPost]
        [RelativeRoute("{urlName}")]
        public ActionResult Search(string searchTerm, string jobProfileUrl)
        {
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                return Redirect($"{SearchResultsPage}?searchTerm={GetUrlEncodedString(searchTerm)}");
            }

            switch (CurrentPageMode)
            {
                case SearchWidgetPageMode.SearchResults:
                    var searchModel = new JobProfileSearchResultViewModel
                    {
                        PlaceholderText = PlaceholderText,
                        AutoCompleteMinimumCharacters = AutoCompleteMinimumCharacters,
                        MaximumNumberOfDisplayedSuggestions = MaximumNumberOfDisplayedSuggestions,
                        UseFuzzyAutoCompleteMatching = UseFuzzyAutoCompleteMatching,
                        SalaryBlankText = SalaryBlankText
                    };
                    return View("SearchResult", searchModel);

                case SearchWidgetPageMode.JobProfile:
                    bool isValidUrl = this.webAppContext.IsValidAndFormattedUrl($"{JobProfileDetailsPage}{jobProfileUrl}/");
                    return isValidUrl
                            ? (ActionResult)new RedirectResult($"{JobProfileDetailsPage}{jobProfileUrl}")
                            : HttpNotFound();

                default:
                    var model = new JobProfileSearchBoxViewModel
                    {
                        PlaceholderText = PlaceholderText,
                        AutoCompleteMinimumCharacters = AutoCompleteMinimumCharacters,
                        MaximumNumberOfDisplayedSuggestions = MaximumNumberOfDisplayedSuggestions,
                        UseFuzzyAutoCompleteMatching = UseFuzzyAutoCompleteMatching
                    };
                    return View("Index", model);
            }
        }

        // GET: JobProfileSearchBox

        /// <summary>
        /// entry point to the widget to show or a landing version or search results.
        /// </summary>
        /// <param name="searchTerm">searchParam</param>
        /// <param name="page">page</param>
        /// <returns>result</returns>
        [HttpGet]
        [RelativeRoute("")]
        public ActionResult Index(string searchTerm, int page = 1)
        {
            switch (CurrentPageMode)
            {
                case SearchWidgetPageMode.SearchResults:
                    //Damn!!!! Sitefinity doesnt support Async await
                    //https://feedback.telerik.com/Project/153/Feedback/Details/165662-mvc-ability-to-use-async-actions-in-mvc-widgets
                    return asyncHelper.Synchronise(() => DisplaySearchResultsAsync(searchTerm, page));

                case SearchWidgetPageMode.Landing:
                default:
                    var model = new JobProfileSearchBoxViewModel
                    {
                        PlaceholderText = PlaceholderText,
                        TotalResultsMessage = NoResultsMessage,
                        AutoCompleteMinimumCharacters = AutoCompleteMinimumCharacters,
                        MaximumNumberOfDisplayedSuggestions = MaximumNumberOfDisplayedSuggestions,
                        UseFuzzyAutoCompleteMatching = UseFuzzyAutoCompleteMatching
                    };
                    return View("Index", model);
            }
        }

        //Todo -> Please consider Configuring multiple toutes on the action if the Action name is same and the routing is done from various source
        //Todo - > For example the Action Index can have multiple routes defined - Both of the Index action can be clubbed together with multiple routes
        [HttpGet]
        [RelativeRoute("{urlName}")]
        public ActionResult Index(string urlName)
        {
            var model = new JobProfileSearchBoxViewModel
            {
                PlaceholderText = PlaceholderText,
                HeaderText = HeaderText,
                TotalResultsMessage = NoResultsMessage,
                JobProfileUrl = new Uri(urlName, UriKind.RelativeOrAbsolute),
                AutoCompleteMinimumCharacters = AutoCompleteMinimumCharacters,
                MaximumNumberOfDisplayedSuggestions = MaximumNumberOfDisplayedSuggestions,
                UseFuzzyAutoCompleteMatching = UseFuzzyAutoCompleteMatching
            };
            return View("JobProfile", model);
        }

        [HttpGet]
        public ActionResult Suggestions(string term, int maxNumberDisplayed, bool fuzzySearch)
        {
            if (!string.IsNullOrEmpty(term))
            {
                var props = new SuggestProperties
                {
                    UseFuzzyMatching = fuzzySearch,
                    MaxResultCount = maxNumberDisplayed
                };
                var results = searchQueryService.GetSuggestion(term, props);
                var suggestions = results.Results.Select(s => new Suggestion
                {
                    label = s.MatchedSuggestion.First().ToString().ToUpper() + s.MatchedSuggestion.Substring(1)
                });
                var distinctSuggestions = suggestions.GroupBy(x => x.label).Select(x => x.First());
                return Json(distinctSuggestions, JsonRequestBehavior.AllowGet);
            }

            return new EmptyResult();
        }

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

        private async Task<ActionResult> DisplaySearchResultsAsync(string searchTerm, int page)
        {
            var resultModel = new JobProfileSearchResultViewModel
            {
                PlaceholderText = PlaceholderText,
                SearchTerm = searchTerm,
                AutoCompleteMinimumCharacters = AutoCompleteMinimumCharacters,
                MaximumNumberOfDisplayedSuggestions = MaximumNumberOfDisplayedSuggestions,
                UseFuzzyAutoCompleteMatching = UseFuzzyAutoCompleteMatching,
                JobProfileCategoryPage = JobProfileCategoryPage,
                SalaryBlankText = SalaryBlankText
            };

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var pageNumber = page > 0 ? page : 1;
                var searchTask = searchQueryService.SearchAsync(searchTerm, new SearchProperties { Page = pageNumber, Count = this.PageSize });
                var spellCheckTask = spellcheckService.CheckSpellingAsync(searchTerm);

                await Task.WhenAll(searchTask, spellCheckTask);

                var results = searchTask.Result;
                resultModel.Count = results.Count;
                resultModel.PageNumber = pageNumber;
                resultModel.SearchResults = mapper.Map<IEnumerable<JobProfileSearchResultItemViewModel>>(results.Results);
                foreach (var resultItem in resultModel.SearchResults)
                {
                    resultItem.ResultItemUrlName = $"{JobProfileDetailsPage}{resultItem.ResultItemUrlName}";
                }

                SetTotalResultsMessage(resultModel);
                SetupPagination(searchTerm, resultModel);

                var spellCheckResult = spellCheckTask.Result;
                if (spellCheckResult.HasCorrected)
                {
                    resultModel.DidYouMeanUrl = GetSearchResultsPageUrl(spellCheckResult.CorrectedTerm);
                    resultModel.DidYouMeanTerm = spellCheckResult.CorrectedTerm;
                }
            }

            return View("SearchResult", resultModel);
        }

        private Uri GetSearchResultsPageUrl(string searchTerm)
        {
            return new Uri($"{SearchResultsPage}?searchTerm={GetUrlEncodedString(searchTerm)}", UriKind.RelativeOrAbsolute);
        }

        private void SetupPagination(string searchTerm, JobProfileSearchResultViewModel resultModel)
        {
            resultModel.TotalPages = (int)Math.Ceiling((double)resultModel.Count / PageSize);

            if (resultModel.TotalPages > 1 && resultModel.TotalPages >= resultModel.PageNumber)
            {
                resultModel.NextPageUrl = new Uri($"{SearchResultsPage}?searchTerm={HttpUtility.UrlEncode(searchTerm)}&page={resultModel.PageNumber + 1}", UriKind.RelativeOrAbsolute);
                resultModel.NextPageUrlText = $"{resultModel.PageNumber + 1} of {resultModel.TotalPages}";

                if (resultModel.PageNumber > 1)
                {
                    resultModel.PreviousPageUrl = new Uri($"{SearchResultsPage}?searchTerm={HttpUtility.UrlEncode(searchTerm)}{(resultModel.PageNumber == 2 ? string.Empty : $"&page={resultModel.PageNumber - 1}")}", UriKind.RelativeOrAbsolute);
                    resultModel.PreviousPageUrlText = $"{resultModel.PageNumber - 1} of {resultModel.TotalPages}";
                }
            }
        }

        private void SetTotalResultsMessage(JobProfileSearchResultViewModel resultModel)
        {
            var totalFound = resultModel.Count;
            if (totalFound == 0)
            {
                resultModel.TotalResultsMessage = NoResultsMessage;
            }
            else
            {
                resultModel.TotalResultsMessage = $"{totalFound} result{(totalFound == 1 ? string.Empty : "s")} found";
            }
        }

        #endregion Actions
    }
}