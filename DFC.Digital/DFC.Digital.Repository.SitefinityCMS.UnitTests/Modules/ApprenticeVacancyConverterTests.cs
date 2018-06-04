using FakeItEasy;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Model;
using Xunit;

namespace DFC.Digital.Repository.SitefinityCMS.Modules.Tests
{
    public class ApprenticeVacancyConverterTests
    {
        private readonly IDynamicContentExtensions dynamicContentExtensions;
        private readonly DynamicContent dummyDynamicContent;

        public ApprenticeVacancyConverterTests()
        {
            dynamicContentExtensions = A.Fake<IDynamicContentExtensions>();
            dummyDynamicContent = A.Dummy<DynamicContent>();
        }

        [Fact]
        public void ConvertFromTest()
        {
            //Assign
            SetupCalls();
            var appConverter = new ApprenticeVacancyConverter(dynamicContentExtensions);

            //Act
            appConverter.ConvertFrom(dummyDynamicContent);

            //Assert
            A.CallTo(() => dynamicContentExtensions.GetFieldValue<Lstring>(A<DynamicContent>._, A<string>._))
                .MustHaveHappened();
        }

        private void SetupCalls()
        {
            A.CallTo(() => dynamicContentExtensions.GetFieldValue<Lstring>(A<DynamicContent>._, A<string>._))
                .Returns("test");
        }
    }
}