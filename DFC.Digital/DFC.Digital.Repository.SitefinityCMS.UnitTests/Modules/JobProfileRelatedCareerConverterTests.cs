using FakeItEasy;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Model;
using Xunit;

namespace DFC.Digital.Repository.SitefinityCMS.Modules.Tests
{
    public class JobProfileRelatedCareerConverterTests
    {
        private readonly IDynamicContentExtensions dynamicContentExtensions;
        private readonly DynamicContent dummyDynamicContent;

        public JobProfileRelatedCareerConverterTests()
        {
            dynamicContentExtensions = A.Fake<IDynamicContentExtensions>();
            dummyDynamicContent = A.Dummy<DynamicContent>();
        }

        [Fact(Skip = "LString throwing a null reference exception")]
        public void ConvertFromTest()
        {
            //Assign
            SetupCalls();
            var appConverter = new JobProfileRelatedCareerConverter(dynamicContentExtensions);

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