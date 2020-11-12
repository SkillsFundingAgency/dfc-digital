using AutoMapper;
using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Repository.SitefinityCMS;
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
    [ControllerToolboxItem(Name = "PreSearchFilters", Title = "Configure Pre Search Filters", SectionName = SitefinityConstants.CustomWidgetSection)]
    public class PreSearchFiltersController : BaseDfcController
    {
        #region Private Fields

        private readonly IPreSearchFiltersFactory preSearchFiltersFactory;
        private readonly IPreSearchFilterStateManager preSearchFilterStateManager;
        private readonly IMapper autoMapper;

        private readonly ISearchQueryService<JobProfileIndex> searchQueryService;
        private readonly IBuildSearchFilterService buildSearchFilterService;
        private readonly IAsyncHelper asyncHelper;

        private readonly ITaxonomyRepository taxonomyRepository;

        #endregion Private Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PreSearchFiltersController"/> class.
        /// </summary>
        /// <param name="applicationLogger">application logger</param>
        /// <param name="preSearchFiltersFactory">Sitefinity Repository to use</param>
        /// <param name="preSearchFilterStateManager">Pre search filter state manager</param>
        /// <param name="autoMapper">Instance of auto mapper</param>
        /// <param name="searchQueryService">Instance of search query service</param>
        /// <param name="buildSearchFilterService">Instance of search filter service</param>
        /// <param name="asyncHelper">Instance of asyncHelper</param>
        /// <param name="taxonomyRepository">Instance of taxonomyRepository</param>
        public PreSearchFiltersController(
            IApplicationLogger applicationLogger,
            IMapper autoMapper,
            IPreSearchFiltersFactory preSearchFiltersFactory,
            IPreSearchFilterStateManager preSearchFilterStateManager,
            ISearchQueryService<JobProfileIndex> searchQueryService,
            IBuildSearchFilterService buildSearchFilterService,
            IAsyncHelper asyncHelper,
            ITaxonomyRepository taxonomyRepository) : base(applicationLogger)
        {
            this.preSearchFiltersFactory = preSearchFiltersFactory;
            this.autoMapper = autoMapper;
            this.preSearchFilterStateManager = preSearchFilterStateManager;
            this.searchQueryService = searchQueryService;
            this.buildSearchFilterService = buildSearchFilterService;
            this.asyncHelper = asyncHelper;
            this.taxonomyRepository = taxonomyRepository;
        }

        #endregion Constructors

        #region Public Properties

        [DisplayName("Section Title - must be unique across pages")]
        public string SectionTitle { get; set; } = "Demo Section";

        [DisplayName("Section Description")]
        public string SectionDescription { get; set; } = "Demo Description";

        [DisplayName("Section Content Type - One of  Interest, Enabler, EntryQualification, TrainingRoute, JobArea, CareerFocus, PreferredTaskType, Skill")]
        public PreSearchFilterType FilterType { get; set; }

        [DisplayName("Is this section a single option select only")]
        public bool SingleSelectOnly { get; set; } = false;

        [DisplayName("Previous page URL - where to post the form when back button is clicked")]
        public string PreviousPageUrl { get; set; } = "homepageurl";

        [DisplayName("Next page URL - where to post the form when the continue button is clicked")]
        public string NextPageUrl { get; set; } = "nextpageurl";

        [DisplayName("The number of this page in the selection sequence")]
        public int ThisPageNumber { get; set; } = 1;

        [DisplayName("The total number of selection pages")]
        public int TotalNumberOfPages { get; set; } = 99;

        [DisplayName("Index Field Operators - Should match the one in the results widget up to this point in the journey")]
        public string IndexFieldOperators { get; set; } = "Skills|and,EntryQualifications|and,JobAreas|and,Enablers|nand,TrainingRoutes|nand";

        [DisplayName("Enable Accordion")]
        public bool EnableAccordion { get; set; } = false;

        [DisplayName("Group By")]
        public string GroupFieldsBy { get; set; } = "Skills";

        [DisplayName("Should show number of profile matched banner")]
        public string NumberOfMatchesMessage { get; set; } = "We have found {0} career matches based on your selection.";

        [DisplayName("Select Message")]
        public string SelectMessage { get; set; } = @"<div class=""govuk-hint"" id=""qualifications-hint"">Select all that apply.</div>";

        [DisplayName("Use Page profile count")]
        public bool UsePageProfileCount { get; set; } = true;

        [DisplayName("Use Dummy Sitefinity Data")]
        public bool UseDummySiteFinity { get; set; } = false;

        #endregion Public Properties

        #region Actions

        // GET: PreSearchFilters
        public ActionResult Index()
        {
            return View(GetCurrentPageFilter());
        }

        [HttpPost]
        public ActionResult Index(PsfModel model, PsfSearchResultsViewModel resultsViewModel)
        {
            var startTime = DateTime.Now;

            CheckForBackState(model);

            var timeMessage = $"Stage1: {(DateTime.Now - startTime).TotalSeconds} | ";

            // If the previous page is search page then, there will not be any sections in the passed PSFModel
            var previousPsfPage = model?.Section == null ? resultsViewModel?.PreSearchFiltersModel : model;
            if (previousPsfPage != null)
            {
                preSearchFilterStateManager.RestoreState(previousPsfPage.OptionsSelected);
                if (preSearchFilterStateManager.ShouldSaveState(ThisPageNumber, previousPsfPage.Section.PageNumber))
                {
                    var previousFilterSection = autoMapper.Map<PreSearchFilterSection>(previousPsfPage.Section);
                    preSearchFilterStateManager.SaveState(previousFilterSection);
                }
            }

            timeMessage += $"Stage2: {(DateTime.Now - startTime).TotalSeconds} | ";

            var currentPageFilter = GetCurrentPageFilter();

            timeMessage += $"Stage3: {(DateTime.Now - startTime).TotalSeconds} | ";

            if (!UsePageProfileCount)
            {
                currentPageFilter.NumberOfMatches = 123;
            }
            else
            {
                if (ThisPageNumber > 1)
                {
                    currentPageFilter.NumberOfMatches = asyncHelper.Synchronise(() => GetNumberOfMatches(currentPageFilter));
                }
            }

            timeMessage += $"Stage4: {(DateTime.Now - startTime).TotalSeconds} | ";

            currentPageFilter.Section.SelectMessage = "<b>" + timeMessage + "</b>";

            return View(currentPageFilter);
        }

        private void CheckForBackState(PsfModel model)
        {
            //if we have gone backwards, set the model up for the page
            if (model?.Back?.OptionsSelected != null)
            {
                model.Section = new PsfSection()
                {
                    Name = SectionTitle,
                    SectionDataType = FilterType.ToString(),
                    PageNumber = ThisPageNumber,
                    SingleSelectOnly = SingleSelectOnly,
                };

                model.OptionsSelected = model.Back.OptionsSelected;
            }
        }

        private void SetDefaultForCovidJobProfiles(PsfModel currentPageFilter, bool doesNotHaveSavedState)
        {
            //Only do this on the Training routes page (Which is been used for Covid affected filter)
            //Only do this the first time the page is loaded
            if (FilterType == PreSearchFilterType.TrainingRoute && doesNotHaveSavedState)
            {
                currentPageFilter.Section.Options.Where(s => s.Name == "No").FirstOrDefault().IsSelected = true;
                currentPageFilter.Section.SingleSelectedValue = "No";
            }
        }

        private async Task<int> GetNumberOfMatches(PsfModel model)
        {
            var fieldDefinitions = buildSearchFilterService.GetIndexFieldDefinitions(IndexFieldOperators);
            preSearchFilterStateManager.RestoreState(model.OptionsSelected);
            var filterState = preSearchFilterStateManager.GetPreSearchFilterState();
            model.Sections = autoMapper.Map<List<PsfSection>>(filterState.Sections);

            var resultsModel = autoMapper.Map<PreSearchFiltersResultsModel>(model);
            var properties = new SearchProperties
            {
                Page = 0,
                Count = 0,
                FilterBy = buildSearchFilterService.BuildPreSearchFilters(resultsModel, fieldDefinitions.ToDictionary(k => k.Key, v => v.Value))
            };
            var results = await searchQueryService.SearchAsync("*", properties);

            return (int)results.Count;
        }

        private PsfModel GetCurrentPageFilter()
        {
            var savedSection = preSearchFilterStateManager.GetSavedSection(SectionTitle, FilterType);
            var restoredSection = preSearchFilterStateManager.RestoreOptions(savedSection, UseDummySiteFinity ? GetDummyFilterOptions() : GetFilterOptions());
            var groupedSections = restoredSection.Options.GroupBy(o => o.PSFCategory).OrderBy(g => g.Key);

            //create the section for this page
            var currentSection = autoMapper.Map<PsfSection>(restoredSection);

            var filterSection = currentSection ?? new PsfSection();
            filterSection.Name = SectionTitle;
            filterSection.Description = SectionDescription;
            filterSection.SingleSelectOnly = SingleSelectOnly;
            filterSection.NextPageUrl = NextPageUrl;
            filterSection.PreviousPageUrl = PreviousPageUrl;
            filterSection.PageNumber = ThisPageNumber;
            filterSection.EnableAccordion = EnableAccordion;
            filterSection.GroupFieldsBy = GroupFieldsBy;
            filterSection.TotalNumberOfPages = TotalNumberOfPages;
            filterSection.SectionDataType = FilterType.ToString();
            filterSection.SelectMessage = SelectMessage;
            var thisPageModel = new PsfModel
            {
                //Throw the state out again
                OptionsSelected = preSearchFilterStateManager.GetStateJson(),
                Section = filterSection,
                GroupedOptions = groupedSections,
                NumberOfMatchesMessage = NumberOfMatchesMessage,
            };

            //Need to do this to force the model we have changed to refresh
            ModelState.Clear();

            SetDefaultForCovidJobProfiles(thisPageModel, savedSection == null);

            return thisPageModel;
        }

        #endregion Actions

        #region Private methods
        private IEnumerable<PreSearchFilter> GetDummyFilterOptions()
        {
            for (int ii = 0; ii < 5; ii++)
            {
                yield return new PreSearchFilter
                {
                    Id = new Guid("e99079a2-a201-4b45-bc81-85e807dbcb5a"),
                    Description = $"Description {ii}",
                    Title = $"Option {ii}",
                    Order = ii,
                    UrlName = $"URL-{ii}",
                    NotApplicable = false,
                };
            }
        }

    private IEnumerable<PreSearchFilter> GetFilterOptions()
        {
            switch (FilterType)
            {
                case PreSearchFilterType.Enabler:
                    {
                        return preSearchFiltersFactory.GetRepository<PsfEnabler>().GetAllFilters().OrderBy(o => o.Order);
                    }

                case PreSearchFilterType.EntryQualification:
                    {
                        return preSearchFiltersFactory.GetRepository<PsfEntryQualification>().GetAllFilters().OrderBy(o => o.Order);
                    }

                case PreSearchFilterType.Interest:
                    {
                        return preSearchFiltersFactory.GetRepository<PsfInterest>().GetAllFilters().OrderBy(o => o.Order);
                    }

                case PreSearchFilterType.TrainingRoute:
                    {
                        return preSearchFiltersFactory.GetRepository<PsfTrainingRoute>().GetAllFilters().OrderBy(o => o.Order);
                    }

                case PreSearchFilterType.JobArea:
                    {
                        return preSearchFiltersFactory.GetRepository<PsfJobArea>().GetAllFilters().OrderBy(o => o.Order);
                    }

                case PreSearchFilterType.CareerFocus:
                    {
                        return preSearchFiltersFactory.GetRepository<PsfCareerFocus>().GetAllFilters().OrderBy(o => o.Order);
                    }

                case PreSearchFilterType.PreferredTaskType:
                    {
                        return preSearchFiltersFactory.GetRepository<PsfPreferredTaskType>().GetAllFilters().OrderBy(o => o.Order);
                    }

                case PreSearchFilterType.Skill:
                    {
                        return preSearchFiltersFactory.GetRepository<PsfOnetSkill>().GetAllFilters().OrderBy(o => o.Order).ThenBy(o => o.Title);
                    }

                case PreSearchFilterType.JobProfileCategoryUrl:
                    {
                        return GetJobProfileCategories().OrderBy(o => o.Title);
                    }

                default:
                    {
                        return Enumerable.Empty<PreSearchFilter>();
                    }
            }
        }

        private IQueryable<PsfCategory> GetJobProfileCategories()
        {
            //return categories
            return taxonomyRepository.GetMany(category => category.Taxonomy.Name == "job-profile-categories").Select(category => new PsfCategory
            {
                Title = category.Title,
                Description = category.Description,
                UrlName = category.UrlName
            });
        }

        #endregion Private methods
    }
}