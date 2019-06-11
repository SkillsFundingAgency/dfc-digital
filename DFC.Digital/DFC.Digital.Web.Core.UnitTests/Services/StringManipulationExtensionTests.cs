using System.Collections.Generic;
using Xunit;
using FluentAssertions;

namespace DFC.Digital.Web.Core.Tests
{
    public class StringManipulationExtensionTests
    {
        private const string RegexPattern = "(?:[^a-z0-9 ]|(?<=['\"])s)";

        [Theory]
        [InlineData("<script />", "script ")]
        [InlineData("??Maths@", "Maths")]
        [InlineData("", "")]
        [InlineData(null, null)]
        public void ReplaceSpecialCharactersTest(string searchTerm, string expectedSearchTerm)
        {
            //Act
            var result = StringManipulationExtension.ReplaceSpecialCharacters(searchTerm, RegexPattern);

            // Assert
            result.Should().Be(expectedSearchTerm);
        }



        [Theory]
        [MemberData(nameof(GetUrlEncodedStringTestsInput))]
        public void GetUrlEncodedStringTest(string input, string expectedOutput)
        {
             
             
            //Act
            var result = StringManipulationExtension.GetUrlEncodedString(input);

            //Assert
            result.Should().BeEquivalentTo(expectedOutput);
        }


        public static IEnumerable<object[]> GetUrlEncodedStringTestsInput()
        {
            yield return new object[]
            {
                "/courses-search-results?searchTerm=math<script>",
                "%2fcourses-search-results%3fsearchTerm%3dmath%3cscript%3e"
            };

            yield return new object[]
            {
                "/courses-search-results?searchTerm=language&provider=london",
                "%2fcourses-search-results%3fsearchTerm%3dlanguage%26provider%3dlondon"
            };

            yield return new object[]
            {
                "courses-search-results?searchTerm=language&provider=london&dfe1619Funded=1619&location=leeds&studymode=1&page=1",
                "courses-search-results%3fsearchTerm%3dlanguage%26provider%3dlondon%26dfe1619Funded%3d1619%26location%3dleeds%26studymode%3d1%26page%3d1"
            };

            yield return new object[]
            {
                "pageurl/itemid",
                "pageurl%2fitemid"
            };

            yield return new object[]
            {
                null,
                string.Empty
            };
        }
    }
}