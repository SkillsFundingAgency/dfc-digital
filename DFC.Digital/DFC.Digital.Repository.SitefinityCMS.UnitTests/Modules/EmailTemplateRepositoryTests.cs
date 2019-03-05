using DFC.Digital.Data.Model;
using DFC.Digital.Repository.SitefinityCMS.Modules;
using FakeItEasy;
using System;
using System.Linq.Expressions;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.GenericContent.Model;
using Xunit;

namespace DFC.Digital.Repository.SitefinityCMS.UnitTests.Modules
{
    public class EmailTemplateRepositoryTests
    {
        private readonly IDynamicModuleConverter<EmailTemplate> fakeEmailTemplateConverter;
        private readonly IDynamicModuleRepository<EmailTemplate> fakeRepository;
        private readonly IDynamicContentExtensions fakeDynamicContentExtensions;

        public EmailTemplateRepositoryTests()
        {
            fakeEmailTemplateConverter = A.Fake<IDynamicModuleConverter<EmailTemplate>>();
            fakeRepository = A.Fake<IDynamicModuleRepository<EmailTemplate>>();
            fakeDynamicContentExtensions = A.Fake<IDynamicContentExtensions>();
        }

        [Theory]
        [InlineData("Contact Adviser Email Template", true)]
        public void GetByTemplateNameTest(string templateName, bool publishing)
        {
            var dummyEmailTemplate = A.Dummy<EmailTemplate>();
            dummyEmailTemplate.TemplateName = templateName;
            A.CallTo(() => fakeEmailTemplateConverter.ConvertFrom(A<DynamicContent>._)).Returns(dummyEmailTemplate);

            var emailTemplateRepository = new EmailTemplateRepository(fakeRepository, fakeDynamicContentExtensions, fakeEmailTemplateConverter);
            emailTemplateRepository.GetByTemplateName(templateName);

            A.CallTo(() => fakeRepository.Get(A<Expression<Func<DynamicContent, bool>>>.That.Matches(m => LinqExpressionsTestHelper.IsExpressionEqual(m, item => item.UrlName == templateName && item.Status == (publishing ? ContentLifecycleStatus.Master : ContentLifecycleStatus.Live))))).MustHaveHappened();
        }
    }
}
