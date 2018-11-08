using AutoMapper;
using DFC.Digital.Data.Model;
using DFC.Digital.Repository.SitefinityCMS.Modules;
using DFC.Digital.Web.Sitefinity.CmsExtensions;
using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Model;
using Xunit;

namespace DFC.Digital.Repository.SitefinityCMS.CMSExtensions.Tests
{
    public class ApprenticeshipReportConverterTests
    {
        private readonly IDynamicContentExtensions dummyDynamicContentExtensions;
        private readonly IDynamicModuleConverter<CmsReportItem> dummyCmsReportIteModuleConverter;
        private readonly IDynamicModuleConverter<SocCodeReport> dummySocCodeReportConverter;
        private readonly IMapper mapper;
        private readonly DynamicContent dummyDynamicContent;

        public ApprenticeshipReportConverterTests()
        {
            dummyCmsReportIteModuleConverter = A.Fake<IDynamicModuleConverter<CmsReportItem>>(x => x.Strict());
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CmsExtensionsAutoMapperProfile>();
            });
            mapper = config.CreateMapper();
            dummyDynamicContent = A.Dummy<DynamicContent>();
            dummySocCodeReportConverter = A.Fake<IDynamicModuleConverter<SocCodeReport>>(x => x.Strict());
            dummyDynamicContentExtensions = A.Fake<IDynamicContentExtensions>(x => x.Strict());
        }

        [Fact]
        public void ConvertFromTest()
        {
            //Assign
            SetupCalls();
            var socCodeReportConverter = new ApprenticeshipReportConverter(dummySocCodeReportConverter, dummyDynamicContentExtensions, dummyCmsReportIteModuleConverter, mapper);

            //Act
            socCodeReportConverter.ConvertFrom(dummyDynamicContent);

            //Assert
            A.CallTo(() => dummyCmsReportIteModuleConverter.ConvertFrom(A<DynamicContent>._))
                .MustHaveHappened();
            A.CallTo(() => dummyDynamicContentExtensions.GetFieldValue<DynamicContent>(A<DynamicContent>._, A<string>._))
                .MustHaveHappened();
            A.CallTo(() => dummySocCodeReportConverter.ConvertFrom(A<DynamicContent>._)).MustHaveHappened();
        }

        private void SetupCalls()
        {
            A.CallTo(() => dummySocCodeReportConverter.ConvertFrom(A<DynamicContent>._)).Returns(new SocCodeReport());
            A.CallTo(() => dummyCmsReportIteModuleConverter.ConvertFrom(A<DynamicContent>._))
                .Returns(new CmsReportItem
                {
                    DateCreated = DateTime.Now,
                    UrlName = nameof(CmsReportItem.UrlName),
                    LastModifiedBy = nameof(CmsReportItem.LastModifiedBy)
                });
            A.CallTo(() => dummyDynamicContentExtensions.GetFieldValue<Lstring>(A<DynamicContent>._, A<string>._))
                .Returns("test");
            A.CallTo(() => dummyDynamicContentExtensions.GetFieldValue<DynamicContent>(A<DynamicContent>._, A<string>._))
                .Returns(dummyDynamicContent);
        }
    }
}