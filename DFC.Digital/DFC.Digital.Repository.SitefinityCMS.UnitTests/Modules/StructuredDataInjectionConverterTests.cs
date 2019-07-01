using FakeItEasy;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Model;
using Xunit;

namespace DFC.Digital.Repository.SitefinityCMS.Modules.Tests
{
    public class StructuredDataInjectionConverterTests
    {
        private readonly IDynamicContentExtensions fakeDynamicContentExtensions;
        private readonly DynamicContent fakeDynamicContentItem;

        public StructuredDataInjectionConverterTests()
        {
            fakeDynamicContentExtensions = A.Fake<IDynamicContentExtensions>();
            fakeDynamicContentItem = A.Dummy<DynamicContent>();
        }

        [Fact]
        public void ConvertFromTest()
        {
            // Arrange
            SetupCalls();
            var socSkillsMatrixConverter =
                new StructuredDataInjectionConverter(fakeDynamicContentExtensions);

            //Act
            socSkillsMatrixConverter.ConvertFrom(fakeDynamicContentItem);

            //Assert
            A.CallTo(() => fakeDynamicContentExtensions.GetFieldValue<Lstring>(A<DynamicContent>._, A<string>._))
                .MustHaveHappened(3, Times.Exactly);
        }

        private void SetupCalls()
        {
            A.CallTo(() => fakeDynamicContentExtensions.GetFieldValue<Lstring>(A<DynamicContent>._, A<string>._))
                .Returns("test");
        }
    }
}