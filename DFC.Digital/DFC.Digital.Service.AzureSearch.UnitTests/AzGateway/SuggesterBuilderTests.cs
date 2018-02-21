using FluentAssertions;
using Xunit;

namespace DFC.Digital.Service.AzureSearch.Tests
{
    public class SuggesterBuilderTests
    {
        [Fact]
        public void BuildForTypeTest()
        {
            var suggestionBuilder = new SuggesterBuilder();
            var result = suggestionBuilder.BuildForType<TestSuggestBuilder>();
            result.Should().Contain(new[] { "PropertyOne", "PropertyThree" }).And.NotContain(new[] { "PropertyTwo" });
        }
    }
}