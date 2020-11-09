using AutoMapper;
using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Repository.SitefinityCMS;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Controllers;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models;
using FakeItEasy;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Telerik.Sitefinity.Taxonomies.Model;
using TestStack.FluentMVCTesting;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.UnitTests
{
    public class PreSearchFiltersControllerTests
    {
        private const int NumberDummyFilterOptions = 5;

        private IPreSearchFiltersFactory psfRepositoryFactoryFake;
        private IApplicationLogger loggerFake;
        private IPreSearchFiltersRepository<PsfInterest> psfFakeIntrestRepository;
        private IPreSearchFiltersRepository<PsfEnabler> psfFakeEnablerRepository;
        private IPreSearchFiltersRepository<PsfEntryQualification> psfFakeQalificationsRepository;
        private IPreSearchFiltersRepository<PsfTrainingRoute> psfFakeTrainingRepository;
        private IPreSearchFiltersRepository<PsfJobArea> psfFakeJobAreaRepository;
        private IPreSearchFiltersRepository<PsfCareerFocus> psfFakeCareerFocusRepository;
        private IPreSearchFiltersRepository<PsfPreferredTaskType> psfFakePreferredTaskTypeRepository;
        private IPreSearchFilterStateManager fakePsfStateManager;
        private ISearchQueryService<JobProfileIndex> fakeSearchQueryService;
        private IBuildSearchFilterService fakeBuildSearchFilterService;
        private IAsyncHelper fakeAsyncHelper;
        private ITaxonomyRepository fakeTaxonomyRepository;

       [Fact]
        public void IndexNoModelTest()
        {
            //Setup the fakes and dummies for test
            SetUpFakesAndCalls();
            SetUpStateMangerFakesAndCalls(PreSearchFilterType.Interest, false);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<JobProfilesAutoMapperProfile>();
            });
            var mapper = config.CreateMapper();

            //Instantiate & Act
            var preSearchFiltersController =
                new PreSearchFiltersController(loggerFake, mapper, psfRepositoryFactoryFake, fakePsfStateManager, fakeSearchQueryService, fakeBuildSearchFilterService, fakeAsyncHelper, fakeTaxonomyRepository)
                {
                    FilterType = PreSearchFilterType.Interest
                };

            //Act on the index
            var indexResult = preSearchFiltersController.WithCallTo(c => c.Index());

            PsfModel firstVm = null;

            //Assert
            indexResult.ShouldRenderDefaultView().WithModel<PsfModel>(vm =>
            {
                CheckFilterSecton(preSearchFiltersController, vm.Section, PreSearchFilterType.Interest);
                firstVm = vm;
            }).AndNoModelErrors();

            A.CallTo(() => psfFakeIntrestRepository.GetAllFilters()).MustHaveHappened();

            A.CallTo(() => fakePsfStateManager.RestoreState(A<string>._)).MustNotHaveHappened();
            A.CallTo(() => fakePsfStateManager.ShouldSaveState(A<int>._, A<int>._)).MustNotHaveHappened();
            A.CallTo(() => fakePsfStateManager.SaveState(A<PreSearchFilterSection>._)).MustNotHaveHappened();

            A.CallTo(() => fakePsfStateManager.GetSavedSection(A<string>._, A<PreSearchFilterType>._)).MustHaveHappened();
            A.CallTo(() => fakePsfStateManager.RestoreOptions(A<PreSearchFilterSection>._, A<IEnumerable<PreSearchFilter>>._)).MustHaveHappened();
            A.CallTo(() => fakePsfStateManager.GetStateJson()).MustHaveHappened();
        }

        [Fact]
        public void IndexWithPsfResultsTest()
        {
            //Setup the fakes and dummies for test
            SetUpFakesAndCalls();
            SetUpStateMangerFakesAndCalls(PreSearchFilterType.Interest, true);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<JobProfilesAutoMapperProfile>();
            });
            var mapper = config.CreateMapper();

            //Instantiate & Act
            var preSearchFiltersController =
                new PreSearchFiltersController(loggerFake, mapper, psfRepositoryFactoryFake, fakePsfStateManager, fakeSearchQueryService, fakeBuildSearchFilterService, fakeAsyncHelper, fakeTaxonomyRepository)
                {
                    FilterType = PreSearchFilterType.Interest
                };

            //Act on the index
            var firstVm = new PsfModel();
            var resultsViewModel = new PsfSearchResultsViewModel
            {
                PreSearchFiltersModel = GeneratePreSEarchFiltersViewModel(PreSearchFilterType.Interest)
            };

            var postFromResultsPageCall = preSearchFiltersController.WithCallTo(c => c.Index(firstVm, resultsViewModel));
            postFromResultsPageCall.ShouldRenderDefaultView().WithModel<PsfModel>(vm =>
            {
                vm.Section.Should().NotBeNull();
            })
            .AndNoModelErrors();

            A.CallTo(() => psfFakeIntrestRepository.GetAllFilters()).MustHaveHappened();
            A.CallTo(() => psfFakeIntrestRepository.GetAllFilters()).MustHaveHappened();

            A.CallTo(() => fakePsfStateManager.RestoreState(A<string>._)).MustHaveHappened();
            A.CallTo(() => fakePsfStateManager.ShouldSaveState(A<int>._, A<int>._)).MustHaveHappened();
            A.CallTo(() => fakePsfStateManager.SaveState(A<PreSearchFilterSection>._)).MustHaveHappened();
            A.CallTo(() => fakePsfStateManager.GetSavedSection(A<string>._, A<PreSearchFilterType>._)).MustHaveHappened();
            A.CallTo(() => fakePsfStateManager.RestoreOptions(A<PreSearchFilterSection>._, A<IEnumerable<PreSearchFilter>>._)).MustHaveHappened();
            A.CallTo(() => fakePsfStateManager.GetStateJson()).MustHaveHappened();
        }

        [Fact]
        public void IndexWithPsfModelFirstPageTest()
        {
            //Setup the fakes and dummies for test
            SetUpFakesAndCalls();
            SetUpStateMangerFakesAndCalls(PreSearchFilterType.Interest, false);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<JobProfilesAutoMapperProfile>();
            });
            var mapper = config.CreateMapper();

            //Instantiate & Act
            var preSearchFiltersController = new PreSearchFiltersController(loggerFake, mapper, psfRepositoryFactoryFake, fakePsfStateManager, fakeSearchQueryService, fakeBuildSearchFilterService, fakeAsyncHelper, fakeTaxonomyRepository)
            {
                FilterType = PreSearchFilterType.Interest
            };

            //Act on the index
            var firstVm = new PsfModel();
            var postFromResultsPageCall = preSearchFiltersController.WithCallTo(c => c.Index(firstVm, null));
            postFromResultsPageCall.ShouldRenderDefaultView().WithModel<PsfModel>(vm =>
            {
                vm.Section.Should().NotBeNull();
            })
            .AndNoModelErrors();

            A.CallTo(() => psfFakeIntrestRepository.GetAllFilters()).MustHaveHappened();
            A.CallTo(() => psfFakeIntrestRepository.GetAllFilters()).MustHaveHappened();

            A.CallTo(() => fakePsfStateManager.RestoreState(A<string>._)).MustNotHaveHappened();
            A.CallTo(() => fakePsfStateManager.ShouldSaveState(A<int>._, A<int>._)).MustNotHaveHappened();
            A.CallTo(() => fakePsfStateManager.SaveState(A<PreSearchFilterSection>._)).MustNotHaveHappened();
            A.CallTo(() => fakePsfStateManager.GetSavedSection(A<string>._, A<PreSearchFilterType>._)).MustHaveHappened();
            A.CallTo(() => fakePsfStateManager.RestoreOptions(A<PreSearchFilterSection>._, A<IEnumerable<PreSearchFilter>>._)).MustHaveHappened();
            A.CallTo(() => fakePsfStateManager.GetStateJson()).MustHaveHappened();
        }

        [Theory]
        [InlineData(PreSearchFilterType.Interest)]
        [InlineData(PreSearchFilterType.Enabler)]
        [InlineData(PreSearchFilterType.CareerFocus)]
        [InlineData(PreSearchFilterType.EntryQualification)]
        [InlineData(PreSearchFilterType.JobArea)]
        [InlineData(PreSearchFilterType.PreferredTaskType)]
        [InlineData(PreSearchFilterType.TrainingRoute)]
        [InlineData(PreSearchFilterType.JobProfileCategoryUrl)]
        public void IndexRepositoryTest(PreSearchFilterType filterType)
        {
            //Setup the fakes and dummies
            SetUpFakesAndCalls();

            SetUpStateMangerFakesAndCalls(PreSearchFilterType.Interest, false);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<JobProfilesAutoMapperProfile>();
            });
            var mapper = config.CreateMapper();

            //Instantiate & Act
            var preSearchFiltersController = new PreSearchFiltersController(loggerFake, mapper, psfRepositoryFactoryFake, fakePsfStateManager, fakeSearchQueryService, fakeBuildSearchFilterService, fakeAsyncHelper, fakeTaxonomyRepository)
            {
                FilterType = filterType
            };

            //Act on the index
            var indexResult = preSearchFiltersController.WithCallTo(c => c.Index());

            //Assert
            indexResult.ShouldRenderDefaultView().WithModel<PsfModel>(vm =>
            {
                vm.Section.Should().NotBeNull();
                CheckFilterSecton(preSearchFiltersController, vm.Section, filterType);
            }).AndNoModelErrors();
        }

        private PsfModel GeneratePreSEarchFiltersViewModel(PreSearchFilterType filterType)
        {
            var filtersModel = new PsfModel { OptionsSelected = "DummyJsonState" };

            var filterSectionOne = new PsfSection
            {
                Name = "Multi Select Section One",
                Description = "Dummy Title One",
                SingleSelectOnly = false,
                NextPageUrl = "NextSectionURL",
                PreviousPageUrl = "HomePageURL",
                PageNumber = 1,
                TotalNumberOfPages = 2,
                SectionDataType = filterType.ToString()
            };

            filterSectionOne.Options = new List<PsfOption>();

            for (int ii = 0; ii < 3; ii++)
            {
                var iiString = ii.ToString();
                filterSectionOne.Options.Add(item: new PsfOption
                {
                    Id = iiString,
                    IsSelected = false,
                    Name = $"Title-{iiString}",
                    Description = $"Description-{iiString}",
                    OptionKey = $"{iiString}-UrlName",
                    ClearOtherOptionsIfSelected = false
                });
            }

            filtersModel.Section = filterSectionOne;

            return filtersModel;
        }

        private void SetUpStateMangerFakesAndCalls(PreSearchFilterType filterType, bool shouldSaveState, bool addNotApplicable = true)
        {
            var dummyStateSection = new PreSearchFilterSection
            {
                SectionDataType = filterType,
                Options = GetDummyPreSearchFilterOption(addNotApplicable)
            };

            fakePsfStateManager = A.Fake<IPreSearchFilterStateManager>(ops => ops.Strict());
            A.CallTo(() => fakePsfStateManager.GetSavedSection(A<string>._, A<PreSearchFilterType>._)).Returns(dummyStateSection);
            A.CallTo(() => fakePsfStateManager.RestoreOptions(A<PreSearchFilterSection>._, A<IEnumerable<PreSearchFilter>>._)).Returns(dummyStateSection);
            A.CallTo(() => fakePsfStateManager.GetStateJson()).Returns("DummyStateJson");
            A.CallTo(() => fakePsfStateManager.ShouldSaveState(A<int>._, A<int>._)).Returns(shouldSaveState);
            A.CallTo(() => fakePsfStateManager.RestoreState(A<string>._)).DoesNothing();
            A.CallTo(() => fakePsfStateManager.SaveState(A<PreSearchFilterSection>._)).DoesNothing();
        }

        private List<PreSearchFilterOption> GetDummyPreSearchFilterOption(bool addNotApplicable)
        {
            var retList = new List<PreSearchFilterOption>();

            for (int ii = 0; ii < NumberDummyFilterOptions; ii++)
            {
                retList.Add(new PreSearchFilterOption
                {
                    Id = "e99079a2-a201-4b45-bc81-85e807dbcb5a",
                    Description = $"Description {ii}",
                    Name = $"Option {ii}",
                    IsSelected = false,
                    OptionKey = $"DummyOptionKey",
                    ClearOtherOptionsIfSelected = addNotApplicable & (ii == (NumberDummyFilterOptions - 1))
                });
            }

            return retList;
        }

        private void SetUpFakesAndCalls(bool addNotApplicable = true)
        {
            psfRepositoryFactoryFake = A.Fake<IPreSearchFiltersFactory>(ops => ops.Strict());
            loggerFake = A.Fake<IApplicationLogger>();
            A.Fake<IWebAppContext>(ops => ops.Strict());

            psfFakeIntrestRepository = A.Fake<IPreSearchFiltersRepository<PsfInterest>>(ops => ops.Strict());
            psfFakeEnablerRepository = A.Fake<IPreSearchFiltersRepository<PsfEnabler>>(ops => ops.Strict());
            psfFakeQalificationsRepository = A.Fake<IPreSearchFiltersRepository<PsfEntryQualification>>(ops => ops.Strict());
            psfFakeTrainingRepository = A.Fake<IPreSearchFiltersRepository<PsfTrainingRoute>>(ops => ops.Strict());
            psfFakeJobAreaRepository = A.Fake<IPreSearchFiltersRepository<PsfJobArea>>(ops => ops.Strict());
            psfFakeCareerFocusRepository = A.Fake<IPreSearchFiltersRepository<PsfCareerFocus>>(ops => ops.Strict());
            psfFakePreferredTaskTypeRepository = A.Fake<IPreSearchFiltersRepository<PsfPreferredTaskType>>(ops => ops.Strict());
            fakeBuildSearchFilterService = A
                .Fake<IBuildSearchFilterService>(ops => ops.Strict());
            fakeSearchQueryService = A.Fake<ISearchQueryService<JobProfileIndex>>(ops => ops.Strict());
            fakeTaxonomyRepository = A.Fake<ITaxonomyRepository>(ops => ops.Strict());
            fakeAsyncHelper = new AsyncHelper();

            A.Fake<IMapper>(ops => ops.Strict());

            //Set up call
            A.CallTo(() => psfFakeIntrestRepository.GetAllFilters()).Returns(GetTestFilterRepoOptions<PsfInterest>(addNotApplicable));
            A.CallTo(() => psfFakeEnablerRepository.GetAllFilters()).Returns(GetTestFilterRepoOptions<PsfEnabler>(addNotApplicable));
            A.CallTo(() => psfFakeQalificationsRepository.GetAllFilters()).Returns(GetTestFilterRepoOptions<PsfEntryQualification>(addNotApplicable));
            A.CallTo(() => psfFakeTrainingRepository.GetAllFilters()).Returns(GetTestFilterRepoOptions<PsfTrainingRoute>(addNotApplicable));
            A.CallTo(() => psfFakeJobAreaRepository.GetAllFilters()).Returns(GetTestFilterRepoOptions<PsfJobArea>(addNotApplicable));
            A.CallTo(() => psfFakeCareerFocusRepository.GetAllFilters()).Returns(GetTestFilterRepoOptions<PsfCareerFocus>(addNotApplicable));
            A.CallTo(() => psfFakePreferredTaskTypeRepository.GetAllFilters()).Returns(GetTestFilterRepoOptions<PsfPreferredTaskType>(addNotApplicable));
            A.CallTo(() => fakeTaxonomyRepository.GetMany(A<Expression<Func<HierarchicalTaxon, bool>>>._)).Returns(new EnumerableQuery<HierarchicalTaxon>(new List<HierarchicalTaxon>()));

            A.CallTo(() => psfRepositoryFactoryFake.GetRepository<PsfInterest>()).Returns(psfFakeIntrestRepository);
            A.CallTo(() => psfRepositoryFactoryFake.GetRepository<PsfEnabler>()).Returns(psfFakeEnablerRepository);
            A.CallTo(() => psfRepositoryFactoryFake.GetRepository<PsfEntryQualification>()).Returns(psfFakeQalificationsRepository);
            A.CallTo(() => psfRepositoryFactoryFake.GetRepository<PsfTrainingRoute>()).Returns(psfFakeTrainingRepository);
            A.CallTo(() => psfRepositoryFactoryFake.GetRepository<PsfJobArea>()).Returns(psfFakeJobAreaRepository);
            A.CallTo(() => psfRepositoryFactoryFake.GetRepository<PsfCareerFocus>()).Returns(psfFakeCareerFocusRepository);
            A.CallTo(() => psfRepositoryFactoryFake.GetRepository<PsfPreferredTaskType>()).Returns(psfFakePreferredTaskTypeRepository);
        }

        private void CheckFilterSecton(PreSearchFiltersController controller, PsfSection filterSection, PreSearchFilterType expectedFilterType, bool addNotApplicable = true)
        {
            filterSection.Description.Should().BeEquivalentTo(controller.SectionDescription);
            filterSection.Name.Should().BeEquivalentTo(controller.SectionTitle);
            filterSection.NextPageUrl.Should().BeEquivalentTo(controller.NextPageUrl);
            filterSection.PreviousPageUrl.Should().BeEquivalentTo(controller.PreviousPageUrl);
            filterSection.SectionDataType.Should().BeEquivalentTo(expectedFilterType.ToString());
            filterSection.PageNumber.Should().Be(controller.ThisPageNumber);
            filterSection.TotalNumberOfPages.Should().Be(controller.TotalNumberOfPages);
            filterSection.Options.Count.Should().Be(5);

            int idx = 0;
            foreach (PreSearchFilterOption p in GetDummyPreSearchFilterOption(addNotApplicable))
            {
                filterSection.Options[idx].Id.Should().BeEquivalentTo(p.Id);
                filterSection.Options[idx].Name.Should().BeEquivalentTo(p.Name);
                filterSection.Options[idx].Description.Should().BeEquivalentTo(p.Description);
                filterSection.Options[idx].IsSelected.Should().Be(false);
                filterSection.Options[idx].ClearOtherOptionsIfSelected.Should().Be(p.ClearOtherOptionsIfSelected);
                filterSection.Options[idx].OptionKey.Should().BeEquivalentTo(p.OptionKey);
                idx++;
            }
        }

        private IEnumerable<T> GetTestFilterRepoOptions<T>(bool addNotApplicable)
            where T : PreSearchFilter, new()
        {
            for (int ii = 0; ii < NumberDummyFilterOptions; ii++)
            {
                yield return new T
                {
                    Id = new Guid("e99079a2-a201-4b45-bc81-85e807dbcb5a"),
                    Description = $"Description {ii}",
                    Title = $"Option {ii}",
                    Order = ii,
                    UrlName = $"URL-{ii}",
                    NotApplicable = addNotApplicable & (ii == (NumberDummyFilterOptions - 1))
                };
            }
        }
    }
}