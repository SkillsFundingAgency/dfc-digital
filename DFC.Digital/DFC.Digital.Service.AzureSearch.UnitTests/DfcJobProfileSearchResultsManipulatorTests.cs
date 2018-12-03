using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Services;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DFC.Digital.Service.AzureSearch.UnitTests
{
    public class DfcJobProfileSearchResultsManipulatorTests
    {
        public DfcJobProfileSearchResultsManipulatorTests()
        {
            JobProfileSearchResultsManipulator = new JobProfileSearchResultsManipulator();
        }

        private JobProfileSearchResultsManipulator JobProfileSearchResultsManipulator { get; set; }

        [Theory]
        [InlineData(true, null)]
        [InlineData(false, null)]
        [InlineData(true, "AltTitle")]
        public void ReorderForAlterantiveTitleNullTests(bool useNullSearchResults, string searchTerm)
        {
            var manipulatedResults = this.JobProfileSearchResultsManipulator.ReorderForAlterantiveTitle(useNullSearchResults ? null : GetDummySearchResults(), searchTerm);
            if (useNullSearchResults)
            {
                manipulatedResults.Should().BeNull();
            }
            else
            {
                manipulatedResults.Should().BeEquivalentTo(GetDummySearchResults());
            }
        }

        [Theory]
        [InlineData("doesNotExistAltTitle", "Title One")]
        [InlineData("existInJobTwoAltTitle", "Title Two")]
        [InlineData("existInJobOneAltTitle", "Title One")]
        public void ReorderForAlterantiveTitleTests(string searchTerm, string expectedFirstJobTitle)
        {
            var manipulatedResults = this.JobProfileSearchResultsManipulator.ReorderForAlterantiveTitle(GetDummySearchResults(), searchTerm);
            manipulatedResults.Results.First().ResultItem.Title.Should().Be(expectedFirstJobTitle);
        }

        public SearchResult<JobProfileIndex> GetDummySearchResults()
        {
            var dummyResults = new SearchResult<JobProfileIndex>
            {
                Count = 2
            };

            var results = new List<SearchResultItem<JobProfileIndex>>();
            var position1 = new SearchResultItem<JobProfileIndex> { Rank = 1, ResultItem = new JobProfileIndex() { Title = "Title One", AlternativeTitle = new List<string>() { "JP1AltTitle1", "existInJobOneAltTitle" } } };
            results.Add(position1);
            var position2 = new SearchResultItem<JobProfileIndex> { Rank = 2, ResultItem = new JobProfileIndex() { Title = "Title Two", AlternativeTitle = new List<string>() { "JP2AltTitle1", "existInJobTwoAltTitle" } } };
            results.Add(position2);
            dummyResults.Results = results;
            return dummyResults;
        }
    }
}
