using AutoMapper;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Config;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Controllers;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models;
using FakeItEasy;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using TestStack.FluentMVCTesting;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.UnitTests.Controllers
{
    public class PreSearchFiltersControllerTests
    {
        private const int NumberDummyFilterOptions = 5;

        private IPreSearchFiltersFactory pSfRepositoryFactoryFake;
        private IApplicationLogger loggerFake;
        private IWebAppContext webAppContextFake;
        private IPreSearchFiltersRepository<PsfInterest> pSfFakeIntrestRepository;
        private IPreSearchFiltersRepository<PsfEnabler> pSfFakeEnablerRepository;
        private IPreSearchFiltersRepository<PsfEntryQualification> pSfFakeQalificationsRepository;
        private IPreSearchFiltersRepository<PsfTrainingRoute> pSfFakeTrainingRepository;
        private IPreSearchFiltersRepository<PsfJobArea> pSfFakeJobAreaRepository;
        private IPreSearchFiltersRepository<PsfCareerFocus> pSfFakeCareerFocusRepository;
        private IPreSearchFiltersRepository<PsfPreferredTaskType> pSfFakePreferredTaskTypeRepository;
        private IPreSearchFilterStateManager fakePsfStateManager;
        private IMapper fakeAutoMapper;

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
            var preSearchFiltersController = new PreSearchFiltersController(webAppContextFake, loggerFake, mapper, pSfRepositoryFactoryFake, fakePsfStateManager);
            preSearchFiltersController.FilterType = PreSearchFilterType.Interest;

            //Act on the index
            var indexResult = preSearchFiltersController.WithCallTo(c => c.Index());

            PsfModel firstVm = null;

            //Assert
            indexResult.ShouldRenderDefaultView().WithModel<PsfModel>(vm =>
            {
                CheckFilterSecton(preSearchFiltersController, vm.Section, PreSearchFilterType.Interest);
                firstVm = vm;
            }).AndNoModelErrors();

            A.CallTo(() => pSfFakeIntrestRepository.GetAllFilters()).MustHaveHappened();

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
            var preSearchFiltersController = new PreSearchFiltersController(webAppContextFake, loggerFake, mapper, pSfRepositoryFactoryFake, fakePsfStateManager);
            preSearchFiltersController.FilterType = PreSearchFilterType.Interest;

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

            A.CallTo(() => pSfFakeIntrestRepository.GetAllFilters()).MustHaveHappened();
            A.CallTo(() => pSfFakeIntrestRepository.GetAllFilters()).MustHaveHappened();

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
            var preSearchFiltersController = new PreSearchFiltersController(webAppContextFake, loggerFake, mapper, pSfRepositoryFactoryFake, fakePsfStateManager);
            preSearchFiltersController.FilterType = PreSearchFilterType.Interest;

            //Act on the index
            var firstVm = new PsfModel();
            var postFromResultsPageCall = preSearchFiltersController.WithCallTo(c => c.Index(firstVm, null));
            postFromResultsPageCall.ShouldRenderDefaultView().WithModel<PsfModel>(vm =>
            {
                vm.Section.Should().NotBeNull();
            })
            .AndNoModelErrors();

            A.CallTo(() => pSfFakeIntrestRepository.GetAllFilters()).MustHaveHappened();
            A.CallTo(() => pSfFakeIntrestRepository.GetAllFilters()).MustHaveHappened();

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
            var preSearchFiltersController = new PreSearchFiltersController(webAppContextFake, loggerFake, mapper, pSfRepositoryFactoryFake, fakePsfStateManager)
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
            var filtersModel = new PsfModel() { OptionsSelected = "DummyJsonState" };

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
            var dummyStateSection = new PreSearchFilterSection()
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
                retList.Add(new PreSearchFilterOption()
                {
                    Id = "e99079a2-a201-4b45-bc81-85e807dbcb5a",
                    Description = $"Description {ii.ToString()}",
                    Name = $"Option {ii.ToString()}",
                    IsSelected = false,
                    OptionKey = $"DummyOptionKey",
                    ClearOtherOptionsIfSelected = addNotApplicable & (ii == (NumberDummyFilterOptions - 1))
                });
            }

            return retList;
        }

        private void SetUpFakesAndCalls(bool addNotApplicable = true)
        {
            pSfRepositoryFactoryFake = A.Fake<IPreSearchFiltersFactory>(ops => ops.Strict());
            loggerFake = A.Fake<IApplicationLogger>(ops => ops.Strict());
            webAppContextFake = A.Fake<IWebAppContext>(ops => ops.Strict());

            pSfFakeIntrestRepository = A.Fake<IPreSearchFiltersRepository<PsfInterest>>(ops => ops.Strict());
            pSfFakeEnablerRepository = A.Fake<IPreSearchFiltersRepository<PsfEnabler>>(ops => ops.Strict());
            pSfFakeQalificationsRepository = A.Fake<IPreSearchFiltersRepository<PsfEntryQualification>>(ops => ops.Strict());
            pSfFakeTrainingRepository = A.Fake<IPreSearchFiltersRepository<PsfTrainingRoute>>(ops => ops.Strict());
            pSfFakeJobAreaRepository = A.Fake<IPreSearchFiltersRepository<PsfJobArea>>(ops => ops.Strict());
            pSfFakeCareerFocusRepository = A.Fake<IPreSearchFiltersRepository<PsfCareerFocus>>(ops => ops.Strict());
            pSfFakePreferredTaskTypeRepository = A.Fake<IPreSearchFiltersRepository<PsfPreferredTaskType>>(ops => ops.Strict());
            fakeAutoMapper = A.Fake<IMapper>(ops => ops.Strict());

            //Set up call
            A.CallTo(() => pSfFakeIntrestRepository.GetAllFilters()).Returns(GetTestFilterRepoOptions<PsfInterest>(addNotApplicable));
            A.CallTo(() => pSfFakeEnablerRepository.GetAllFilters()).Returns(GetTestFilterRepoOptions<PsfEnabler>(addNotApplicable));
            A.CallTo(() => pSfFakeQalificationsRepository.GetAllFilters()).Returns(GetTestFilterRepoOptions<PsfEntryQualification>(addNotApplicable));
            A.CallTo(() => pSfFakeTrainingRepository.GetAllFilters()).Returns(GetTestFilterRepoOptions<PsfTrainingRoute>(addNotApplicable));
            A.CallTo(() => pSfFakeJobAreaRepository.GetAllFilters()).Returns(GetTestFilterRepoOptions<PsfJobArea>(addNotApplicable));
            A.CallTo(() => pSfFakeCareerFocusRepository.GetAllFilters()).Returns(GetTestFilterRepoOptions<PsfCareerFocus>(addNotApplicable));
            A.CallTo(() => pSfFakePreferredTaskTypeRepository.GetAllFilters()).Returns(GetTestFilterRepoOptions<PsfPreferredTaskType>(addNotApplicable));

            A.CallTo(() => pSfRepositoryFactoryFake.GetRepository<PsfInterest>()).Returns(pSfFakeIntrestRepository);
            A.CallTo(() => pSfRepositoryFactoryFake.GetRepository<PsfEnabler>()).Returns(pSfFakeEnablerRepository);
            A.CallTo(() => pSfRepositoryFactoryFake.GetRepository<PsfEntryQualification>()).Returns(pSfFakeQalificationsRepository);
            A.CallTo(() => pSfRepositoryFactoryFake.GetRepository<PsfTrainingRoute>()).Returns(pSfFakeTrainingRepository);
            A.CallTo(() => pSfRepositoryFactoryFake.GetRepository<PsfJobArea>()).Returns(pSfFakeJobAreaRepository);
            A.CallTo(() => pSfRepositoryFactoryFake.GetRepository<PsfCareerFocus>()).Returns(pSfFakeCareerFocusRepository);
            A.CallTo(() => pSfRepositoryFactoryFake.GetRepository<PsfPreferredTaskType>()).Returns(pSfFakePreferredTaskTypeRepository);
        }

        private void CheckFilterSecton(PreSearchFiltersController controller, PsfSection filterSection, PreSearchFilterType expectedFilterType, bool addNotApplicable = true)
        {
            filterSection.Description.ShouldBeEquivalentTo(controller.SectionDescription);
            filterSection.Name.ShouldBeEquivalentTo(controller.SectionTitle);
            filterSection.NextPageUrl.ShouldBeEquivalentTo(controller.NextPageUrl);
            filterSection.PreviousPageUrl.ShouldBeEquivalentTo(controller.PreviousPageUrl);
            filterSection.SectionDataType.ShouldBeEquivalentTo(expectedFilterType.ToString());
            filterSection.PageNumber.ShouldBeEquivalentTo(controller.ThisPageNumber);
            filterSection.TotalNumberOfPages.ShouldBeEquivalentTo(controller.TotalNumberOfPages);
            filterSection.Options.Count.ShouldBeEquivalentTo(5);

            int idx = 0;
            foreach (PreSearchFilterOption p in GetDummyPreSearchFilterOption(addNotApplicable))
            {
                filterSection.Options[idx].Id.ShouldBeEquivalentTo(p.Id);
                filterSection.Options[idx].Name.ShouldBeEquivalentTo(p.Name);
                filterSection.Options[idx].Description.ShouldBeEquivalentTo(p.Description);
                filterSection.Options[idx].IsSelected.ShouldBeEquivalentTo(false);
                filterSection.Options[idx].ClearOtherOptionsIfSelected.ShouldBeEquivalentTo(p.ClearOtherOptionsIfSelected);
                filterSection.Options[idx].OptionKey.ShouldBeEquivalentTo(p.OptionKey);
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
                    Id = new System.Guid("e99079a2-a201-4b45-bc81-85e807dbcb5a"),
                    Description = $"Description {ii.ToString()}",
                    Title = $"Option {ii.ToString()}",
                    Order = ii,
                    UrlName = $"URL-{ii.ToString()}",
                    NotApplicable = addNotApplicable & (ii == (NumberDummyFilterOptions - 1))
                };
            }
        }
    }
}