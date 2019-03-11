using DFC.Digital.Repository.SitefinityCMS.Modules;
using FakeItEasy;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Model;
using Xunit;

namespace DFC.Digital.Repository.SitefinityCMS.UnitTests
{
    public class EmailTemplateConverterTests
    {
        private readonly IDynamicContentExtensions fakeDynamicContentExtensions;
        private readonly DynamicContent fakeDynamicContentItem;

        public EmailTemplateConverterTests()
        {
            fakeDynamicContentExtensions = A.Fake<IDynamicContentExtensions>();
            fakeDynamicContentItem = A.Dummy<DynamicContent>();
        }

        [Fact]
        public void ConvertFromTest()
        {
            // Arrange
            SetupCalls();
            var emailTemplateConverter =
                new EmailTemplateConverter(fakeDynamicContentExtensions);

            //Act
            emailTemplateConverter.ConvertFrom(fakeDynamicContentItem);

            //Assert
            A.CallTo(() => fakeDynamicContentExtensions.GetFieldValue<Lstring>(A<DynamicContent>._, A<string>._))
                .MustHaveHappened(5, Times.Exactly);
            A.CallTo(() => fakeDynamicContentExtensions.GetContentWithoutHtmlTags(A<DynamicContent>._, A<string>._))
                .MustHaveHappened(1, Times.Exactly);
        }

        private void SetupCalls()
        {
            A.CallTo(() => fakeDynamicContentExtensions.GetFieldValue<Lstring>(A<DynamicContent>._, A<string>._))
                .Returns("test");
            A.CallTo(() => fakeDynamicContentExtensions.GetContentWithoutHtmlTags(A<DynamicContent>._, A<string>._))
                .Returns("test");
        }
    }
}
