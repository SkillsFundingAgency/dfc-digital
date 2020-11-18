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
using System.Web.Mvc;
using Telerik.Sitefinity.Mvc;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Controllers
{
    [ControllerToolboxItem(Name = "PreSearchFiltersResults", Title = "Pre Search Results", SectionName = SitefinityConstants.CustomWidgetSection)]
    public class PsfSearchController : BaseDfcController
    {
        #region Private Fields

        /// <summary>
        /// The search service
        /// </summary>
        private readonly ISearchQueryService<JobProfileIndex> searchQueryService;

        private readonly IWebAppContext webAppContext;
        private readonly IMapper mapper;
        private readonly IAsyncHelper asyncHelper;
        private readonly IBuildSearchFilterService buildSearchFilterService;
        private IPreSearchFilterStateManager preSearchFilterStateManager;

        #endregion Private Fields

        #region Constructors

        public PsfSearchController(ISearchQueryService<JobProfileIndex> searchQueryService, IWebAppContext webAppContext, IMapper mapper, IAsyncHelper asyncHelper, IBuildSearchFilterService buildSearchFilterService, IPreSearchFilterStateManager preSearchFilterStateManager, IApplicationLogger loggingService) : base(loggingService)
        {
            this.searchQueryService = searchQueryService;
            this.webAppContext = webAppContext;
            this.mapper = mapper;
            this.asyncHelper = asyncHelper;
            this.buildSearchFilterService = buildSearchFilterService;
            this.preSearchFilterStateManager = preSearchFilterStateManager;
        }

        #endregion Constructors

        #region Public Properties

        /// <summary>
        /// Gets or sets the main page title.
        /// </summary>
        /// <value>
        /// The main page title.
        /// </value>
        [DisplayName("Main Page Title")]
        public string MainPageTitle { get; set; } = "Your filtered careers";

        /// <summary>
        /// Gets or sets the secondary text.
        /// </summary>
        /// <value>
        /// The secondary text.
        /// </value>
        [DisplayName("Secondary Text")]
        public string SecondaryText { get; set; } = "Below you'll find appropriate careers based on the choices you made on the previous page(s).";

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

        /// <summary>
        /// Gets or sets the job profile category page.
        /// </summary>
        /// <value>
        /// The job profile category page.
        /// </value>
        [DisplayName("Jobprofile Category Page")]
        public string JobProfileCategoryPage { get; set; } = "/job-categories/";

        /// <summary>
        /// Gets or sets the PSF search results page.
        /// </summary>
        /// <value>
        /// The PSF search results page.
        /// </value>
        public string PsfSearchResultsPage { get; set; } = "filter-search-results";

        /// <summary>
        /// Gets or sets the previous page URL.
        /// </summary>
        /// <value>
        /// The back page URL.
        /// </value>
        [DisplayName("Back Page Url")]
        public string BackPageUrl { get; set; } = "/previous-page-url/";

        /// <summary>
        /// Gets or sets the previous page URL text.
        /// </summary>
        /// <value>
        /// The back page URL text.
        /// </value>
        [DisplayName("Back Page Url Text")]
        public string BackPageUrlText { get; set; } = "Back";

        /// <summary>
        /// Gets or sets the index field operators.
        /// </summary>
        /// <value>
        /// The index field operators.
        /// </value>
        [DisplayName("Index Field Operators")]
        public string IndexFieldOperators { get; set; } = "EntryQualifications|and,Interests|and,JobAreas|and,Enablers|nand,TrainingRoutes|nand,PreferredTaskTypes|and";

        [DisplayName("Index search fields, comma seperated. Used to rank the search results after filtering")]
        public string IndexSearchField { get; set; } = "Skills";

        [DisplayName("Index sort fields, comma seperated. Used to sort the search results after filtering and ranking, the sort field needs to already be in the index")]
        public string IndexSortField { get; set; } = "search.score() desc, EntryQualificationLowestLevel desc";

        /// <summary>
        /// Gets or sets message to be displayed when there are no results
        /// </summary>
        [DisplayName("No results found message")]
        public string NoResultsMessage { get; set; } = "0 results found - try again using a different filters";

        [DisplayName("Text when Salary does not have values.  If you change this value, you will also need to change the reciprocal value in JobProfileDetails widget on 'Job profiles' page.")]
        public string SalaryBlankText { get; set; } = "Variable";

        [DisplayName("Demo Interests Value")]
        public string DemoInterestsValues { get; set; } =
            "true,4c029fc4-2d4d-49ab-841a-ff137a6a4040|finance~true,eed1753f-2df8-42c5-bda1-edcb5dc295cb|totaly-wierd-stuff~false,hdhdhdhdhdhdhd";

        [DisplayName("Demo Enablers Values")]
        public string DemoEnablersValues { get; set; } = "true,c32e091b-9f51-49e5-b8b1-7ba50256c31e|drivers-license~false,hdhdhdhdhdhdhd";

        [DisplayName("Demo Training Values")]
        public string DemoTrainingRoutesValues { get; set; } = "false,37a6a4040|finance~true,fe9c4668-ae43-42ff-8870-85dbc983e697|work-your-way-up~false,hdhdhdhdhdhdhd";

        [DisplayName("Demo Entry Qualifications value")]
        public string DemoEntryQualificationsValues { get; set; } = "true,level-8~true,level-6~false,hdhdhdhdhdhdhd";

        [DisplayName("Demo Job Areas Values")]
        public string DemoJobAreasValues { get; set; } = "false,37a6a4040|finance~true,fe9c4668-ae43-42ff-8870-85dbc983e697|work-your-way-up~false,hdhdhdhdhdhdhd";

        [DisplayName("Demo Preferred Tasks value")]
        public string DemoPreferredTaskTypesValues { get; set; } = "true,level-8~true,level-6~false,hdhdhdhdhdhdhd";

        [DisplayName("Caveat index field")]
        public string CaveatFinderIndexFieldName { get; set; } = nameof(JobProfileIndex.TrainingRoutes);

        [DisplayName("Caveat index field value e.g. Covid19")]
        public string CaveatFinderIndexValue { get; set; } = "no";

        [DisplayName("Caveat tag for title")]
        public string CaveatTagMarkup { get; set; } = @"<strong class=""govuk-tag govuk-tag--grey"">COVID Impacted</strong>";

        [DisplayName("Caveat disclaimer")]
        public string CaveatMarkup { get; set; } = @"<p class=""govuk-inset-text"">This may be impacted in the short term due to current Coronavirus pandemic</p>";

        public string OverviewMessage { get; set; } = @"<div class=""govuk-warning-text"">
                <span class=""govuk-warning-text__icon"" aria-hidden=""true"">!</span>
                <strong class=""govuk-warning-text__text"">
                    <span class=""govuk-warning-text__assistive"">Warning</span>
                    If the job roles you have been matched to are COVID impacted, you should
                    <a href= ""/contact-us"" class=""govuk-link"">contact our advisers</a> to consider your options and next steps.
                 </strong>
            </div>";

        #endregion Public Properties

        #region Actions

        // GET: PSFSearch
        public ActionResult Index()
        {
            if (!webAppContext.IsContentAuthoringSite)
            {
                return Redirect("\\");
            }

            var model = GetDummyPreSearchFiltersModel();

            return Search(model);
        }

        [HttpPost]
        public ActionResult Index(PsfModel model, PsfSearchResultsViewModel resultsViewModel, int page = 1)
        {
            if (model?.Section != null)
            {
                return Search(model, page);
            }
            else
            {
                return Search(resultsViewModel?.PreSearchFiltersModel, page, false);
            }
        }

        [HttpPost]
        public ActionResult Search(PsfModel model, int page = 1, bool notPaging = true)
        {
            return asyncHelper.Synchronise(() => DisplaySearchResultsAsync(model, page, notPaging));
        }

        private static void AddFilterSection(PsfSection currentSection, PsfModel model, string demovalues)
        {
            var values = demovalues.Split('~');
            foreach (var value in values)
            {
                var data = value.Split(',');
                if (data.Length == 2)
                {
                    currentSection.Options.Add(new PsfOption
                    {
                        IsSelected = Convert.ToBoolean(data[0]),
                        OptionKey = Convert.ToString(data[1])
                    });
                }
            }

            if (currentSection.Options.Any())
            {
                model.Sections.Add(currentSection);
            }
        }

        private PsfModel GetDummyPreSearchFiltersModel()
        {
            var model = new PsfModel
            {
                Sections = new List<PsfSection>(),
                Section = new PsfSection { Options = new List<PsfOption>() }
            };

            // interests
            var interestSection = new PsfSection
            {
                Name = nameof(JobProfileIndex.Interests),
                Options = new List<PsfOption>(),
                SectionDataType = "Interest",
            };
            if (!string.IsNullOrWhiteSpace(DemoInterestsValues))
            {
                AddFilterSection(interestSection, model, DemoInterestsValues);
            }

            // enablers
            var enablersSection = new PsfSection
            {
                Name = nameof(JobProfileIndex.Enablers),
                SectionDataType = nameof(PreSearchFilterType.Enabler),
                Options = new List<PsfOption>(),
            };
            if (!string.IsNullOrWhiteSpace(DemoEnablersValues))
            {
                AddFilterSection(enablersSection, model, DemoEnablersValues);
            }

            // training routes
            var trainingRouteSection = new PsfSection
            {
                Name = nameof(JobProfileIndex.TrainingRoutes),
                SectionDataType = nameof(PreSearchFilterType.TrainingRoute),
                Options = new List<PsfOption>(),
            };
            if (!string.IsNullOrWhiteSpace(DemoTrainingRoutesValues))
            {
                AddFilterSection(trainingRouteSection, model, DemoTrainingRoutesValues);
            }

            // entry qualifics
            var entrySection = new PsfSection
            {
                Name = nameof(JobProfileIndex.EntryQualifications),
                Options = new List<PsfOption>(),
                SectionDataType = nameof(PreSearchFilterType.EntryQualification),
            };
            if (!string.IsNullOrWhiteSpace(DemoEntryQualificationsValues))
            {
                AddFilterSection(entrySection, model, DemoEntryQualificationsValues);
            }

            //job areas
            var jobAreas = new PsfSection
            {
                Name = nameof(JobProfileIndex.EntryQualifications),
                Options = new List<PsfOption>(),
                SectionDataType = nameof(PreSearchFilterType.JobArea),
            };
            if (!string.IsNullOrWhiteSpace(DemoJobAreasValues))
            {
                AddFilterSection(jobAreas, model, DemoJobAreasValues);
            }

            // preferred tasks
            var preferredTasks = new PsfSection
            {
                Name = nameof(JobProfileIndex.EntryQualifications),
                Options = new List<PsfOption>(),
                SectionDataType = nameof(PreSearchFilterType.PreferredTaskType),
            };
            if (!string.IsNullOrWhiteSpace(DemoPreferredTaskTypesValues))
            {
                AddFilterSection(preferredTasks, model, DemoPreferredTaskTypesValues);
            }

            return model;
        }

        private async Task<ActionResult> DisplaySearchResultsAsync(PsfModel model, int page, bool notPaging = true)
        {
            var resultModel = GetPsfSearchResultsViewModel(model, notPaging);

            var pageNumber = page > 0 ? page : 1;
            var fieldDefinitions = GetIndexFieldDefinitions();
            var resultsModel = mapper.Map<PreSearchFiltersResultsModel>(model);
            var properties = new SearchProperties
            {
                Page = pageNumber,
                SearchFields = new List<string>() { IndexSearchField },
                Count = this.PageSize,
                UseRawSearchTerm = true,
                FilterBy = buildSearchFilterService.BuildPreSearchFilters(resultsModel, fieldDefinitions.ToDictionary(k => k.Key, v => v.Value)),
                OrderByFields = IndexSortField.TrimEnd(',').Split(',').ToList(),
            };

            var searchTerm = buildSearchFilterService.GetSearchTerm(properties, resultsModel, IndexSearchField.Split(','));
            var results = await searchQueryService.SearchAsync(searchTerm, properties);
            resultModel.Count = results.Count;
            resultModel.PageNumber = pageNumber;
            resultModel.SearchResults = mapper.Map<IEnumerable<JobProfileSearchResultItemViewModel>>(results.Results, opts =>
            {
                opts.Items.Add(nameof(CaveatFinderIndexFieldName), CaveatFinderIndexFieldName);
                opts.Items.Add(nameof(CaveatFinderIndexValue), CaveatFinderIndexValue);
            });

            SetMatchingSkillsCount(resultModel, resultsModel, results);

            foreach (var resultItem in resultModel.SearchResults)
            {
                resultItem.ResultItemUrlName = $"{JobProfileDetailsPage}{resultItem.ResultItemUrlName}";
            }

            SetTotalResultsMessage(resultModel);
            SetupPagination(resultModel);
            return View("SearchResult", resultModel);
        }

        private void SetMatchingSkillsCount(JobProfileSearchResultViewModel resultViewModel, PreSearchFiltersResultsModel preSearchFiltersResultsModel, SearchResult<JobProfileIndex> searchResult)
        {
            var fieldFilter = preSearchFiltersResultsModel.Sections?.FirstOrDefault(section =>
                    section.SectionDataTypes.Equals("Skills", StringComparison.InvariantCultureIgnoreCase));

            if (fieldFilter?.Options.Count > 0)
            {
                var selectedSkills = fieldFilter.Options.Where(opt => opt.IsSelected).Select(opt => opt.OptionKey);
                foreach (var viewItem in resultViewModel.SearchResults)
                {
                    var resultItem = searchResult.Results.Where(r => r.ResultItem.UrlName == viewItem.ResultItemUrlName).FirstOrDefault().ResultItem;
                    viewItem.MatchingSkillsCount = resultItem.Skills.Count(skill => selectedSkills.Contains(skill));
                }
            }
        }

        private PsfSearchResultsViewModel GetPsfSearchResultsViewModel(PsfModel model, bool notPaging)
        {
            preSearchFilterStateManager.RestoreState(model.OptionsSelected);
            if (notPaging)
            {
                if (model.Section.SingleSelectedValue != null)
                {
                    model.Section.SingleSelectOnly = true;
                    var optionSelec =
                        model.Section.Options.FirstOrDefault(o => o.OptionKey == model.Section.SingleSelectedValue);
                    if (optionSelec != null)
                    {
                        optionSelec.IsSelected = true;
                    }
                }
                else
                {
                    model.Section.SingleSelectOnly = false;
                }

                var psfilterSection = mapper.Map<PreSearchFilterSection>(model.Section);

                preSearchFilterStateManager.UpdateSectionState(psfilterSection);
            }

            var filterState = preSearchFilterStateManager.GetPreSearchFilterState();

            model.Sections = mapper.Map<List<PsfSection>>(filterState.Sections);

            var resultModel = new PsfSearchResultsViewModel
            {
                MainPageTitle = MainPageTitle,
                SecondaryText = SecondaryText,
                OverviewMessage = OverviewMessage,
                PreSearchFiltersModel = new PsfModel
                {
                    OptionsSelected = preSearchFilterStateManager.GetStateJson(),
                    Section = new PsfSection
                    {
                        PageNumber = notPaging ? model.Section.PageNumber++ : model.Section.PageNumber
                    }
                },
                BackPageUrl = new Uri(BackPageUrl, UriKind.RelativeOrAbsolute),
                BackPageUrlText = BackPageUrlText,
                JobProfileCategoryPage = JobProfileCategoryPage,
                SalaryBlankText = SalaryBlankText,
                CaveatTagMarkup = CaveatTagMarkup,
                CaveatMarkup = CaveatMarkup
            };

            //Need to do this to force the model we have changed to refresh
            ModelState.Clear();

            return resultModel;
        }

        private IEnumerable<KeyValuePair<string, PreSearchFilterLogicalOperator>> GetIndexFieldDefinitions()
        {
            var fields = IndexFieldOperators.Split(',');

            var fieldDefinitions = new List<KeyValuePair<string, PreSearchFilterLogicalOperator>>();
            foreach (var field in fields)
            {
                var fieldDefinition = field.Split('|');
                if (fieldDefinition.Length == 2 && Enum.TryParse<PreSearchFilterLogicalOperator>(fieldDefinition[1], true, out var operand))
                {
                    fieldDefinitions.Add(new KeyValuePair<string, PreSearchFilterLogicalOperator>(fieldDefinition[0], operand));
                }
            }

            return fieldDefinitions;
        }

        private void SetTotalResultsMessage(JobProfileSearchResultViewModel resultModel)
        {
            var totalFound = resultModel.Count;
            resultModel.TotalResultCount = totalFound;
            resultModel.TotalResultsMessage = totalFound == 0 ? NoResultsMessage : $"{totalFound} result{(totalFound == 1 ? string.Empty : "s")} found";
        }

        private void SetupPagination(JobProfileSearchResultViewModel resultModel)
        {
            resultModel.TotalPages = (int)Math.Ceiling((double)resultModel.Count / PageSize);

            if (resultModel.TotalPages > 1 && resultModel.TotalPages >= resultModel.PageNumber)
            {
                resultModel.NextPageUrl = new Uri($"{PsfSearchResultsPage}?page={resultModel.PageNumber + 1}", UriKind.RelativeOrAbsolute);
                resultModel.NextPageUrlText = $"{resultModel.PageNumber + 1} of {resultModel.TotalPages}";

                if (resultModel.PageNumber > 1)
                {
                    resultModel.PreviousPageUrl = new Uri($"{PsfSearchResultsPage}{(resultModel.PageNumber == 2 ? string.Empty : $"?page={resultModel.PageNumber - 1}")}", UriKind.RelativeOrAbsolute);
                    resultModel.PreviousPageUrlText = $"{resultModel.PageNumber - 1} of {resultModel.TotalPages}";
                }
            }
        }

        #endregion Actions
    }
}