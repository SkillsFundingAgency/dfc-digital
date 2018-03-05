using DFC.Digital.Data.Model;
using FluentAssertions;
using Xunit;

namespace DFC.Digital.Service.AzureSearch.Tests
{
    public class DfcSearchQueryBuilderTests
    {
        [Theory]
        [InlineData("term1", null, "(FilterableTitle eq 'term1' or FilterableAlternativeTitle eq 'term1')")]
        [InlineData("term1", "*", "*")]
        public void BuildExclusiveExactMatchTest(string searchTerm, string filter, string expected)
        {
            var properties = new SearchProperties
            {
                FilterBy = filter,
            };

            var testObject = new DfcSearchQueryBuilder();
            var result = testObject.BuildExclusiveExactMatch(searchTerm, properties);

            result.FilterBy.Should().Be(expected);
        }

        [Theory]
        [InlineData("term1", null, "(FilterableTitle ne 'term1' and FilterableAlternativeTitle ne 'term1')", 2, 8)]
        [InlineData("term1", "*", "*", 10, 0)]
        public void BuildExclusivePartialMatchTest(string searchTerm, string filter, string expected, int exactMatchCount, int expectedPartialMatchCount)
        {
            var properties = new SearchProperties
            {
                FilterBy = filter,
            };

            var testObject = new DfcSearchQueryBuilder();
            var result = testObject.BuildExclusivePartialMatch(searchTerm, properties, exactMatchCount);

            result.FilterBy.Should().Be(expected);
            result.Count.Should().Be(expectedPartialMatchCount);
        }

        [Theory]
        [InlineData("term1", "term1", null)]
        [InlineData("*", "\\*", null)]
        [InlineData("*", "*", true)]
        public void CleanSearchTermTest(string searchTerm, string expected, bool? shouldUseRaw)
        {
            SearchProperties properties = shouldUseRaw == true ? new SearchProperties { UseRawSearchTerm = true } : null;
            var testObject = new DfcSearchQueryBuilder();
            var result = testObject.EscapeSpecialCharactersInSearchTerm(searchTerm, properties);

            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("term~nurse", "termnurse", false)]
        [InlineData("term~nurse", "term~nurse", true)]
        [InlineData("term-nurse-vertinary", "term-nurse-vertinary", false)]
        [InlineData("term-nurse-vertinary", "term-nurse-vertinary", true)]
        [InlineData("term~", "term", false)]
        [InlineData("term^nurse", "termnurse", false)]
        [InlineData("+term +nurse", "term nurse", false)]
        [InlineData("term?nurse", "termnurse", false)]
        [InlineData("/[term]nurse/", "termnurse", false)]
        [InlineData("term !nurse", "term nurse", false)]
        [InlineData("term - nurse", "term nurse", false)]
        [InlineData("term + nurse", "term nurse", false)]
        [InlineData("term or nurse", "term or nurse", false)]
        [InlineData("term && nurse", "term nurse", false)]
        [InlineData("term & nurse", "term nurse", false)]
        [InlineData("term || nurse", "term nurse", false)]
        [InlineData("term and nurse", "term and nurse", false)]
        [InlineData("(term)", "term", false)]
        [InlineData("term Children's", "term children's", false)]
        [InlineData("<term Children's>", "term children's", false)]
        public void RemoveSpecialCharactersFromTheSearchTerm(string searchTerm, string expected, bool shouldUseRaw)
        {
            SearchProperties properties = shouldUseRaw ? new SearchProperties { UseRawSearchTerm = true } : null;
            var testObject = new DfcSearchQueryBuilder();
            var result = testObject.RemoveSpecialCharactersFromTheSearchTerm(searchTerm, properties);
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("term", "/.*term.*/ term~")]
        [InlineData("term~nurse", "/.*termnurse.*/ termnurse~")]
        [InlineData("term - nurse", "/.*term.*/ term~/.*nurse.*/ nurse~")]
        [InlineData("term-nurse-vertinary", "term-nurse-vertinary")]
        [InlineData("term nurs~ term-nurse-vertinary", "/.*term.*/ term~/.*nurs.*/ nurs~term-nurse-vertinary")]
        [InlineData("term~", "/.*term.*/ term~")]
        [InlineData("term^nurse", "/.*termnurse.*/ termnurse~")]
        [InlineData("+term +nurse", "/.*term.*/ term~/.*nurse.*/ nurse~")]
        [InlineData("term?nurse", "/.*termnurse.*/ termnurse~")]
        [InlineData("/[term]nurse/", "/.*termnurse.*/ termnurse~")]
        [InlineData("term !nurse", "/.*term.*/ term~/.*nurse.*/ nurse~")]
        [InlineData("term + nurse", "/.*term.*/ term~/.*nurse.*/ nurse~")]
        [InlineData("term or nurse", "/.*term.*/ term~/.*or.*/ or~/.*nurse.*/ nurse~")]
        [InlineData("term && nurse", "/.*term.*/ term~/.*nurse.*/ nurse~")]
        [InlineData("term & nurse", "/.*term.*/ term~/.*nurse.*/ nurse~")]
        [InlineData("term || nurse", "/.*term.*/ term~/.*nurse.*/ nurse~")]
        [InlineData("term and nurse", "/.*term.*/ term~/.*and.*/ and~/.*nurse.*/ nurse~")]
        [InlineData("(term)", "/.*term.*/ term~")]
        [InlineData("term Children's", "/.*term.*/ term~/.*children's.*/ children's~")]
        [InlineData("<term Children's>", "/.*term.*/ term~/.*children's.*/ children's~")]
        public void BuiBuildContainPartialSearchTest(string searchTerm, string expected)
        {
            var testObject = new DfcSearchQueryBuilder();
            var searchTermResult = testObject.RemoveSpecialCharactersFromTheSearchTerm(searchTerm, new SearchProperties() { UseRawSearchTerm = false });
            var outputWithContainsWildCard = testObject.BuildContainPartialSearch(searchTermResult, new SearchProperties());
            outputWithContainsWildCard.Should().Be(expected);
        }
    }
}