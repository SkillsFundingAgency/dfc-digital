using DFC.Digital.Data.Interfaces;
using FakeItEasy;
using System;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Model;
using Xunit;

namespace DFC.Digital.Repository.SitefinityCMS.CMSExtensions.Tests
{
    public class CmsReportItemConverterTests
    {
        private readonly IDynamicContentExtensions dummyDynamicContentExtensions;
        private readonly IUserRepository dummyUserRepository;
        private readonly DynamicContent dummyDynamicContent;

        public CmsReportItemConverterTests()
        {
            dummyDynamicContentExtensions = A.Fake<IDynamicContentExtensions>(x => x.Strict());
            dummyUserRepository = A.Fake<IUserRepository>(x => x.Strict());
            dummyDynamicContent = A.Dummy<DynamicContent>();
        }

        [Fact]
        public void ConvertFromTest()
        {
            //Assign
            SetupCalls();
            var cmsReportItemConverter = new CmsReportItemConverter(dummyDynamicContentExtensions, dummyUserRepository);

            //Act
            cmsReportItemConverter.ConvertFrom(dummyDynamicContent);

            //Assert
            A.CallTo(() => dummyDynamicContentExtensions.GetFieldValue<Lstring>(A<DynamicContent>._, A<string>._))
                .MustHaveHappened();
            A.CallTo(() => dummyDynamicContentExtensions.GetFieldValue<DateTime>(A<DynamicContent>._, A<string>._))
                .MustHaveHappened();
            A.CallTo(() => dummyDynamicContentExtensions.GetFieldValue<Guid>(A<DynamicContent>._, A<string>._))
                .Returns(Guid.Empty);
            A.CallTo(() => dummyUserRepository.GetUserNameById(A<Guid>._)).MustHaveHappened();
        }

        private void SetupCalls()
        {
            A.CallTo(() => dummyDynamicContentExtensions.GetFieldValue<Lstring>(A<DynamicContent>._, A<string>._))
                .Returns("test");
            A.CallTo(() => dummyDynamicContentExtensions.GetFieldValue<DateTime>(A<DynamicContent>._, A<string>._))
                .Returns(DateTime.Now);
            A.CallTo(() => dummyDynamicContentExtensions.GetFieldValue<Guid>(A<DynamicContent>._, A<string>._))
                .Returns(Guid.Empty);
            A.CallTo(() => dummyUserRepository.GetUserNameById(A<Guid>._)).Returns("user name");
        }
    }
}