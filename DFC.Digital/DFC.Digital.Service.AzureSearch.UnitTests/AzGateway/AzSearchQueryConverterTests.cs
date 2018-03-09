using DFC.Digital.AutomationTest.Utilities;
using DFC.Digital.Data.Model;
using FluentAssertions;
using Microsoft.Azure.Search.Models;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DFC.Digital.Service.AzureSearch.AzGateway.Tests
{
    public class AzSearchQueryConverterTests
    {
        [Fact]
        public void BuildSearchParametersTest()
        {
            var properties = new SearchProperties
            {
                Count = 2,
                FilterBy = "filterby",
                Page = 3,
                SearchFields = new[] { "Field1", "Field2" },
                UseRawSearchTerm = true
            };

            var queryConverter = new AzSearchQueryConverter();
            var result = queryConverter.BuildSearchParameters(properties);

            result.SearchMode.Should().Be(SearchMode.Any);                      //SearchMode = SearchMode.Any
            result.IncludeTotalResultCount.Should().BeTrue();                   //IncludeTotalResultCount = true
            result.SearchFields.Should().BeEquivalentTo(properties.SearchFields);  //SearchFields = properties.SearchFields,
            result.Filter.Should().Be(properties.FilterBy);                     //Filter = properties.FilterBy,
            result.Skip.Should().Be(4);                                         //Skip = (properties.Page - 1) * properties.Count,
            result.Top.Should().Be(2);                                          //Top = properties.Count,
            result.QueryType.Should().Be(QueryType.Full);                       //QueryType = QueryType.Full,
        }

        [Fact]
        public void ConvertResultTest()
        {
            var properties = new SearchProperties
            {
                Count = 2,
                FilterBy = "filterby",
                Page = 3,
                SearchFields = new[] { "Field1", "Field2" },
                UseRawSearchTerm = true
            };

            var indexResult = new DocumentSearchResult<JobProfileIndex>
            {
                Count = 101,
                Results = new List<Microsoft.Azure.Search.Models.SearchResult<JobProfileIndex>>
                {
                    new Microsoft.Azure.Search.Models.SearchResult<JobProfileIndex>
                    {
                        Score = 1,
                        Document = DummyJobProfileIndex.GenerateJobProfileIndexDummy("one")
                    },
                    new Microsoft.Azure.Search.Models.SearchResult<JobProfileIndex>
                    {
                        Score = 1.2,
                        Document = DummyJobProfileIndex.GenerateJobProfileIndexDummy("two")
                    },
                }
            };

            var queryConverter = new AzSearchQueryConverter();
            var result = queryConverter.ConvertToSearchResult(indexResult, properties);

            result.Count.Should().Be(indexResult.Count);                                            //Count = result.Count,
            result.Results.First().Rank.Should().Be(5);                                             //Results = result.ToSearchResultItems(properties)
            result.Results.First().ResultItem
                .Should().BeEquivalentTo(DummyJobProfileIndex.GenerateJobProfileIndexDummy("one"));    //Results = result.ToSearchResultItems(properties)

            result.Results.Last().Rank.Should().Be(6);                                              //Results = result.ToSearchResultItems(properties)
            result.Results.Last().ResultItem
                .Should().BeEquivalentTo(DummyJobProfileIndex.GenerateJobProfileIndexDummy("two"));    //Results = result.ToSearchResultItems(properties)
        }

        [Theory]
        [InlineData(0, false)]
        [InlineData(7, false)]
        [InlineData(0, true)]
        [InlineData(23, true)]
        public void BuildSuggestParametersTest(int count, bool fuzzy)
        {
            var props = new SuggestProperties
            {
                MaxResultCount = count,
                UseFuzzyMatching = fuzzy
            };

            var queryConverter = new AzSearchQueryConverter();
            var result = queryConverter.BuildSuggestParameters(props);

            result.Top.Should().Be(count);
            result.UseFuzzyMatching.Should().Be(fuzzy);
        }

        [Fact]
        public void ConvertToSuggestionResultTest()
        {
            var suggestResult = new DocumentSuggestResult<JobProfileIndex>
            {
                Coverage = 1,
                Results = new List<SuggestResult<JobProfileIndex>>
                {
                    new SuggestResult<JobProfileIndex>
                    {
                        Document = DummyJobProfileIndex.GenerateJobProfileIndexDummy("one"),
                        Text = "one"
                    },
                    new SuggestResult<JobProfileIndex>
                    {
                        Document = DummyJobProfileIndex.GenerateJobProfileIndexDummy("two"),
                        Text = "two"
                    }
                }
            };

            var properties = new SuggestProperties
            {
                MaxResultCount = 5,
                UseFuzzyMatching = true
            };

            var queryConverter = new AzSearchQueryConverter();
            var result = queryConverter.ConvertToSuggestionResult(suggestResult, properties);

            result.Coverage.Should().Be(1);
            result.Results.Count().Should().Be(suggestResult.Results.Count());
            result.Results.First().Index.Should().BeEquivalentTo(DummyJobProfileIndex.GenerateJobProfileIndexDummy("one"));
            result.Results.First().MatchedSuggestion.Should().Be("one");
            result.Results.Last().Index.Should().BeEquivalentTo(DummyJobProfileIndex.GenerateJobProfileIndexDummy("two"));
            result.Results.Last().MatchedSuggestion.Should().Be("two");
        }
    }
}