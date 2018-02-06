using System;
using System.Collections.Generic;
using System.Linq;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace DFC.Digital.Service.AzureSearch.Tests
{
    public class PreSearchFilterStateManagerTests
    {
        private const int NumberDummyFilterOptions = 5;
        private IPreSearchFiltersFactory pSFRepositoryFactoryFake;
        private IPreSearchFiltersRepository<PSFInterest> pSFFakeIntrestRepository;
        private IPreSearchFiltersRepository<PSFEnabler> pSFFakeEnablerRepository;
        private IPreSearchFiltersRepository<PSFEntryQualification> pSFFakeQalificationsRepository;
        private IPreSearchFiltersRepository<PSFTrainingRoute> pSFFakeTrainingRepository;
        private IPreSearchFiltersRepository<PSFJobArea> pSFFakeJobAreaRepository;
        private IPreSearchFiltersRepository<PSFCareerFocus> pSFFakeCareerFocusRepository;
        private IPreSearchFiltersRepository<PSFPreferredTaskType> pSFFakePreferredTaskTypeRepository;

        [Theory]
        [InlineData("")]
        [InlineData("{\"Sections\":[]}")]
        public void RestoreStateTest(string stateJson)
        {
            // Act
            var testObject = new PreSearchFilterStateManager();
            testObject.RestoreState(stateJson);

            // Assert
            testObject.GetStateJson().ShouldBeEquivalentTo(stateJson);
        }

        [Theory]
        [InlineData(true, "{\"Sections\":[]}", "{\"Sections\":[{\"Name\":null,\"SectionDataType\":\"Interest\",\"Options\":[{\"IsSelected\":true,\"ClearOtherOptionsIfSelected\":false,\"OptionKey\":\"OptionKey\"}],\"SingleSelectedValue\":null}]}")]
        [InlineData(false, "{\"Sections\":[]}", "{\"Sections\":[{\"Name\":null,\"SectionDataType\":\"Interest\",\"Options\":[],\"SingleSelectedValue\":null}]}")]
        public void UpdateSectionStateTest(bool validSection, string stateJson, string expectedJson)
        {
            // Assign
            var section = new PreSearchFilterSection
            {
                Options = validSection
                ? new List<PreSearchFilterOption>
                {
                    new PreSearchFilterOption
                    {
                        IsSelected = true,
                        Name = nameof(PreSearchFilterOption.Name),
                        OptionKey = nameof(PreSearchFilterOption.OptionKey)
                    }
                }
                : new List<PreSearchFilterOption>()
            };

            // Act
            var testObject = new PreSearchFilterStateManager();
            testObject.RestoreState(stateJson);
            testObject.UpdateSectionState(section);

            // Assert
            testObject.GetStateJson().ShouldBeEquivalentTo(expectedJson);
        }

        [Theory]
        [InlineData(PreSearchFilterType.PreferredTaskType, "test2")]
        [InlineData(PreSearchFilterType.CareerFocus, "test3")]
        public void GetPreSearchFilterStateTest(PreSearchFilterType preSearchFilter, string sectionTitle)
        {
            // Assign
            var stateJson = "{\"Sections\":[]}";
            var section = GetPreSearchFilterSection(preSearchFilter, sectionTitle);

            // Act
            var testObject = new PreSearchFilterStateManager();
            testObject.RestoreState(stateJson);
            testObject.UpdateSectionState(section);
            var result = testObject.GetPreSearchFilterState();

            // Assert
            result.Sections.First().ShouldBeEquivalentTo(section);
        }

        [Theory]
        [InlineData(PreSearchFilterType.PreferredTaskType, "test2")]
        [InlineData(PreSearchFilterType.CareerFocus, "test3")]
        public void GetSavedSectionTest(PreSearchFilterType preSearchFilter, string sectionTitle)
        {
            // Assign
            var stateJson = "{\"Sections\":[]}";
            var section = GetPreSearchFilterSection(preSearchFilter, sectionTitle);

            // Act
            var testObject = new PreSearchFilterStateManager();
            testObject.RestoreState(stateJson);
            testObject.UpdateSectionState(section);
            var result = testObject.GetSavedSection(sectionTitle, preSearchFilter);

            // Assert
            result.ShouldBeEquivalentTo(section);
        }

        [Theory]
        [InlineData(PreSearchFilterType.PreferredTaskType, "test")]
        [InlineData(PreSearchFilterType.CareerFocus, "test")]
        public void RestoreOptionsTest(PreSearchFilterType preSearchFilter, string sectionTitle)
        {
            // Assign
            var stateJson = "{\"Sections\":[]}";
            var section = GetPreSearchFilterSection(preSearchFilter, sectionTitle);
            SetUpFakesAndCalls(false);

            // Act
            var testObject = new PreSearchFilterStateManager();
            testObject.RestoreState(stateJson);
            testObject.UpdateSectionState(section);
            var result = testObject.RestoreOptions(section, GetFilterOptions(preSearchFilter));

            // Assert
            result.Options.First(x => x.IsSelected).OptionKey.ShouldBeEquivalentTo(section.Options.First().OptionKey);
        }

        [Theory]
        [InlineData(true, false, "{\"Sections\":[{\"Name\":null,\"SectionDataType\":\"Interest\",\"Options\":[{\"IsSelected\":true,\"ClearOtherOptionsIfSelected\":false,\"OptionKey\":\"SingleSelectedValue\"}],\"SingleSelectedValue\":\"\"}]}")]
        [InlineData(true, true, "{\"Sections\":[{\"Name\":null,\"SectionDataType\":\"Interest\",\"Options\":[{\"IsSelected\":true,\"ClearOtherOptionsIfSelected\":false,\"OptionKey\":\"SingleSelectedValue\"}],\"SingleSelectedValue\":\"SingleSelectedValue\"}]}")]
        [InlineData(false, false, "{\"Sections\":[]}")]
        public void SaveStateTest(bool validPreviousSection, bool singleValue, string expectedJson)
        {
            // Assign
            var stateJson = "{\"Sections\":[]}";
            var section = validPreviousSection
                ? new PreSearchFilterSection
                {
                    Options = new List<PreSearchFilterOption>
                    {
                        new PreSearchFilterOption
                        {
                            IsSelected = true,
                            Name = nameof(PreSearchFilterOption.Name),
                            OptionKey = nameof(PreSearchFilterSection.SingleSelectedValue)
                        }
                    },
                    SingleSelectedValue = singleValue ? nameof(PreSearchFilterSection.SingleSelectedValue) : string.Empty,
                    SingleSelectOnly = singleValue
                }
                : null;

            // Act
            var testObject = new PreSearchFilterStateManager();
            testObject.RestoreState(stateJson);
            testObject.SaveState(section);

            // Assert
            testObject.GetStateJson().ShouldBeEquivalentTo(expectedJson);
        }

        [Theory]
        [InlineData(2, 1, true)]
        [InlineData(3, 4, false)]
        [InlineData(4, 4, false)]
        [InlineData(0, 0, false)]
        [InlineData(7, 6, true)]
        public void ShouldSaveStateTest(int currentPage, int previousPage, bool expected)
        {
            // Act
            var testObject = new PreSearchFilterStateManager();
            var result = testObject.ShouldSaveState(currentPage, previousPage);

            // Assert
            result.ShouldBeEquivalentTo(expected);
        }

        private static PreSearchFilterSection GetPreSearchFilterSection(PreSearchFilterType preSearchFilter, string sectionTitle)
        {
            var section = new PreSearchFilterSection
            {
                SectionDataType = preSearchFilter,
                Name = sectionTitle,
                Options = new List<PreSearchFilterOption>
                {
                    new PreSearchFilterOption { Name = nameof(PreSearchFilterOption.Name), IsSelected = true, OptionKey = $"e99079a2-a201-4b45-bc81-85e807dbcb5a|URL-{NumberDummyFilterOptions - 2}" }
                }
            };
            return section;
        }

        private void SetUpFakesAndCalls(bool addNotApplicable)
        {
            pSFRepositoryFactoryFake = A.Fake<IPreSearchFiltersFactory>(ops => ops.Strict());
            pSFFakeIntrestRepository = A.Fake<IPreSearchFiltersRepository<PSFInterest>>(ops => ops.Strict());
            pSFFakeEnablerRepository = A.Fake<IPreSearchFiltersRepository<PSFEnabler>>(ops => ops.Strict());
            pSFFakeQalificationsRepository = A.Fake<IPreSearchFiltersRepository<PSFEntryQualification>>(ops => ops.Strict());
            pSFFakeTrainingRepository = A.Fake<IPreSearchFiltersRepository<PSFTrainingRoute>>(ops => ops.Strict());
            pSFFakeJobAreaRepository = A.Fake<IPreSearchFiltersRepository<PSFJobArea>>(ops => ops.Strict());
            pSFFakeCareerFocusRepository = A.Fake<IPreSearchFiltersRepository<PSFCareerFocus>>(ops => ops.Strict());
            pSFFakePreferredTaskTypeRepository = A.Fake<IPreSearchFiltersRepository<PSFPreferredTaskType>>(ops => ops.Strict());

            //Set up call
            A.CallTo(() => pSFFakeIntrestRepository.GetAllFilters()).Returns(GetTestFilterOptions<PSFInterest>(addNotApplicable));
            A.CallTo(() => pSFFakeEnablerRepository.GetAllFilters()).Returns(GetTestFilterOptions<PSFEnabler>(addNotApplicable));
            A.CallTo(() => pSFFakeQalificationsRepository.GetAllFilters()).Returns(GetTestFilterOptions<PSFEntryQualification>(addNotApplicable));
            A.CallTo(() => pSFFakeTrainingRepository.GetAllFilters()).Returns(GetTestFilterOptions<PSFTrainingRoute>(addNotApplicable));
            A.CallTo(() => pSFFakeJobAreaRepository.GetAllFilters()).Returns(GetTestFilterOptions<PSFJobArea>(addNotApplicable));
            A.CallTo(() => pSFFakeCareerFocusRepository.GetAllFilters()).Returns(GetTestFilterOptions<PSFCareerFocus>(addNotApplicable));
            A.CallTo(() => pSFFakePreferredTaskTypeRepository.GetAllFilters()).Returns(GetTestFilterOptions<PSFPreferredTaskType>(addNotApplicable));

            A.CallTo(() => pSFRepositoryFactoryFake.GetRepository<PSFInterest>()).Returns(pSFFakeIntrestRepository);
            A.CallTo(() => pSFRepositoryFactoryFake.GetRepository<PSFEnabler>()).Returns(pSFFakeEnablerRepository);
            A.CallTo(() => pSFRepositoryFactoryFake.GetRepository<PSFEntryQualification>()).Returns(pSFFakeQalificationsRepository);
            A.CallTo(() => pSFRepositoryFactoryFake.GetRepository<PSFTrainingRoute>()).Returns(pSFFakeTrainingRepository);
            A.CallTo(() => pSFRepositoryFactoryFake.GetRepository<PSFJobArea>()).Returns(pSFFakeJobAreaRepository);
            A.CallTo(() => pSFRepositoryFactoryFake.GetRepository<PSFCareerFocus>()).Returns(pSFFakeCareerFocusRepository);
            A.CallTo(() => pSFRepositoryFactoryFake.GetRepository<PSFPreferredTaskType>()).Returns(pSFFakePreferredTaskTypeRepository);
        }

        private IEnumerable<PreSearchFilter> GetFilterOptions(PreSearchFilterType preSearchFilterType)
        {
            switch (preSearchFilterType)
            {
                case PreSearchFilterType.Enabler:
                {
                    return pSFRepositoryFactoryFake.GetRepository<PSFEnabler>().GetAllFilters().OrderBy(o => o.Order);
                }

                case PreSearchFilterType.EntryQualification:
                {
                    return pSFRepositoryFactoryFake.GetRepository<PSFEntryQualification>().GetAllFilters().OrderBy(o => o.Order);
                }

                case PreSearchFilterType.Interest:
                {
                    return pSFRepositoryFactoryFake.GetRepository<PSFInterest>().GetAllFilters().OrderBy(o => o.Order);
                }

                case PreSearchFilterType.TrainingRoute:
                {
                    return pSFRepositoryFactoryFake.GetRepository<PSFTrainingRoute>().GetAllFilters().OrderBy(o => o.Order);
                }

                case PreSearchFilterType.JobArea:
                {
                    return pSFRepositoryFactoryFake.GetRepository<PSFJobArea>().GetAllFilters().OrderBy(o => o.Order);
                }

                case PreSearchFilterType.CareerFocus:
                {
                    return pSFRepositoryFactoryFake.GetRepository<PSFCareerFocus>().GetAllFilters().OrderBy(o => o.Order);
                }

                case PreSearchFilterType.PreferredTaskType:
                {
                    return pSFRepositoryFactoryFake.GetRepository<PSFPreferredTaskType>().GetAllFilters().OrderBy(o => o.Order);
                }

                default:
                {
                    return Enumerable.Empty<PreSearchFilter>();
                }
            }
        }

        private IEnumerable<T> GetTestFilterOptions<T>(bool addNotApplicable)
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