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
        private IPreSearchFiltersFactory pSfRepositoryFactoryFake;
        private IPreSearchFiltersRepository<PsfInterest> pSfFakeIntrestRepository;
        private IPreSearchFiltersRepository<PsfEnabler> pSfFakeEnablerRepository;
        private IPreSearchFiltersRepository<PsfEntryQualification> pSfFakeQalificationsRepository;
        private IPreSearchFiltersRepository<PsfTrainingRoute> pSfFakeTrainingRepository;
        private IPreSearchFiltersRepository<PsfJobArea> pSfFakeJobAreaRepository;
        private IPreSearchFiltersRepository<PsfCareerFocus> pSfFakeCareerFocusRepository;
        private IPreSearchFiltersRepository<PsfPreferredTaskType> pSfFakePreferredTaskTypeRepository;

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
            pSfRepositoryFactoryFake = A.Fake<IPreSearchFiltersFactory>(ops => ops.Strict());
            pSfFakeIntrestRepository = A.Fake<IPreSearchFiltersRepository<PsfInterest>>(ops => ops.Strict());
            pSfFakeEnablerRepository = A.Fake<IPreSearchFiltersRepository<PsfEnabler>>(ops => ops.Strict());
            pSfFakeQalificationsRepository = A.Fake<IPreSearchFiltersRepository<PsfEntryQualification>>(ops => ops.Strict());
            pSfFakeTrainingRepository = A.Fake<IPreSearchFiltersRepository<PsfTrainingRoute>>(ops => ops.Strict());
            pSfFakeJobAreaRepository = A.Fake<IPreSearchFiltersRepository<PsfJobArea>>(ops => ops.Strict());
            pSfFakeCareerFocusRepository = A.Fake<IPreSearchFiltersRepository<PsfCareerFocus>>(ops => ops.Strict());
            pSfFakePreferredTaskTypeRepository = A.Fake<IPreSearchFiltersRepository<PsfPreferredTaskType>>(ops => ops.Strict());

            //Set up call
            A.CallTo(() => pSfFakeIntrestRepository.GetAllFilters()).Returns(GetTestFilterOptions<PsfInterest>(addNotApplicable));
            A.CallTo(() => pSfFakeEnablerRepository.GetAllFilters()).Returns(GetTestFilterOptions<PsfEnabler>(addNotApplicable));
            A.CallTo(() => pSfFakeQalificationsRepository.GetAllFilters()).Returns(GetTestFilterOptions<PsfEntryQualification>(addNotApplicable));
            A.CallTo(() => pSfFakeTrainingRepository.GetAllFilters()).Returns(GetTestFilterOptions<PsfTrainingRoute>(addNotApplicable));
            A.CallTo(() => pSfFakeJobAreaRepository.GetAllFilters()).Returns(GetTestFilterOptions<PsfJobArea>(addNotApplicable));
            A.CallTo(() => pSfFakeCareerFocusRepository.GetAllFilters()).Returns(GetTestFilterOptions<PsfCareerFocus>(addNotApplicable));
            A.CallTo(() => pSfFakePreferredTaskTypeRepository.GetAllFilters()).Returns(GetTestFilterOptions<PsfPreferredTaskType>(addNotApplicable));

            A.CallTo(() => pSfRepositoryFactoryFake.GetRepository<PsfInterest>()).Returns(pSfFakeIntrestRepository);
            A.CallTo(() => pSfRepositoryFactoryFake.GetRepository<PsfEnabler>()).Returns(pSfFakeEnablerRepository);
            A.CallTo(() => pSfRepositoryFactoryFake.GetRepository<PsfEntryQualification>()).Returns(pSfFakeQalificationsRepository);
            A.CallTo(() => pSfRepositoryFactoryFake.GetRepository<PsfTrainingRoute>()).Returns(pSfFakeTrainingRepository);
            A.CallTo(() => pSfRepositoryFactoryFake.GetRepository<PsfJobArea>()).Returns(pSfFakeJobAreaRepository);
            A.CallTo(() => pSfRepositoryFactoryFake.GetRepository<PsfCareerFocus>()).Returns(pSfFakeCareerFocusRepository);
            A.CallTo(() => pSfRepositoryFactoryFake.GetRepository<PsfPreferredTaskType>()).Returns(pSfFakePreferredTaskTypeRepository);
        }

        private IEnumerable<PreSearchFilter> GetFilterOptions(PreSearchFilterType preSearchFilterType)
        {
            switch (preSearchFilterType)
            {
                case PreSearchFilterType.Enabler:
                {
                    return pSfRepositoryFactoryFake.GetRepository<PsfEnabler>().GetAllFilters().OrderBy(o => o.Order);
                }

                case PreSearchFilterType.EntryQualification:
                {
                    return pSfRepositoryFactoryFake.GetRepository<PsfEntryQualification>().GetAllFilters().OrderBy(o => o.Order);
                }

                case PreSearchFilterType.Interest:
                {
                    return pSfRepositoryFactoryFake.GetRepository<PsfInterest>().GetAllFilters().OrderBy(o => o.Order);
                }

                case PreSearchFilterType.TrainingRoute:
                {
                    return pSfRepositoryFactoryFake.GetRepository<PsfTrainingRoute>().GetAllFilters().OrderBy(o => o.Order);
                }

                case PreSearchFilterType.JobArea:
                {
                    return pSfRepositoryFactoryFake.GetRepository<PsfJobArea>().GetAllFilters().OrderBy(o => o.Order);
                }

                case PreSearchFilterType.CareerFocus:
                {
                    return pSfRepositoryFactoryFake.GetRepository<PsfCareerFocus>().GetAllFilters().OrderBy(o => o.Order);
                }

                case PreSearchFilterType.PreferredTaskType:
                {
                    return pSfRepositoryFactoryFake.GetRepository<PsfPreferredTaskType>().GetAllFilters().OrderBy(o => o.Order);
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