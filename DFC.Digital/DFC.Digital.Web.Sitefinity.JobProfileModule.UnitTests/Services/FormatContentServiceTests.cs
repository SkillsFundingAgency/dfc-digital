using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.UnitTests
{
    public class FormatContentServiceTests
    {
        [Theory]
        [InlineData("intro", 0, "and", "")]
        [InlineData("intro", 1, "and", "intro item.")]
        [InlineData("intro", 2, "and", "intro item and item.")]
        [InlineData("intro", 3, "and", "intro item, item and item.")]
        public void GetParagraphTextTest(string leadingText, int numberOfItems, string conjunction, string expected)
        {
            // Arrange
           var formatContentService = new FormatContentService();

            // Act
             var result = formatContentService.GetParagraphText(leadingText, GetRelatedItems(numberOfItems), conjunction);

            // Assert
            result.Should().Be(expected);
        }

        private IEnumerable<string> GetRelatedItems(int numberOfItems)
        {
            for (int n = 0; n < numberOfItems; n++)
            {
              yield return "item";
            }
        }
    }
}