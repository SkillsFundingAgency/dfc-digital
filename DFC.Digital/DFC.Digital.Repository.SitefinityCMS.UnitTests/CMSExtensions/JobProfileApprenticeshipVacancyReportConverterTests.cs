using DFC.Digital.Data.Model;
using FakeItEasy;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Model;
using Xunit;

namespace DFC.Digital.Repository.SitefinityCMS.CMSExtensions.Tests
{
    public class JobProfileApprenticeshipVacancyReportConverterTests
    {
        private readonly IDynamicContentExtensions dummyDynamicContentExtensions;
        private readonly IDynamicModuleConverter<JobProfileReport> dummyJobProfileConverter;
        private readonly IDynamicModuleConverter<SocCodeReport> dummySocCodeReportConverter;
        private readonly DynamicContent dummyDynamicContent;

        public JobProfileApprenticeshipVacancyReportConverterTests()
        {
            dummyJobProfileConverter = A.Fake<IDynamicModuleConverter<JobProfileReport>>(x => x.Strict());
            dummyDynamicContent = A.Dummy<DynamicContent>();
            dummySocCodeReportConverter = A.Fake<IDynamicModuleConverter<SocCodeReport>>(x => x.Strict());
            dummyDynamicContentExtensions = A.Fake<IDynamicContentExtensions>(x => x.Strict());
        }

        [Fact]
        public void ConvertFromTest()
        {
            //Assign
            SetupCalls();
            var jobProfileApprenticeshipVacancyReportConverter = new JobProfileApprenticeshipVacancyReportConverter(dummySocCodeReportConverter, dummyJobProfileConverter, dummyDynamicContentExtensions);

            //Act
            jobProfileApprenticeshipVacancyReportConverter.ConvertFrom(dummyDynamicContent);

            //Assert
            A.CallTo(() => dummyJobProfileConverter.ConvertFrom(A<DynamicContent>._))
                .MustHaveHappened();
            A.CallTo(() => dummyDynamicContentExtensions.GetFieldValue<DynamicContent>(A<DynamicContent>._, A<string>._))
                .MustHaveHappened();
            A.CallTo(() => dummySocCodeReportConverter.ConvertFrom(A<DynamicContent>._)).MustHaveHappened();
        }

        private void SetupCalls()
        {
            A.CallTo(() => dummySocCodeReportConverter.ConvertFrom(A<DynamicContent>._)).Returns(new SocCodeReport());
            A.CallTo(() => dummyJobProfileConverter.ConvertFrom(A<DynamicContent>._))
                .Returns(new JobProfileReport());
            A.CallTo(() => dummyDynamicContentExtensions.GetFieldValue<DynamicContent>(A<DynamicContent>._, A<string>._))
                .Returns(dummyDynamicContent);
        }
    }
}