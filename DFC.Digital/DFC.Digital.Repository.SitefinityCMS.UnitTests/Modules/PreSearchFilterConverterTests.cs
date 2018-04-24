using DFC.Digital.Data.Model;
using FakeItEasy;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Model;
using Xunit;

namespace DFC.Digital.Repository.SitefinityCMS.Modules.Tests
{
    public class PreSearchFilterConverterTests
    {
        private readonly IDynamicContentExtensions dynamicContentExtensions;
        private readonly DynamicContent dummyDynamicContent;

        public PreSearchFilterConverterTests()
        {
            dynamicContentExtensions = A.Fake<IDynamicContentExtensions>();
            dummyDynamicContent = A.Dummy<DynamicContent>();
        }

        [Fact]
        public void ConvertFromTest()
        {
            //Assign
            SetupCalls();
            var appConverter = new PreSearchFilterConverter<PreSearchFilter>(dynamicContentExtensions);

            //Act
            appConverter.ConvertFrom(dummyDynamicContent);

            //Assert
            A.CallTo(() => dynamicContentExtensions.GetFieldValue<Lstring>(A<DynamicContent>._, A<string>._))
                .MustHaveHappened();
            A.CallTo(() => dynamicContentExtensions.GetFieldValue<bool>(A<DynamicContent>._, A<string>._))
                .MustHaveHappened();
            A.CallTo(() => dynamicContentExtensions.GetFieldValue<decimal?>(A<DynamicContent>._, A<string>._))
                .MustHaveHappened();
            A.CallTo(() => dynamicContentExtensions.GetFieldValue<int>(A<DynamicContent>._, A<string>._))
                .MustNotHaveHappened();
        }

        private void SetupCalls()
        {
            A.CallTo(() => dynamicContentExtensions.GetFieldValue<Lstring>(A<DynamicContent>._, A<string>._))
                .Returns("test");
            A.CallTo(() => dynamicContentExtensions.GetFieldValue<bool>(A<DynamicContent>._, A<string>._))
                .Returns(false);
            A.CallTo(() => dynamicContentExtensions.GetFieldValue<decimal?>(A<DynamicContent>._, A<string>._))
                .Returns(10);
        }
    }
}