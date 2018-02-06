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

        private IPreSearchFiltersFactory pSFRepositoryFactoryFake;
        private IApplicationLogger loggerFake;
        private IWebAppContext webAppContextFake;
        private IPreSearchFiltersRepository<PSFInterest> pSFFakeIntrestRepository;
        private IPreSearchFiltersRepository<PSFEnabler> pSFFakeEnablerRepository;
        private IPreSearchFiltersRepository<PSFEntryQualification> pSFFakeQalificationsRepository;
        private IPreSearchFiltersRepository<PSFTrainingRoute> pSFFakeTrainingRepository;
        private IPreSearchFiltersRepository<PSFJobArea> pSFFakeJobAreaRepository;
        private IPreSearchFiltersRepository<PSFCareerFocus> pSFFakeCareerFocusRepository;
        private IPreSearchFiltersRepository<PSFPreferredTaskType> pSFFakePreferredTaskTypeRepository;
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
            var preSearchFiltersController = new PreSearchFiltersController(webAppContextFake, loggerFake, mapper, pSFRepositoryFactoryFake, fakePsfStateManager);
            preSearchFiltersController.FilterType = PreSearchFilterType.Interest;

            //Act on the index
            var indexResult = preSearchFiltersController.WithCallTo(c => c.Index());

            PSFModel firstVm = null;

            //Assert
            indexResult.ShouldRenderDefaultView().WithModel<PSFModel>(vm =>
            {
                CheckFilterSecton(preSearchFiltersController, vm.Section, PreSearchFilterType.Interest);
                firstVm = vm;
            }).AndNoModelErrors();

            A.CallTo(() => pSFFakeIntrestRepository.GetAllFilters()).MustHaveHappened();

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
            var preSearchFiltersController = new PreSearchFiltersController(webAppContextFake, loggerFake, mapper, pSFRepositoryFactoryFake, fakePsfStateManager);
            preSearchFiltersController.FilterType = PreSearchFilterType.Interest;

            //Act on the index
            var firstVM = new PSFModel();
            var resultsViewModel = new PsfSearchResultsViewModel
            {
                PreSearchFiltersModel = GeneratePreSEarchFiltersViewModel(PreSearchFilterType.Interest)
            };

            var postFromResultsPageCall = preSearchFiltersController.WithCallTo(c => c.Index(firstVM, resultsViewModel));
            postFromResultsPageCall.ShouldRenderDefaultView().WithModel<PSFModel>(vm =>
            {
                vm.Section.Should().NotBeNull();
            })
            .AndNoModelErrors();

            A.CallTo(() => pSFFakeIntrestRepository.GetAllFilters()).MustHaveHappened();
            A.CallTo(() => pSFFakeIntrestRepository.GetAllFilters()).MustHaveHappened();

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
            var preSearchFiltersController = new PreSearchFiltersController(webAppContextFake, loggerFake, mapper, pSFRepositoryFactoryFake, fakePsfStateManager);
            preSearchFiltersController.FilterType = PreSearchFilterType.Interest;

            //Act on the index
            var firstVM = new PSFModel();
            var postFromResultsPageCall = preSearchFiltersController.WithCallTo(c => c.Index(firstVM, null));
            postFromResultsPageCall.ShouldRenderDefaultView().WithModel<PSFModel>(vm =>
            {
                vm.Section.Should().NotBeNull();
            })
            .AndNoModelErrors();

            A.CallTo(() => pSFFakeIntrestRepository.GetAllFilters()).MustHaveHappened();
            A.CallTo(() => pSFFakeIntrestRepository.GetAllFilters()).MustHaveHappened();

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
            var preSearchFiltersController = new PreSearchFiltersController(webAppContextFake, loggerFake, mapper, pSFRepositoryFactoryFake, fakePsfStateManager)
            {
                FilterType = filterType
            };

            //Act on the index
            var indexResult = preSearchFiltersController.WithCallTo(c => c.Index());

            //Assert
            indexResult.ShouldRenderDefaultView().WithModel<PSFModel>(vm =>
            {
                vm.Section.Should().NotBeNull();
                CheckFilterSecton(preSearchFiltersController, vm.Section, filterType);
            }).AndNoModelErrors();
        }

        private PSFModel GeneratePreSEarchFiltersViewModel(PreSearchFilterType filterType)
        {
            var filtersModel = new PSFModel() { OptionsSelected = "DummyJsonState" };

            var filterSectionOne = new PSFSection
            {
                Name = "Multi Select Section One",
                Description = "Dummy Title One",
                SingleSelectOnly = false,
                NextPageURL = "NextSectionURL",
                PreviousPageURL = "HomePageURL",
                PageNumber = 1,
                TotalNumberOfPages = 2,
                SectionDataType = filterType.ToString()
            };

            filterSectionOne.Options = new List<PSFOption>();

            for (int ii = 0; ii < 3; ii++)
            {
                var iiString = ii.ToString();
                filterSectionOne.Options.Add(item: new PSFOption
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
            pSFRepositoryFactoryFake = A.Fake<IPreSearchFiltersFactory>(ops => ops.Strict());
            loggerFake = A.Fake<IApplicationLogger>(ops => ops.Strict());
            webAppContextFake = A.Fake<IWebAppContext>(ops => ops.Strict());

            pSFFakeIntrestRepository = A.Fake<IPreSearchFiltersRepository<PSFInterest>>(ops => ops.Strict());
            pSFFakeEnablerRepository = A.Fake<IPreSearchFiltersRepository<PSFEnabler>>(ops => ops.Strict());
            pSFFakeQalificationsRepository = A.Fake<IPreSearchFiltersRepository<PSFEntryQualification>>(ops => ops.Strict());
            pSFFakeTrainingRepository = A.Fake<IPreSearchFiltersRepository<PSFTrainingRoute>>(ops => ops.Strict());
            pSFFakeJobAreaRepository = A.Fake<IPreSearchFiltersRepository<PSFJobArea>>(ops => ops.Strict());
            pSFFakeCareerFocusRepository = A.Fake<IPreSearchFiltersRepository<PSFCareerFocus>>(ops => ops.Strict());
            pSFFakePreferredTaskTypeRepository = A.Fake<IPreSearchFiltersRepository<PSFPreferredTaskType>>(ops => ops.Strict());
            fakeAutoMapper = A.Fake<IMapper>(ops => ops.Strict());

            //Set up call
            A.CallTo(() => pSFFakeIntrestRepository.GetAllFilters()).Returns(GetTestFilterRepoOptions<PSFInterest>(addNotApplicable));
            A.CallTo(() => pSFFakeEnablerRepository.GetAllFilters()).Returns(GetTestFilterRepoOptions<PSFEnabler>(addNotApplicable));
            A.CallTo(() => pSFFakeQalificationsRepository.GetAllFilters()).Returns(GetTestFilterRepoOptions<PSFEntryQualification>(addNotApplicable));
            A.CallTo(() => pSFFakeTrainingRepository.GetAllFilters()).Returns(GetTestFilterRepoOptions<PSFTrainingRoute>(addNotApplicable));
            A.CallTo(() => pSFFakeJobAreaRepository.GetAllFilters()).Returns(GetTestFilterRepoOptions<PSFJobArea>(addNotApplicable));
            A.CallTo(() => pSFFakeCareerFocusRepository.GetAllFilters()).Returns(GetTestFilterRepoOptions<PSFCareerFocus>(addNotApplicable));
            A.CallTo(() => pSFFakePreferredTaskTypeRepository.GetAllFilters()).Returns(GetTestFilterRepoOptions<PSFPreferredTaskType>(addNotApplicable));

            A.CallTo(() => pSFRepositoryFactoryFake.GetRepository<PSFInterest>()).Returns(pSFFakeIntrestRepository);
            A.CallTo(() => pSFRepositoryFactoryFake.GetRepository<PSFEnabler>()).Returns(pSFFakeEnablerRepository);
            A.CallTo(() => pSFRepositoryFactoryFake.GetRepository<PSFEntryQualification>()).Returns(pSFFakeQalificationsRepository);
            A.CallTo(() => pSFRepositoryFactoryFake.GetRepository<PSFTrainingRoute>()).Returns(pSFFakeTrainingRepository);
            A.CallTo(() => pSFRepositoryFactoryFake.GetRepository<PSFJobArea>()).Returns(pSFFakeJobAreaRepository);
            A.CallTo(() => pSFRepositoryFactoryFake.GetRepository<PSFCareerFocus>()).Returns(pSFFakeCareerFocusRepository);
            A.CallTo(() => pSFRepositoryFactoryFake.GetRepository<PSFPreferredTaskType>()).Returns(pSFFakePreferredTaskTypeRepository);
        }

        private void CheckFilterSecton(PreSearchFiltersController controller, PSFSection filterSection, PreSearchFilterType expectedFilterType, bool addNotApplicable = true)
        {
            filterSection.Description.ShouldBeEquivalentTo(controller.SectionDescription);
            filterSection.Name.ShouldBeEquivalentTo(controller.SectionTitle);
            filterSection.NextPageURL.ShouldBeEquivalentTo(controller.NextPageURL);
            filterSection.PreviousPageURL.ShouldBeEquivalentTo(controller.PreviousPageURL);
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