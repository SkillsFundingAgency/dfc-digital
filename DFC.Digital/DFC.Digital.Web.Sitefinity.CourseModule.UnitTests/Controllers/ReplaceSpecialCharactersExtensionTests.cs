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
        public void ReplaceSpecialCharactersTest(string searchTerm, string expectedSearchTerm)
        {
            var dummyReturnedCharacter = ReplaceSpecialCharactersExtension.ReplaceSpecialCharacter(searchTerm);

            // Assert
            dummyReturnedCharacter.Should().Be(expectedSearchTerm);
        }

        #endregion Action Tests
    }
}
