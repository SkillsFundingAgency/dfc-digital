using DFC.Digital.AutomationTest.Utilities;
using DFC.Digital.Data.Model;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.Services.Tests
{
    public class JobProfileSearchResultsManipulatorTests
    {
        [Theory]
        [InlineData("dummyTitletest5", 1, "dummyTitletest5")]
        [InlineData("dummyTitletest5", 2, "dummyTitletest1")]
        public void ReorderTitleTest(string searchTitle, int page, string expectedFirstResult)
        {
            var mannipulator = new JobProfileSearchResultsManipulator();
            SearchResult<Data.Model.JobProfileIndex> data = new SearchResult<JobProfileIndex>
            {
                Results = DummyJobProfileIndex.GenerateJobProfileResultItemDummyCollection("test", 10, 1)
            };

            SearchProperties searchProperties = new SearchProperties
            {
                Page = page
            };

            var result = mannipulator.Reorder(data, searchTitle, searchProperties);
            result.Results.First().ResultItem.Title.Should().Be(expectedFirstResult);
        }

        [Theory]
        [InlineData("dummyAlternativeTitle5", 1, "dummyAlternativeTitle5")]
        [InlineData("dummyAlternativeTitle5", 2, "dummyAlternativeTitle1")]
        public void ReorderAlternativeTitleTest(string searchTitle, int page, string expectedFirstResult)
        {
            var mannipulator = new JobProfileSearchResultsManipulator();
            SearchResult<Data.Model.JobProfileIndex> data = new SearchResult<JobProfileIndex>
            {
                Results = DummyJobProfileIndex.GenerateJobProfileResultItemDummyCollection("Test", 10, 1)
            };

            SearchProperties searchProperties = new SearchProperties
            {
                Page = page
            };

            var result = mannipulator.Reorder(data, searchTitle, searchProperties);
            result.Results.First().ResultItem.AlternativeTitle.First().Should().Be(expectedFirstResult);
        }

        [Fact]
        public void ReorderNullResultTest()
        {
            var mannipulator = new JobProfileSearchResultsManipulator();
            SearchResult<Data.Model.JobProfileIndex> data = new SearchResult<JobProfileIndex>
            {
                Results = null
            };

            SearchProperties searchProperties = new SearchProperties
            {
                Page = 1
            };

            var result = mannipulator.Reorder(data, "test", searchProperties);
            result.Results.Should().BeEquivalentTo(Enumerable.Empty<SearchResultItem<JobProfileIndex>>());
        }

        [Fact]
        public void ReorderNullDataTest()
        {
            var mannipulator = new JobProfileSearchResultsManipulator();
            SearchProperties searchProperties = new SearchProperties
            {
                Page = 1
            };

            var result = mannipulator.Reorder(null, "test", searchProperties);
            result.Results.Should().BeEquivalentTo(Enumerable.Empty<SearchResultItem<JobProfileIndex>>());
        }
    }
}