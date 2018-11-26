using DFC.Digital.Data.Model;
using FluentAssertions;
using Xunit;

namespace DFC.Digital.Service.AzureSearch.UnitTests
{
    public class DfcBuildExactMatchSearch
    {
        [Theory]
        [InlineData("", "")]
        [InlineData("term1", "")]
        [InlineData("term1 term2", "\"term1 term2\" ")]
        public void BuildExactMatchTest(string searchTerm, string expected)
        {
            var queryBuilder = new DfcSearchQueryBuilder();
            var result = queryBuilder.BuildExactMatchSearch(searchTerm);
            result.Should().Be(expected);
        }
    }
}
