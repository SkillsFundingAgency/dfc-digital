using DFC.Digital.Web.Core;
using FluentAssertions;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.CourseModule.UnitTests
{
    public class ReplaceSpecialCharactersExtensionTests
    {
        #region Action Tests

        [Theory]
        [InlineData("<script />", "script ")]
        [InlineData("??Maths@", "Maths")]
        [InlineData("", "")]
        [InlineData(null, null)]
        public void ReplaceSpecialCharactersTest(string searchTerm, string expectedSearchTerm)
        {
            var dummyReturnedCharacter = SpecialCharacterExtensions.ReplaceSpecialCharacters(searchTerm);

            // Assert
            dummyReturnedCharacter.Should().Be(expectedSearchTerm);
        }

        #endregion Action Tests
    }
}
