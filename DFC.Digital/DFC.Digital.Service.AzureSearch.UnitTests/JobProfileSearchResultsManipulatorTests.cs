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
        [InlineData("dummyTitleTest5", 1, "dummyTitleTest5")]
        [InlineData("dummyTitleTest5", 2, "dummyTitleTest1")]
        public void ReorderTitleTest(string searchTitle, int page, string expectedFirstResult)
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
            result.Results.First().ResultItem.Title.Should().Be(expectedFirstResult);
        }

        [Theory]
        [InlineData("dummyAlternativeTitleTest5", 1, "dummyAlternativeTitleTest5")]
        [InlineData("dummyAlternativeTitleTest5", 2, "dummyAlternativeTitleTest1")]
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