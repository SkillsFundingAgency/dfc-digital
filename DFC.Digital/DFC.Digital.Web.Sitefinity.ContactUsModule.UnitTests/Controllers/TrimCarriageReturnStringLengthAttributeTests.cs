using DFC.Digital.Web.Core;
using FluentAssertions;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.ContactUsModule.UnitTests
{
    public class TrimCarriageReturnStringLengthAttributeTests
    {
        #region Action Tests

        [Theory]
        [InlineData("\r\n", 1, true)]
        [InlineData("\n", 1, true)]
        [InlineData("\t\n", 1, false)]
        public void TrimCarriageReturnTest(string charLimiter, int maxCharLimit, bool expected)
        {
            var dummyReturnAttribute = new TrimCarriageReturnStringLengthAttribute(maxCharLimit);

            //Act
            var trimmedResult = dummyReturnAttribute.IsValid(charLimiter);

            //Assert
            trimmedResult.Should().Be(expected);
        }

        #endregion Action Tests
    }
}
