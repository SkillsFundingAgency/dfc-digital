using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using FakeItEasy;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DFC.Digital.Service.AzureSearch.UnitTests
{
    public class PreSearchFilterStateManagerTests
    {
        private const int NumberDummyFilterOptions = 5;
        private IPreSearchFiltersFactory psfRepositoryFactoryFake;
        private IPreSearchFiltersRepository<PsfInterest> psfFakeIntrestRepository;
        private IPreSearchFiltersRepository<PsfEnabler> psfFakeEnablerRepository;
        private IPreSearchFiltersRepository<PsfEntryQualification> psfFakeQalificationsRepository;
        private IPreSearchFiltersRepository<PsfTrainingRoute> psfFakeTrainingRepository;
        private IPreSearchFiltersRepository<PsfJobArea> psfFakeJobAreaRepository;
        private IPreSearchFiltersRepository<PsfCareerFocus> psfFakeCareerFocusRepository;
        private IPreSearchFiltersRepository<PsfPreferredTaskType> psfFakePreferredTaskTypeRepository;

        [Theory]
        [InlineData("")]
        [InlineData("{\"Sections\":[]}")]
        public void RestoreStateTest(string stateJson)
        {
            // Act
            var testObject = new PreSearchFilterStateManager();
            testObject.RestoreState(stateJson);

            // Assert
            testObject.GetStateJson().Should().BeEquivalentTo(stateJson);
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
            testObject.GetStateJson().Should().BeEquivalentTo(expectedJson);
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
            result.Sections.First().Should().BeEquivalentTo(section);
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
            result.Should().BeEquivalentTo(section);
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
            result.Options.First(x => x.IsSelected).OptionKey.Should().BeEquivalentTo(section.Options.First().OptionKey);
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
            testObject.GetStateJson().Should().BeEquivalentTo(expectedJson);
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
            result.Should().Be(expected);
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
            psfRepositoryFactoryFake = A.Fake<IPreSearchFiltersFactory>(ops => ops.Strict());
            psfFakeIntrestRepository = A.Fake<IPreSearchFiltersRepository<PsfInterest>>(ops => ops.Strict());
            psfFakeEnablerRepository = A.Fake<IPreSearchFiltersRepository<PsfEnabler>>(ops => ops.Strict());
            psfFakeQalificationsRepository = A.Fake<IPreSearchFiltersRepository<PsfEntryQualification>>(ops => ops.Strict());
            psfFakeTrainingRepository = A.Fake<IPreSearchFiltersRepository<PsfTrainingRoute>>(ops => ops.Strict());
            psfFakeJobAreaRepository = A.Fake<IPreSearchFiltersRepository<PsfJobArea>>(ops => ops.Strict());
            psfFakeCareerFocusRepository = A.Fake<IPreSearchFiltersRepository<PsfCareerFocus>>(ops => ops.Strict());
            psfFakePreferredTaskTypeRepository = A.Fake<IPreSearchFiltersRepository<PsfPreferredTaskType>>(ops => ops.Strict());

            //Set up call
            A.CallTo(() => psfFakeIntrestRepository.GetAllFilters()).Returns(GetTestFilterOptions<PsfInterest>(addNotApplicable));
            A.CallTo(() => psfFakeEnablerRepository.GetAllFilters()).Returns(GetTestFilterOptions<PsfEnabler>(addNotApplicable));
            A.CallTo(() => psfFakeQalificationsRepository.GetAllFilters()).Returns(GetTestFilterOptions<PsfEntryQualification>(addNotApplicable));
            A.CallTo(() => psfFakeTrainingRepository.GetAllFilters()).Returns(GetTestFilterOptions<PsfTrainingRoute>(addNotApplicable));
            A.CallTo(() => psfFakeJobAreaRepository.GetAllFilters()).Returns(GetTestFilterOptions<PsfJobArea>(addNotApplicable));
            A.CallTo(() => psfFakeCareerFocusRepository.GetAllFilters()).Returns(GetTestFilterOptions<PsfCareerFocus>(addNotApplicable));
            A.CallTo(() => psfFakePreferredTaskTypeRepository.GetAllFilters()).Returns(GetTestFilterOptions<PsfPreferredTaskType>(addNotApplicable));

            A.CallTo(() => psfRepositoryFactoryFake.GetRepository<PsfInterest>()).Returns(psfFakeIntrestRepository);
            A.CallTo(() => psfRepositoryFactoryFake.GetRepository<PsfEnabler>()).Returns(psfFakeEnablerRepository);
            A.CallTo(() => psfRepositoryFactoryFake.GetRepository<PsfEntryQualification>()).Returns(psfFakeQalificationsRepository);
            A.CallTo(() => psfRepositoryFactoryFake.GetRepository<PsfTrainingRoute>()).Returns(psfFakeTrainingRepository);
            A.CallTo(() => psfRepositoryFactoryFake.GetRepository<PsfJobArea>()).Returns(psfFakeJobAreaRepository);
            A.CallTo(() => psfRepositoryFactoryFake.GetRepository<PsfCareerFocus>()).Returns(psfFakeCareerFocusRepository);
            A.CallTo(() => psfRepositoryFactoryFake.GetRepository<PsfPreferredTaskType>()).Returns(psfFakePreferredTaskTypeRepository);
        }

        private IEnumerable<PreSearchFilter> GetFilterOptions(PreSearchFilterType preSearchFilterType)
        {
            switch (preSearchFilterType)
            {
                case PreSearchFilterType.Enabler:
                {
                    return psfRepositoryFactoryFake.GetRepository<PsfEnabler>().GetAllFilters().OrderBy(o => o.Order);
                }

                case PreSearchFilterType.EntryQualification:
                {
                    return psfRepositoryFactoryFake.GetRepository<PsfEntryQualification>().GetAllFilters().OrderBy(o => o.Order);
                }

                case PreSearchFilterType.Interest:
                {
                    return psfRepositoryFactoryFake.GetRepository<PsfInterest>().GetAllFilters().OrderBy(o => o.Order);
                }

                case PreSearchFilterType.TrainingRoute:
                {
                    return psfRepositoryFactoryFake.GetRepository<PsfTrainingRoute>().GetAllFilters().OrderBy(o => o.Order);
                }

                case PreSearchFilterType.JobArea:
                {
                    return psfRepositoryFactoryFake.GetRepository<PsfJobArea>().GetAllFilters().OrderBy(o => o.Order);
                }

                case PreSearchFilterType.CareerFocus:
                {
                    return psfRepositoryFactoryFake.GetRepository<PsfCareerFocus>().GetAllFilters().OrderBy(o => o.Order);
                }

                case PreSearchFilterType.PreferredTaskType:
                {
                    return psfRepositoryFactoryFake.GetRepository<PsfPreferredTaskType>().GetAllFilters().OrderBy(o => o.Order);
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