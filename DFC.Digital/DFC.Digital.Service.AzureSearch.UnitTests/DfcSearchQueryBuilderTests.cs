using DFC.Digital.Data.Model;
using FluentAssertions;
using Xunit;

namespace DFC.Digital.Service.AzureSearch.UnitTests
{
    public class DfcSearchQueryBuilderTests
    {
        [Theory]
        [InlineData("test", null, "/.*test.*/ test~")]
        [InlineData("test1 test2", null, "/.*test1.*/ test1~ /.*test2.*/ test2~")]
        [InlineData("test1 test-2", null, "/.*test1.*/ test1~ test-2")]
        [InlineData("test", false, "/.*test.*/ test~")]
        [InlineData("test1 test2", false, "/.*test1.*/ test1~ /.*test2.*/ test2~")]
        [InlineData("test1 test-2", false, "/.*test1.*/ test1~ test-2")]
        [InlineData("test", true, "test")]
        [InlineData("test1 test2", true, "test1 test2")]
        [InlineData("test1 test-2", true, "test1 test-2")]
        public void BuildContainPartialSearchTest(string cleanedSearchTerm, bool? isRaw, string expectation)
        {
            var properties = isRaw is null ? null : new SearchProperties { UseRawSearchTerm = isRaw.Value };
            var testObject = new DfcSearchQueryBuilder();
            var result = testObject.BuildContainPartialSearch(cleanedSearchTerm, properties);

            result.Should().Be(expectation);
        }

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
        [InlineData("term Children's", "term Children's", false)]
        [InlineData("<term Children's>", "term Children's", false)]
        public void RemoveSpecialCharactersFromTheSearchTerm(string searchTerm, string expected, bool shouldUseRaw)
        {
            SearchProperties properties = shouldUseRaw ? new SearchProperties { UseRawSearchTerm = true } : null;
            var testObject = new DfcSearchQueryBuilder();
            var result = testObject.RemoveSpecialCharactersFromTheSearchTerm(searchTerm, properties);
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("term", "/.*term.*/")]
        [InlineData("term~nurse", "/.*termnurse.*/")]
        [InlineData("term - nurse", "/.*term.*//.*nurse.*/")]
        [InlineData("term-nurse-vertinary", "term-nurse-vertinary")]
        [InlineData("term nurs~ term-nurse-vertinary", "/.*term.*//.*nurs.*/term-nurse-vertinary")]
        [InlineData("term~", "/.*term.*/")]
        [InlineData("term^nurse", "/.*termnurse.*/")]
        [InlineData("+term +nurse", "/.*term.*//.*nurse.*/")]
        [InlineData("term?nurse", "/.*termnurse.*/")]
        [InlineData("/[term]nurse/", "/.*termnurse.*/")]
        [InlineData("term !nurse", "/.*term.*//.*nurse.*/")]
        [InlineData("term + nurse", "/.*term.*//.*nurse.*/")]
        [InlineData("term or nurse", "/.*term.*//.*or.*//.*nurse.*/")]
        [InlineData("term && nurse", "/.*term.*//.*nurse.*/")]
        [InlineData("term & nurse", "/.*term.*//.*nurse.*/")]
        [InlineData("term || nurse", "/.*term.*//.*nurse.*/")]
        [InlineData("term and nurse", "/.*term.*//.*and.*//.*nurse.*/")]
        [InlineData("(term)", "/.*term.*/")]
        [InlineData("term Children's", "/.*term.*//.*Children's.*/")]
        [InlineData("<term Children's>", "/.*term.*//.*Children's.*/")]
        public void BuiBuildContainPartialSearchTest(string searchTerm, string expected)
        {
            var testObject = new DfcSearchQueryBuilder();
            var searchTermResult = testObject.RemoveSpecialCharactersFromTheSearchTerm(searchTerm, new SearchProperties() { UseRawSearchTerm = false });
            var outputWithContainsWildCard = testObject.BuildContainPartialSearch(searchTermResult, new SearchProperties());
            outputWithContainsWildCard.Should().Be(expected);
        }

        [Theory]
        [InlineData("plumber", "plumb")] //er
        [InlineData("offenders", "offend")] //ers
        [InlineData("plumbing engineer", "plumb engine")] //ing
        [InlineData("Business development manager", "Business develop manag")] //"ment"
        [InlineData("installation and plumbing engineer", "install plumb engine")] //ation
        [InlineData("director or clock repairer", "direct clock repair")] //or
        [InlineData("pharmacology ecology", "pharmacolo ecolo")] //ology
        [InlineData("optometry", "opto")] //metry
        [InlineData("dietetics", "dietet")] //ics
        [InlineData("laundrette", "laundr")] //ette
        [InlineData("performance", "perform")] //ance
        [InlineData("fisheries", "fisher")] //ies
        [InlineData("diplomacy", "diplo")] //macy
        [InlineData("director and clock repairer", "direct clock repair")] //and
        [InlineData("Hydrotherapy", "Hydrothera")] //therapy
        public void TrimSuffixesTest(string searchTerm, string expected)
        {
            var testObject = new DfcSearchQueryBuilder();
            var searchTermResult = testObject.RemoveSpecialCharactersFromTheSearchTerm(searchTerm, new SearchProperties() { UseRawSearchTerm = false });
            var trimmedOutput = testObject.TrimCommonWordsAndSuffixes(searchTermResult, new SearchProperties());
            trimmedOutput.Should().Be(expected);
        }

        [Theory]
        [InlineData("pharmacology", "pharmac", "pharmac pharmacolo")] //ology
        [InlineData("ecology", "ecology", "ecolo")] //ology
        [InlineData("biology", "biology", "biolo")] //ology
        [InlineData("criminology", "crimin", "crimin criminolo")] //ology
        [InlineData("", "crimin", "crimin")] //ology
        [InlineData("criminology", "", "criminolo")] //ology
        public void SpecialologiesTest(string searchTerm, string replacedSuffixTerm, string expected)
        {
            var testObject = new DfcSearchQueryBuilder();
            var searchTermResult = testObject.Specialologies(searchTerm, replacedSuffixTerm);
            searchTermResult.Should().Be(expected);
        }

        [Theory]
        [InlineData("and", true)]
        [InlineData("the", false)]
        [InlineData("or", true)]
        [InlineData("is", false)]
        [InlineData("as", true)]
        [InlineData("are", false)]
        [InlineData("if", true)]
        [InlineData("to", false)]
        [InlineData("also", true)]
        [InlineData("but", true)]
        [InlineData("not", true)]
        public void IsCommonCojoinginWordTest(string searchTerm, bool expected)
        {
            var testObject = new DfcSearchQueryBuilder();
            var searchTermResult = testObject.IsCommonCojoinginWord(searchTerm?.ToLower());
            searchTermResult.Should().Be(expected);
        }

        [Theory]
        [InlineData("plumber", "plumb")] //er
        [InlineData("offenders", "offend")] //ers
        [InlineData("plumbing", "plumb")] //ing
        [InlineData("management", "manage")] //"ment"
        [InlineData("installation", "install")] //ation
        [InlineData("optometry", "opto")] //metry
        [InlineData("dietetics", "dietet")] //ics
        [InlineData("laundrette", "laundr")] //ette
        [InlineData("performance", "perform")] //ance
        [InlineData("fisheries", "fisher")] //ies
        [InlineData("diplomacy", "diplo")] //macy
        public void TrimSuffixFromSingleWordTest(string searchTerm, string expected)
        {
            var testObject = new DfcSearchQueryBuilder();
            var searchTermResult = testObject.TrimSuffixFromSingleWord(searchTerm);
            searchTermResult.Should().Be(expected);
        }
    }
}