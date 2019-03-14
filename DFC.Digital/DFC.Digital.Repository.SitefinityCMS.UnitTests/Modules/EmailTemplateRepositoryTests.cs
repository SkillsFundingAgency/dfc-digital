using DFC.Digital.Data.Model;
using DFC.Digital.Repository.SitefinityCMS.Modules;
using FakeItEasy;
using FluentAssertions;
using System;
using System.Linq.Expressions;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.GenericContent.Model;
using Telerik.Sitefinity.Model;
using Xunit;

namespace DFC.Digital.Repository.SitefinityCMS.UnitTests
{
    public class EmailTemplateRepositoryTests
    {
        private readonly IDynamicModuleConverter<EmailTemplate> fakeEmailTemplateConverter;
        private readonly IDynamicModuleRepository<EmailTemplate> fakeRepository;

        public EmailTemplateRepositoryTests()
        {
            fakeEmailTemplateConverter = A.Fake<IDynamicModuleConverter<EmailTemplate>>();
            fakeRepository = A.Fake<IDynamicModuleRepository<EmailTemplate>>();
        }

        [Theory]
        [InlineData("Contact Adviser Email Template", true)]
        [InlineData("Contact Adviser Email Template", false)]
        public void GetTemplateNameTest(string templateName, bool validTemplate)
        {
            //Assign
            var fakeEmailTemplateRepository = GetTestEmailTemplateRepository(validTemplate);

            //Act
            fakeEmailTemplateRepository.GetByTemplateName(templateName);

            //Assert
            A.CallTo(() => fakeRepository.Get(A<Expression<Func<DynamicContent, bool>>>._)).MustHaveHappened();

            if (validTemplate)
            {
                A.CallTo(() => fakeEmailTemplateConverter.ConvertFrom(A<DynamicContent>._)).MustHaveHappened();
            }
            else
            {
                A.CallTo(() => fakeEmailTemplateConverter.ConvertFrom(A<DynamicContent>._)).MustNotHaveHappened();
            }
        }

        [Fact]
        public void GetContentTypeTest()
        {
            //Assign
            var dummyEmailTemplate = A.Dummy<EmailTemplate>();
            A.CallTo(() => fakeEmailTemplateConverter.ConvertFrom(A<DynamicContent>._)).Returns(dummyEmailTemplate);
            var emailTemplateRepository = new EmailTemplateRepository(fakeRepository, fakeEmailTemplateConverter);

            //Act
            var result = emailTemplateRepository.GetContentType();

            //Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public void GetProviderNameTest()
        {
            //Assign
            var dummyEmailTemplate = A.Dummy<EmailTemplate>();
            A.CallTo(() => fakeEmailTemplateConverter.ConvertFrom(A<DynamicContent>._)).Returns(dummyEmailTemplate);
            var emailTemplateRepository = new EmailTemplateRepository(fakeRepository, fakeEmailTemplateConverter);

            //Act
            var result = emailTemplateRepository.GetProviderName();

            //Assert
            result.Should().NotBeNull();
        }

        private EmailTemplateRepository GetTestEmailTemplateRepository(bool validTemplate)
        {
            //Setup the fakes and dummies
            var dummyDynamicContent = A.Dummy<DynamicContent>();
            A.CallTo(() => fakeRepository.Get(A<Expression<Func<DynamicContent, bool>>>._)).Returns(validTemplate ? dummyDynamicContent : null);
            A.CallTo(() => fakeEmailTemplateConverter.ConvertFrom(A<DynamicContent>._)).Returns(new EmailTemplate());

            return new EmailTemplateRepository(fakeRepository, fakeEmailTemplateConverter);
        }
    }
}
