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

            /*
            var dumpOptions = string.Empty;
            var options = GetFilterOptions();
            foreach (PreSearchFilter f in options)
            {
                var cat = string.Empty;
                if (!f.PSFCategory.IsNullOrEmpty())
                {
                    cat = f.PSFCategory;
                }

                dumpOptions += $"<p>yield return new PreSearchFilter{{Title = \"{f.Title}\", Description = \"{f.Description}\", UrlName = \"{f.UrlName}\",  Order = {f.Order},  NotApplicable = {f.NotApplicable.ToString().ToLower()}, PSFCategory = \"{cat}\" }};</p>";
            }
            */

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

            // filterSection.Description = dumpOptions;
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

        private IEnumerable<PreSearchFilter> GetDummyFilterOptions()
        {
            switch (FilterType)
            {
                case PreSearchFilterType.Enabler:
                    {
                        return GetRestrictions();
                    }

                case PreSearchFilterType.EntryQualification:
                    {
                        return GetEntryQualification();
                    }

                case PreSearchFilterType.Interest:
                    {
                        return Enumerable.Empty<PreSearchFilter>();
                    }

                case PreSearchFilterType.TrainingRoute:
                    {
                        return GetCovid19();
                    }

                case PreSearchFilterType.JobArea:
                    {
                        return Enumerable.Empty<PreSearchFilter>();
                    }

                case PreSearchFilterType.CareerFocus:
                    {
                        return Enumerable.Empty<PreSearchFilter>();
                    }

                case PreSearchFilterType.PreferredTaskType:
                    {
                        return Enumerable.Empty<PreSearchFilter>();
                    }

                case PreSearchFilterType.Skill:
                    {
                        return GetSkills();
                    }

                case PreSearchFilterType.JobProfileCategoryUrl:
                    {
                        return GetCategories();
                    }

                default:
                    {
                        return Enumerable.Empty<PreSearchFilter>();
                    }
            }
        }

        private IEnumerable<PreSearchFilter> GetEntryQualification()
        {
            yield return new PreSearchFilter { Title = "No formal qualifications", Description = "Dummy Work experience and skills learnt on the job", UrlName = "none", Order = 1, NotApplicable = false, PSFCategory = string.Empty };

            yield return new PreSearchFilter { Title = "Up to Level 2", Description = "Achieved up to GCSEs, BTECs or equivalent", UrlName = "job-level-2", Order = 2, NotApplicable = false, PSFCategory = string.Empty };

            yield return new PreSearchFilter { Title = "Levels 3 to 4", Description = "Achieved A Levels or equivalent", UrlName = "levels-3-to-4", Order = 3, NotApplicable = false, PSFCategory = string.Empty };

            yield return new PreSearchFilter { Title = "Levels 5 to 6", Description = "Achieved an undergraduate degree or equivalent", UrlName = "levels-5-to-6", Order = 4, NotApplicable = false, PSFCategory = string.Empty };

            yield return new PreSearchFilter { Title = "Levels 7 to 8", Description = "Achieved a postgraduate degree, professional qualification or equivalent", UrlName = "levels-7-to-8", Order = 5, NotApplicable = false, PSFCategory = string.Empty };
        }

        private IEnumerable<PreSearchFilter> GetSkills()
        {
            yield return new PreSearchFilter { Title = "Active Learning Dummy", Description = " ", UrlName = "active-learning", Order = 0, NotApplicable = false, PSFCategory = "Skills" };

            yield return new PreSearchFilter { Title = "Active Listening", Description = " ", UrlName = "active-listening", Order = 0, NotApplicable = false, PSFCategory = "Skills" };

            yield return new PreSearchFilter { Title = "Admin", Description = " ", UrlName = "clerical", Order = 0, NotApplicable = false, PSFCategory = "Knowledge" };

            yield return new PreSearchFilter { Title = "Administration and Management", Description = " ", UrlName = "administration-and-management", Order = 0, NotApplicable = false, PSFCategory = "Knowledge" };

            yield return new PreSearchFilter { Title = "Ambition", Description = " ", UrlName = "achievement-effort", Order = 0, NotApplicable = false, PSFCategory = "Work style" };

            yield return new PreSearchFilter { Title = "Analytical Thinking", Description = " ", UrlName = "analytical-thinking", Order = 0, NotApplicable = false, PSFCategory = "Work style" };

            yield return new PreSearchFilter { Title = "Attention to Detail", Description = " ", UrlName = "attention-to-detail", Order = 0, NotApplicable = false, PSFCategory = "Work style" };

            yield return new PreSearchFilter { Title = "Biology", Description = " ", UrlName = "biology", Order = 0, NotApplicable = false, PSFCategory = "Knowledge" };

            yield return new PreSearchFilter { Title = "Building and construction", Description = " ", UrlName = "building-and-construction", Order = 0, NotApplicable = false, PSFCategory = "Knowledge" };

            yield return new PreSearchFilter { Title = "Chemistry", Description = " ", UrlName = "chemistry", Order = 0, NotApplicable = false, PSFCategory = "Knowledge" };

            yield return new PreSearchFilter { Title = "Communication", Description = " ", UrlName = "communications", Order = 0, NotApplicable = false, PSFCategory = "Knowledge" };

            yield return new PreSearchFilter { Title = "Computers and I.T.", Description = " ", UrlName = "computers-and-electronics", Order = 0, NotApplicable = false, PSFCategory = "Knowledge" };

            yield return new PreSearchFilter { Title = "Concentration", Description = " ", UrlName = "attentiveness", Order = 0, NotApplicable = false, PSFCategory = "Ability" };

            yield return new PreSearchFilter { Title = "Control Movement", Description = " ", UrlName = "control-movement-abilities", Order = 0, NotApplicable = false, PSFCategory = "Ability" };

            yield return new PreSearchFilter { Title = "Critical Thinking", Description = " ", UrlName = "critical-thinking", Order = 0, NotApplicable = false, PSFCategory = "Skills" };

            yield return new PreSearchFilter { Title = "customer service skills", Description = " ", UrlName = "customer-and-personal-service", Order = 0, NotApplicable = false, PSFCategory = "Knowledge" };

            yield return new PreSearchFilter { Title = "Decision making", Description = " ", UrlName = "judgment-and-decision-making", Order = 0, NotApplicable = false, PSFCategory = "Skills" };

            yield return new PreSearchFilter { Title = "Design", Description = " ", UrlName = "design", Order = 0, NotApplicable = false, PSFCategory = "Knowledge" };

            yield return new PreSearchFilter { Title = "Economics and Accounting", Description = " ", UrlName = "economics-and-accounting", Order = 0, NotApplicable = false, PSFCategory = "Knowledge" };

            yield return new PreSearchFilter { Title = "Education and training", Description = " ", UrlName = "education-and-training", Order = 0, NotApplicable = false, PSFCategory = "Knowledge" };

            yield return new PreSearchFilter { Title = "Effective communication", Description = " ", UrlName = "speaking", Order = 0, NotApplicable = false, PSFCategory = "Skills" };

            yield return new PreSearchFilter { Title = "Empathy", Description = " ", UrlName = "social-perceptiveness", Order = 0, NotApplicable = false, PSFCategory = "Skills" };

            yield return new PreSearchFilter { Title = "Empathy", Description = " ", UrlName = "concern-for-others", Order = 0, NotApplicable = false, PSFCategory = "Work style" };

            yield return new PreSearchFilter { Title = "Endurance", Description = " ", UrlName = "endurance", Order = 0, NotApplicable = false, PSFCategory = "Ability" };

            yield return new PreSearchFilter { Title = "Engineering and Technology", Description = " ", UrlName = "engineering-and-technology", Order = 0, NotApplicable = false, PSFCategory = "Knowledge" };

            yield return new PreSearchFilter { Title = "English Language", Description = " ", UrlName = "english-language", Order = 0, NotApplicable = false, PSFCategory = "Knowledge" };

            yield return new PreSearchFilter { Title = "Enjoy working with others", Description = " ", UrlName = "social-orientation", Order = 0, NotApplicable = false, PSFCategory = "Work style" };

            yield return new PreSearchFilter { Title = "Equipment Maintenance", Description = " ", UrlName = "equipment-maintenance", Order = 0, NotApplicable = false, PSFCategory = "Skills" };

            yield return new PreSearchFilter { Title = "Equipment Selection", Description = " ", UrlName = "equipment-selection", Order = 0, NotApplicable = false, PSFCategory = "Skills" };

            yield return new PreSearchFilter { Title = "Fine Arts", Description = " ", UrlName = "fine-arts", Order = 0, NotApplicable = false, PSFCategory = "Knowledge" };

            yield return new PreSearchFilter { Title = "Food Production", Description = " ", UrlName = "food-production", Order = 0, NotApplicable = false, PSFCategory = "Knowledge" };

            yield return new PreSearchFilter { Title = "Foreign Language", Description = " ", UrlName = "foreign-language", Order = 0, NotApplicable = false, PSFCategory = "Knowledge" };

            yield return new PreSearchFilter { Title = "Geography", Description = " ", UrlName = "geography", Order = 0, NotApplicable = false, PSFCategory = "Knowledge" };

            yield return new PreSearchFilter { Title = "Helping people", Description = " ", UrlName = "service-orientation", Order = 0, NotApplicable = false, PSFCategory = "Skills" };

            yield return new PreSearchFilter { Title = "History and Archeology", Description = " ", UrlName = "history-and-archeology", Order = 0, NotApplicable = false, PSFCategory = "Knowledge" };

            yield return new PreSearchFilter { Title = "Independence", Description = " ", UrlName = "independence", Order = 0, NotApplicable = false, PSFCategory = "Work style" };

            yield return new PreSearchFilter { Title = "Influence and persuasion", Description = " ", UrlName = "persuasion-negotiation", Order = 0, NotApplicable = false, PSFCategory = "Skills" };

            yield return new PreSearchFilter { Title = "Initiative", Description = " ", UrlName = "initiative", Order = 0, NotApplicable = false, PSFCategory = "Work style" };

            yield return new PreSearchFilter { Title = "Innovation", Description = " ", UrlName = "innovation", Order = 0, NotApplicable = false, PSFCategory = "Work style" };

            yield return new PreSearchFilter { Title = "Installation", Description = " ", UrlName = "installation", Order = 0, NotApplicable = false, PSFCategory = "Skills" };

            yield return new PreSearchFilter { Title = "Instructing", Description = " ", UrlName = "instructing", Order = 0, NotApplicable = false, PSFCategory = "Skills" };

            yield return new PreSearchFilter { Title = "Law and government", Description = " ", UrlName = "law-and-government", Order = 0, NotApplicable = false, PSFCategory = "Knowledge" };

            yield return new PreSearchFilter { Title = "Leadership", Description = " ", UrlName = "leadership", Order = 0, NotApplicable = false, PSFCategory = "Work style" };

            yield return new PreSearchFilter { Title = "Leadership and organisation", Description = " ", UrlName = "leadership-coordination-", Order = 0, NotApplicable = false, PSFCategory = "Skills" };

            yield return new PreSearchFilter { Title = "Learning strategy", Description = " ", UrlName = "learning-strategies", Order = 0, NotApplicable = false, PSFCategory = "Skills" };

            yield return new PreSearchFilter { Title = "Manage finances", Description = " ", UrlName = "management-of-financial-resources", Order = 0, NotApplicable = false, PSFCategory = "Skills" };

            yield return new PreSearchFilter { Title = "Manage staff", Description = " ", UrlName = "management-of-personnel-resources", Order = 0, NotApplicable = false, PSFCategory = "Skills" };

            yield return new PreSearchFilter { Title = "Maths", Description = " ", UrlName = "mathematics", Order = 0, NotApplicable = false, PSFCategory = "Knowledge" };

            yield return new PreSearchFilter { Title = "Maths knowledge", Description = " ", UrlName = "quantitative-abilities-mathematics-knowledge", Order = 0, NotApplicable = false, PSFCategory = "Knowledge" };

            yield return new PreSearchFilter { Title = "Maths problem solving", Description = " ", UrlName = "quantitative-abilities-mathematics-skills", Order = 0, NotApplicable = false, PSFCategory = "Skills" };

            yield return new PreSearchFilter { Title = "Mechanical", Description = " ", UrlName = "mechanical", Order = 0, NotApplicable = false, PSFCategory = "Knowledge" };

            yield return new PreSearchFilter { Title = "Media production", Description = " ", UrlName = "communications-and-media", Order = 0, NotApplicable = false, PSFCategory = "Knowledge" };

            yield return new PreSearchFilter { Title = "Medicine and Dentistry", Description = " ", UrlName = "medicine-and-dentistry", Order = 0, NotApplicable = false, PSFCategory = "Knowledge" };

            yield return new PreSearchFilter { Title = "Memory", Description = " ", UrlName = "memory", Order = 0, NotApplicable = false, PSFCategory = "Ability" };

            yield return new PreSearchFilter { Title = "Monitoring equipment", Description = " ", UrlName = "operation-monitoring", Order = 0, NotApplicable = false, PSFCategory = "Skills" };

            yield return new PreSearchFilter { Title = "Monitoring performance", Description = " ", UrlName = "monitoring", Order = 0, NotApplicable = false, PSFCategory = "Skills" };

            yield return new PreSearchFilter { Title = "Negotiation", Description = " ", UrlName = "negotiation", Order = 0, NotApplicable = false, PSFCategory = "Skills" };

            yield return new PreSearchFilter { Title = "Number skills", Description = " ", UrlName = "quantitative-abilities", Order = 0, NotApplicable = false, PSFCategory = "Ability" };

            yield return new PreSearchFilter { Title = "Open to change", Description = " ", UrlName = "adaptability-flexibility", Order = 0, NotApplicable = false, PSFCategory = "Work style" };

            yield return new PreSearchFilter { Title = "Operating equipment", Description = " ", UrlName = "operation-and-control", Order = 0, NotApplicable = false, PSFCategory = "Skills" };

            yield return new PreSearchFilter { Title = "Operations analysis", Description = " ", UrlName = "operations-analysis", Order = 0, NotApplicable = false, PSFCategory = "Skills" };

            yield return new PreSearchFilter { Title = "Persistence", Description = " ", UrlName = "persistence", Order = 0, NotApplicable = false, PSFCategory = "Work style" };

            yield return new PreSearchFilter { Title = "Personnel and Human Resources", Description = " ", UrlName = "personnel-and-human-resources", Order = 0, NotApplicable = false, PSFCategory = "Knowledge" };

            yield return new PreSearchFilter { Title = "Persuasion", Description = " ", UrlName = "persuasion", Order = 0, NotApplicable = false, PSFCategory = "Skills" };

            yield return new PreSearchFilter { Title = "Philosophy and Religion", Description = " ", UrlName = "philosophy-and-theology", Order = 0, NotApplicable = false, PSFCategory = "Knowledge" };

            yield return new PreSearchFilter { Title = "Physics", Description = " ", UrlName = "physics", Order = 0, NotApplicable = false, PSFCategory = "Knowledge" };

            yield return new PreSearchFilter { Title = "Problem solving", Description = " ", UrlName = "troubleshooting", Order = 0, NotApplicable = false, PSFCategory = "Skills" };

            yield return new PreSearchFilter { Title = "Production and Processing", Description = " ", UrlName = "production-and-processing", Order = 0, NotApplicable = false, PSFCategory = "Knowledge" };

            yield return new PreSearchFilter { Title = "Programming", Description = " ", UrlName = "programming", Order = 0, NotApplicable = false, PSFCategory = "Skills" };

            yield return new PreSearchFilter { Title = "Psychology", Description = " ", UrlName = "psychology", Order = 0, NotApplicable = false, PSFCategory = "Knowledge" };

            yield return new PreSearchFilter { Title = "Public Safety and Security", Description = " ", UrlName = "public-safety-and-security", Order = 0, NotApplicable = false, PSFCategory = "Knowledge" };

            yield return new PreSearchFilter { Title = "Quality control", Description = " ", UrlName = "quality-control-analysis", Order = 0, NotApplicable = false, PSFCategory = "Skills" };

            yield return new PreSearchFilter { Title = "Reading", Description = " ", UrlName = "reading-comprehension", Order = 0, NotApplicable = false, PSFCategory = "Skills" };

            yield return new PreSearchFilter { Title = "Repairing machines", Description = " ", UrlName = "repairing", Order = 0, NotApplicable = false, PSFCategory = "Skills" };

            yield return new PreSearchFilter { Title = "Sales and Marketing", Description = " ", UrlName = "sales-and-marketing", Order = 0, NotApplicable = false, PSFCategory = "Knowledge" };

            yield return new PreSearchFilter { Title = "Science", Description = " ", UrlName = "science", Order = 0, NotApplicable = false, PSFCategory = "Skills" };

            yield return new PreSearchFilter { Title = "Self control", Description = " ", UrlName = "self-control", Order = 0, NotApplicable = false, PSFCategory = "Work style" };

            yield return new PreSearchFilter { Title = "Self-reliant working", Description = " ", UrlName = "independence-initiative", Order = 0, NotApplicable = false, PSFCategory = "Work style" };

            yield return new PreSearchFilter { Title = "Sociology and Anthropology", Description = " ", UrlName = "sociology-and-anthropology", Order = 0, NotApplicable = false, PSFCategory = "Knowledge" };

            yield return new PreSearchFilter { Title = "Speaking", Description = " ", UrlName = "speaking-verbal-abilities", Order = 0, NotApplicable = false, PSFCategory = "Skills" };

            yield return new PreSearchFilter { Title = "Systems analysis", Description = " ", UrlName = "systems-analysis", Order = 0, NotApplicable = false, PSFCategory = "Skills" };

            yield return new PreSearchFilter { Title = "Systems development", Description = " ", UrlName = "systems-analysis-systems-evaluation", Order = 0, NotApplicable = false, PSFCategory = "Skills" };

            yield return new PreSearchFilter { Title = "Systems evaluation", Description = " ", UrlName = "systems-evaluation", Order = 0, NotApplicable = false, PSFCategory = "Skills" };

            yield return new PreSearchFilter { Title = "Teamwork", Description = " ", UrlName = "coordination", Order = 0, NotApplicable = false, PSFCategory = "Skills" };

            yield return new PreSearchFilter { Title = "Telecommunications", Description = " ", UrlName = "telecommunications", Order = 0, NotApplicable = false, PSFCategory = "Knowledge" };

            yield return new PreSearchFilter { Title = "Therapy and counseling", Description = " ", UrlName = "therapy-and-counseling", Order = 0, NotApplicable = false, PSFCategory = "Knowledge" };

            yield return new PreSearchFilter { Title = "Thinking and reasoning", Description = " ", UrlName = "idea-generation-and-reasoning-abilities", Order = 0, NotApplicable = false, PSFCategory = "Ability" };

            yield return new PreSearchFilter { Title = "Time management", Description = " ", UrlName = "time-management", Order = 0, NotApplicable = false, PSFCategory = "Skills" };

            yield return new PreSearchFilter { Title = "Transport", Description = " ", UrlName = "transportation", Order = 0, NotApplicable = false, PSFCategory = "Knowledge" };

            yield return new PreSearchFilter { Title = "Verbal reasoning", Description = " ", UrlName = "verbal-abilities", Order = 0, NotApplicable = false, PSFCategory = "Ability" };

            yield return new PreSearchFilter { Title = "Work well with your hands", Description = " ", UrlName = "fine-manipulative-abilities", Order = 0, NotApplicable = false, PSFCategory = "Ability" };

            yield return new PreSearchFilter { Title = "Working under pressure", Description = " ", UrlName = "stress-tolerance", Order = 0, NotApplicable = false, PSFCategory = "Work style" };

            yield return new PreSearchFilter { Title = "Working with others", Description = " ", UrlName = "cooperation", Order = 0, NotApplicable = false, PSFCategory = "Work style" };

            yield return new PreSearchFilter { Title = "Writing", Description = " ", UrlName = "writing", Order = 0, NotApplicable = false, PSFCategory = "Skills" };
        }

        private IEnumerable<PreSearchFilter> GetCategories()
        {
            yield return new PreSearchFilter { Title = "Administration Dummy", Description = string.Empty, UrlName = "administration", PSFCategory = string.Empty };

            yield return new PreSearchFilter { Title = "Animal care", Description = string.Empty, UrlName = "animal-care", PSFCategory = string.Empty };

            yield return new PreSearchFilter { Title = "Beauty and wellbeing", Description = string.Empty, UrlName = "beauty-and-wellbeing", PSFCategory = string.Empty };

            yield return new PreSearchFilter { Title = "Business and finance", Description = string.Empty, UrlName = "business-and-finance", PSFCategory = string.Empty };

            yield return new PreSearchFilter { Title = "Computing, technology and digital", Description = string.Empty, UrlName = "computing-technology-and-digital", PSFCategory = string.Empty };

            yield return new PreSearchFilter { Title = "Construction and trades", Description = string.Empty, UrlName = "construction-and-trades", PSFCategory = string.Empty };

            yield return new PreSearchFilter { Title = "Creative and media", Description = string.Empty, UrlName = "creative-and-media", PSFCategory = string.Empty };

            yield return new PreSearchFilter { Title = "Delivery and storage", Description = string.Empty, UrlName = "delivery-and-storage", PSFCategory = string.Empty };

            yield return new PreSearchFilter { Title = "Emergency and uniform services", Description = string.Empty, UrlName = "emergency-and-uniform-services", PSFCategory = string.Empty };

            yield return new PreSearchFilter { Title = "Engineering and maintenance", Description = string.Empty, UrlName = "engineering-and-maintenance", PSFCategory = string.Empty };

            yield return new PreSearchFilter { Title = "Environment and land", Description = string.Empty, UrlName = "environment-and-land", PSFCategory = string.Empty };

            yield return new PreSearchFilter { Title = "Government services", Description = string.Empty, UrlName = "government-services", PSFCategory = string.Empty };

            yield return new PreSearchFilter { Title = "Healthcare", Description = string.Empty, UrlName = "healthcare", PSFCategory = string.Empty };

            yield return new PreSearchFilter { Title = "Home services", Description = string.Empty, UrlName = "home-services", PSFCategory = string.Empty };

            yield return new PreSearchFilter { Title = "Hospitality and food", Description = string.Empty, UrlName = "hospitality-and-food", PSFCategory = string.Empty };

            yield return new PreSearchFilter { Title = "Law and legal", Description = string.Empty, UrlName = "law-and-legal", PSFCategory = string.Empty };

            yield return new PreSearchFilter { Title = "Managerial", Description = string.Empty, UrlName = "managerial", PSFCategory = string.Empty };

            yield return new PreSearchFilter { Title = "Manufacturing", Description = string.Empty, UrlName = "manufacturing", PSFCategory = string.Empty };

            yield return new PreSearchFilter { Title = "Retail and sales", Description = string.Empty, UrlName = "retail-and-sales", PSFCategory = string.Empty };

            yield return new PreSearchFilter { Title = "Science and research", Description = string.Empty, UrlName = "science-and-research", PSFCategory = string.Empty };

            yield return new PreSearchFilter { Title = "Social care", Description = string.Empty, UrlName = "social-care", PSFCategory = string.Empty };

            yield return new PreSearchFilter { Title = "Sports and leisure", Description = string.Empty, UrlName = "sports-and-leisure", PSFCategory = string.Empty };

            yield return new PreSearchFilter { Title = "Teaching and education", Description = string.Empty, UrlName = "teaching-and-education", PSFCategory = string.Empty };

            yield return new PreSearchFilter { Title = "Transport", Description = string.Empty, UrlName = "transport", PSFCategory = string.Empty };

            yield return new PreSearchFilter { Title = "Travel and tourism", Description = string.Empty, UrlName = "travel-and-tourism", PSFCategory = string.Empty };
        }

        private IEnumerable<PreSearchFilter> GetRestrictions()
        {
            yield return new PreSearchFilter { Title = "jobs that need an enhanced background check Dummy", Description = string.Empty, UrlName = "i-do-not-want-to-have-a-dbs-check", Order = 0, NotApplicable = false, PSFCategory = string.Empty };

            yield return new PreSearchFilter { Title = "jobs that need digital skills", Description = string.Empty, UrlName = "low-digital-skills", Order = 1, NotApplicable = false, PSFCategory = string.Empty };

            yield return new PreSearchFilter { Title = "emotionally demanding jobs", Description = string.Empty, UrlName = "non-emotionally-demanding", Order = 2, NotApplicable = false, PSFCategory = string.Empty };

            yield return new PreSearchFilter { Title = "physically demanding jobs", Description = string.Empty, UrlName = "no-physically-demanding-work", Order = 3, NotApplicable = false, PSFCategory = string.Empty };

            yield return new PreSearchFilter { Title = "jobs that involve weekend working", Description = string.Empty, UrlName = "no-weekends", Order = 4, NotApplicable = false, PSFCategory = string.Empty };

            yield return new PreSearchFilter { Title = "jobs that need a driving licence", Description = string.Empty, UrlName = "no-driving-licence", Order = 5, NotApplicable = false, PSFCategory = string.Empty };
        }

        private IEnumerable<PreSearchFilter> GetCovid19()
        {
            yield return new PreSearchFilter { Title = "Yes", Description = string.Empty, UrlName = "yes", Order = 1, NotApplicable = true, PSFCategory = string.Empty };

            yield return new PreSearchFilter { Title = "No", Description = string.Empty, UrlName = "no", Order = 2, NotApplicable = false, PSFCategory = string.Empty };
        }

        #endregion Private methods
    }
}