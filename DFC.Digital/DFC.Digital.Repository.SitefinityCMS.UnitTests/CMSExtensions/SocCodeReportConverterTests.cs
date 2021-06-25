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
    public class SocCodeReportConverterTests
    {
        private readonly IDynamicContentExtensions dummyDynamicContentExtensions;
        private readonly IDynamicModuleConverter<CmsReportItem> dummyCmsReportIteModuleConverter;
        private readonly IRelatedClassificationsRepository dummyRelatedClassificationsRepository;
        private readonly IMapper mapper;
        private readonly DynamicContent dummyDynamicContent;

        public SocCodeReportConverterTests()
        {
            dummyCmsReportIteModuleConverter = A.Fake<IDynamicModuleConverter<CmsReportItem>>(x => x.Strict());
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CmsExtensionsAutoMapperProfile>();
            });
            mapper = config.CreateMapper();
            dummyDynamicContent = A.Dummy<DynamicContent>();
            dummyRelatedClassificationsRepository = A.Fake<IRelatedClassificationsRepository>(x => x.Strict());
            dummyDynamicContentExtensions = A.Fake<IDynamicContentExtensions>(x => x.Strict());
        }

        [Fact(Skip = "LString throwing a null reference exception")]
        public void ConvertFromTest()
        {
            //Assign
            SetupCalls();
            var socCodeReportConverter = new SocCodeReportConverter(dummyDynamicContentExtensions, dummyRelatedClassificationsRepository, dummyCmsReportIteModuleConverter, mapper);

            //Act
            socCodeReportConverter.ConvertFrom(dummyDynamicContent);

            //Assert
            A.CallTo(() => dummyCmsReportIteModuleConverter.ConvertFrom(A<DynamicContent>._))
                .MustHaveHappened();
            A.CallTo(() => dummyDynamicContentExtensions.GetFieldValue<Lstring>(A<DynamicContent>._, A<string>._))
                .MustHaveHappened();
            A.CallTo(() => dummyRelatedClassificationsRepository.GetRelatedCmsReportClassifications(A<DynamicContent>._, A<string>._, A<string>._)).MustHaveHappened();
        }

        private void SetupCalls()
        {
            A.CallTo(() => dummyCmsReportIteModuleConverter.ConvertFrom(A<DynamicContent>._))
                .Returns(new CmsReportItem { DateCreated = DateTime.Now, Name = nameof(CmsReportItem.Name), LastModifiedBy = nameof(CmsReportItem.LastModifiedBy) });
            A.CallTo(() => dummyDynamicContentExtensions.GetFieldValue<Lstring>(A<DynamicContent>._, A<string>._))
                .Returns("test");
            A.CallTo(() => dummyRelatedClassificationsRepository.GetRelatedCmsReportClassifications(A<DynamicContent>._, A<string>._, A<string>._)).Returns(new EnumerableQuery<TaxonReport>(new List<TaxonReport> { new TaxonReport() }));
        }
    }
}