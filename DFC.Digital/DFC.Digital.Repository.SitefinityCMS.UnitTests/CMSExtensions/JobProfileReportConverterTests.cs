using AutoMapper;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.CmsExtensions;
using FakeItEasy;
using System;
using Telerik.Sitefinity.DynamicModules.Model;
using Xunit;

namespace DFC.Digital.Repository.SitefinityCMS.CMSExtensions.Tests
{
    public class JobProfileReportConverterTests
    {
        private readonly IDynamicModuleConverter<CmsReportItem> dummyCmsReportItemModuleConverter;
        private readonly IMapper mapper;
        private readonly DynamicContent dummyDynamicContent;

        public JobProfileReportConverterTests()
        {
            dummyCmsReportItemModuleConverter = A.Fake<IDynamicModuleConverter<CmsReportItem>>(x => x.Strict());
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CmsExtensionsAutoMapperProfile>();
            });
            mapper = config.CreateMapper();
            dummyDynamicContent = A.Dummy<DynamicContent>();
        }

        [Fact]
        public void ConvertFromTest()
        {
            //Assign
            SetupCalls();
            var jobProfileReportConverter = new JobProfileReportConverter(dummyCmsReportItemModuleConverter, mapper);

            //Act
            jobProfileReportConverter.ConvertFrom(dummyDynamicContent);

            //Assert
            A.CallTo(() => dummyCmsReportItemModuleConverter.ConvertFrom(A<DynamicContent>._))
                .MustHaveHappened();
        }

        private void SetupCalls()
        {
            A.CallTo(() => dummyCmsReportItemModuleConverter.ConvertFrom(A<DynamicContent>._))
                .Returns(new CmsReportItem { DateCreated = DateTime.Now, Name = nameof(CmsReportItem.Name), LastModifiedBy = nameof(CmsReportItem.LastModifiedBy) });
        }
    }
}