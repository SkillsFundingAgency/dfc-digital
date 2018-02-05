using DFC.Digital.Data.Model;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DFC.Digital.Service.AzureSearch.UnitTests
{
    public class DfcBuildFilterServiceTests
    {
        [Theory]
        [MemberData(nameof(GetPsfTestData))]
        public void BuildPreSearchFilterOperationsTest(IEnumerable<KeyValuePair<string, int>> countOfFilters, IEnumerable<KeyValuePair<string, PreSearchFilterLogicalOperator>> filterFields, string expectedFilterBy)
        {
            var model = new PreSearchFiltersResultsModel
            {
                Sections = new List<FilterResultsSection>()
            };

            foreach (var item in countOfFilters)
            {
                var section = new FilterResultsSection
                {
                    SectionDataType = item.Key,
                    Name = item.Key,
                    Options = GetTestFilterOptions(item).ToList(),
                    SingleSelectOnly = item.Value == 1,
                    SingleSelectedValue = item.Value == 1 ? $"{item.Key.ToLowerInvariant()}{item.Value}" : null
                };
                model.Sections.Add(section);
            }

            var testObject = new DfcBuildFilterService();
            var result = testObject.BuildPreSearchFilters(model, filterFields.ToDictionary(k => k.Key, v => v.Value));

            result.Should().Be(expectedFilterBy);
        }

        private static IEnumerable<object[]> GetPsfTestData()
        {
            yield return new object[]
            {
                new Dictionary<string, int>
                {
                    { nameof(PreSearchFilterType.Interest), 1 },
                    { nameof(PreSearchFilterType.TrainingRoute), 1 },
                    { nameof(PreSearchFilterType.EntryQualification), 1 },
                    { nameof(PreSearchFilterType.Enabler), 1 },
                    { nameof(PreSearchFilterType.JobArea), 1 },
                    { nameof(PreSearchFilterType.PreferredTaskType), 1 }
                },
                new Dictionary<string, PreSearchFilterLogicalOperator>
                {
                    { nameof(JobProfileIndex.Interests), PreSearchFilterLogicalOperator.Or },
                    { nameof(JobProfileIndex.TrainingRoutes), PreSearchFilterLogicalOperator.Or },
                    { nameof(JobProfileIndex.EntryQualifications), PreSearchFilterLogicalOperator.Or },
                    { nameof(JobProfileIndex.Enablers), PreSearchFilterLogicalOperator.Or },
                    { nameof(JobProfileIndex.JobAreas), PreSearchFilterLogicalOperator.Or },
                    { nameof(JobProfileIndex.PreferredTaskTypes), PreSearchFilterLogicalOperator.Or },
                },
                "Interests/any(t: search.in(t, 'interest1')) or TrainingRoutes/any(t: search.in(t, 'trainingroute1')) or EntryQualifications/any(t: search.in(t, 'entryqualification1')) or Enablers/any(t: search.in(t, 'enabler1')) or JobAreas/any(t: search.in(t, 'jobarea1')) or PreferredTaskTypes/any(t: search.in(t, 'preferredtasktype1'))"
            };
            yield return new object[]
            {
                new Dictionary<string, int>
                {
                    { nameof(PreSearchFilterType.Interest), 1 },
                    { nameof(PreSearchFilterType.TrainingRoute), 1 },
                    { nameof(PreSearchFilterType.EntryQualification), 1 },
                    { nameof(PreSearchFilterType.Enabler), 1 },
                    { nameof(PreSearchFilterType.JobArea), 1 },
                    { nameof(PreSearchFilterType.PreferredTaskType), 1 }
                },
                new Dictionary<string, PreSearchFilterLogicalOperator>
                {
                    { nameof(JobProfileIndex.Interests), PreSearchFilterLogicalOperator.And },
                    { nameof(JobProfileIndex.TrainingRoutes), PreSearchFilterLogicalOperator.And },
                    { nameof(JobProfileIndex.EntryQualifications), PreSearchFilterLogicalOperator.And },
                    { nameof(JobProfileIndex.Enablers), PreSearchFilterLogicalOperator.And },
                    { nameof(JobProfileIndex.JobAreas), PreSearchFilterLogicalOperator.And },
                    { nameof(JobProfileIndex.PreferredTaskTypes), PreSearchFilterLogicalOperator.And }
                },
                "Interests/any(t: search.in(t, 'interest1')) and TrainingRoutes/any(t: search.in(t, 'trainingroute1')) and EntryQualifications/any(t: search.in(t, 'entryqualification1')) and Enablers/any(t: search.in(t, 'enabler1')) and JobAreas/any(t: search.in(t, 'jobarea1')) and PreferredTaskTypes/any(t: search.in(t, 'preferredtasktype1'))"
            };
            yield return new object[]
            {
                new Dictionary<string, int>
                {
                    { nameof(PreSearchFilterType.Interest), 1 },
                    { nameof(PreSearchFilterType.TrainingRoute), 1 },
                    { nameof(PreSearchFilterType.EntryQualification), 1 },
                    { nameof(PreSearchFilterType.Enabler), 1 },
                    { nameof(PreSearchFilterType.JobArea), 1 },
                    { nameof(PreSearchFilterType.PreferredTaskType), 1 }
                },
                new Dictionary<string, PreSearchFilterLogicalOperator>
                {
                    { nameof(JobProfileIndex.Interests), PreSearchFilterLogicalOperator.Or },
                    { nameof(JobProfileIndex.TrainingRoutes), PreSearchFilterLogicalOperator.Or },
                    { nameof(JobProfileIndex.EntryQualifications), PreSearchFilterLogicalOperator.Or },
                    { nameof(JobProfileIndex.Enablers), PreSearchFilterLogicalOperator.And },
                    { nameof(JobProfileIndex.JobAreas), PreSearchFilterLogicalOperator.And },
                    { nameof(JobProfileIndex.PreferredTaskTypes), PreSearchFilterLogicalOperator.Or }
                },
                "Interests/any(t: search.in(t, 'interest1')) or TrainingRoutes/any(t: search.in(t, 'trainingroute1')) or EntryQualifications/any(t: search.in(t, 'entryqualification1')) and Enablers/any(t: search.in(t, 'enabler1')) and JobAreas/any(t: search.in(t, 'jobarea1')) or PreferredTaskTypes/any(t: search.in(t, 'preferredtasktype1'))"
            };
            yield return new object[]
            {
                new Dictionary<string, int>
                {
                    { nameof(PreSearchFilterType.Interest), 1 },
                    { nameof(PreSearchFilterType.TrainingRoute), 2 },
                    { nameof(PreSearchFilterType.EntryQualification), 3 },
                    { nameof(PreSearchFilterType.Enabler), 4 },
                    { nameof(PreSearchFilterType.JobArea), 3 },
                    { nameof(PreSearchFilterType.PreferredTaskType), 4 }
                },
                new Dictionary<string, PreSearchFilterLogicalOperator>
                {
                    { nameof(JobProfileIndex.Interests), PreSearchFilterLogicalOperator.Or },
                    { nameof(JobProfileIndex.TrainingRoutes), PreSearchFilterLogicalOperator.Or },
                    { nameof(JobProfileIndex.EntryQualifications), PreSearchFilterLogicalOperator.Or },
                    { nameof(JobProfileIndex.Enablers), PreSearchFilterLogicalOperator.And },
                    { nameof(JobProfileIndex.JobAreas), PreSearchFilterLogicalOperator.Or },
                    { nameof(JobProfileIndex.PreferredTaskTypes), PreSearchFilterLogicalOperator.And }
                },
                "Interests/any(t: search.in(t, 'interest1')) or TrainingRoutes/any(t: search.in(t, 'trainingroute1,trainingroute2')) or EntryQualifications/any(t: search.in(t, 'entryqualification1,entryqualification2,entryqualification3')) and Enablers/any(t: search.in(t, 'enabler1,enabler2,enabler3,enabler4')) or JobAreas/any(t: search.in(t, 'jobarea1,jobarea2,jobarea3')) and PreferredTaskTypes/any(t: search.in(t, 'preferredtasktype1,preferredtasktype2,preferredtasktype3,preferredtasktype4'))"
            };
        }

        private IEnumerable<FilterResultsOption> GetTestFilterOptions(KeyValuePair<string, int> item)
        {
            for (int index = 0; index < item.Value; index++)
            {
                yield return new FilterResultsOption
                {
                    Id = (index + 1).ToString(),
                    IsSelected = true,
                    OptionKey = $"{item.Key.ToLowerInvariant()}{index + 1}",
                };
            }
        }
    }
}