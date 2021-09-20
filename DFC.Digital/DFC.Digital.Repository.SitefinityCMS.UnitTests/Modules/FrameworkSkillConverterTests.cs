using FakeItEasy;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Model;
using Xunit;

namespace DFC.Digital.Repository.SitefinityCMS.Modules.Tests
{
    public class FrameworkSkillConverterTests
    {
        private readonly IDynamicContentExtensions fakeDynamicContentExtensions;
        private readonly DynamicContent fakeDynamicContentItem;

        public FrameworkSkillConverterTests()
        {
            fakeDynamicContentExtensions = A.Fake<IDynamicContentExtensions>();
            fakeDynamicContentItem = A.Dummy<DynamicContent>();
        }

        [Fact(Skip = "LString throwing a null reference exception")]
        public void ConvertFromTest()
        {
            // Arrange
            SetupCalls();
            var frameworkSkillConverter =
                new FrameworkSkillConverter(fakeDynamicContentExtensions);

            //Act
            frameworkSkillConverter.ConvertFrom(fakeDynamicContentItem);

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