using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using FakeItEasy;
using FluentAssertions;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using Xunit;

namespace DFC.Digital.Services.SendGrid.Tests
{
    public class SendGridEmailServiceTests
    {
        private readonly IEmailTemplateRepository fakeEmailTemplateRepository;
        private readonly IMergeEmailContent fakeMergeEmailContentService;
        private readonly ISendGridClientActions fakeSendGridClientActions;
        private readonly IConfigurationProvider fakeConfiguration;
        private readonly EmailTemplate goodEmailTemplate;

        public SendGridEmailServiceTests()
        {
            fakeEmailTemplateRepository = A.Fake<IEmailTemplateRepository>(ops => ops.Strict());
            fakeMergeEmailContentService = A.Fake<IMergeEmailContent>(ops => ops.Strict());
            fakeSendGridClientActions = A.Fake<ISendGridClientActions>(ops => ops.Strict());
            fakeConfiguration = A.Fake<IConfigurationProvider>(ops => ops.Strict());
            goodEmailTemplate = new EmailTemplate
            {
                Body = nameof(EmailTemplate.Body),
                BodyNoHtml = nameof(EmailTemplate.BodyNoHtml),
                To = nameof(EmailTemplate.To),
                TemplateName = nameof(EmailTemplate.TemplateName),
                Subject = nameof(EmailTemplate.Subject),
                From = nameof(EmailTemplate.From)
            };
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task SendEmailAsyncTest(bool validEmailTemplate)
        {
            //Assign
            var sendEmailService = new SendGridEmailService(fakeEmailTemplateRepository, fakeMergeEmailContentService, fakeSendGridClientActions, fakeConfiguration);

            var sendRequest = new ContactAdvisorRequest
            {
                TemplateName = nameof(ContactAdvisorRequest.TemplateName),
                Message = nameof(ContactAdvisorRequest.Message)
            };

            A.CallTo(() => fakeEmailTemplateRepository.GetByTemplateName(A<string>._))
                .Returns(validEmailTemplate ? goodEmailTemplate : null);
            A.CallTo(() => fakeMergeEmailContentService.MergeTemplateBodyWithContent(A<ContactAdvisorRequest>._, A<string>._))
                .Returns(nameof(IMergeEmailContent.MergeTemplateBodyWithContent));
            A.CallTo(() => fakeMergeEmailContentService.MergeTemplateBodyWithContentWithHtml(A<ContactAdvisorRequest>._, A<string>._))
                .Returns(nameof(IMergeEmailContent.MergeTemplateBodyWithContentWithHtml));
            A.CallTo(() => fakeSendGridClientActions.SendEmailAsync(A<SendGridClient>._, A<SendGridMessage>._)).Returns(true);
            A.CallTo(() => fakeConfiguration.GetConfig<string>(A<string>._)).Returns(string.Empty);

            //Act
            var result = await sendEmailService.SendEmailAsync(sendRequest);

            //Assert
            A.CallTo(() => fakeEmailTemplateRepository.GetByTemplateName(A<string>._)).MustHaveHappened();
            if (validEmailTemplate)
            {
                A.CallTo(() => fakeMergeEmailContentService.MergeTemplateBodyWithContent(A<ContactAdvisorRequest>._, A<string>._))
                    .MustHaveHappened();
                A.CallTo(() =>
                        fakeMergeEmailContentService.MergeTemplateBodyWithContentWithHtml(A<ContactAdvisorRequest>._, A<string>._))
                    .MustHaveHappened();
                A.CallTo(() => fakeSendGridClientActions.SendEmailAsync(A<SendGridClient>._, A<SendGridMessage>._)).MustHaveHappened();
                result.Should().BeTrue();
                A.CallTo(() => fakeConfiguration.GetConfig<string>(A<string>._)).MustHaveHappened();
            }
            else
            {
                A.CallTo(() => fakeSendGridClientActions.SendEmailAsync(A<SendGridClient>._, A<SendGridMessage>._)).MustNotHaveHappened();
                A.CallTo(() => fakeMergeEmailContentService.MergeTemplateBodyWithContent(A<ContactAdvisorRequest>._, A<string>._))
                    .MustNotHaveHappened();
                A.CallTo(() =>
                        fakeMergeEmailContentService.MergeTemplateBodyWithContentWithHtml(A<ContactAdvisorRequest>._, A<string>._))
                    .MustNotHaveHappened();
                result.Should().BeFalse();
                A.CallTo(() => fakeConfiguration.GetConfig<string>(A<string>._)).MustNotHaveHappened();
            }
        }
    }
}