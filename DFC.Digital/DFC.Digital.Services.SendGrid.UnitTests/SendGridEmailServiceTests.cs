using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using FakeItEasy;
using FluentAssertions;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace DFC.Digital.Services.SendGrid.Tests
{
    public class SendGridEmailServiceTests
    {
        private readonly IEmailTemplateRepository fakeEmailTemplateRepository;
        private readonly IMergeEmailContent fakeMergeEmailContentService;
        private readonly ISendGridClientActions fakeSendGridClientActions;
        private readonly EmailTemplate goodEmailTemplate;

        public SendGridEmailServiceTests()
        {
            fakeEmailTemplateRepository = A.Fake<IEmailTemplateRepository>(ops => ops.Strict());
            fakeMergeEmailContentService = A.Fake<IMergeEmailContent>(ops => ops.Strict());
            fakeSendGridClientActions = A.Fake<ISendGridClientActions>(ops => ops.Strict());
            goodEmailTemplate = new EmailTemplate
            {
                Body = nameof(EmailTemplate.Body),
                To = "trevk15@yahoo.co.uk",
                TemplateName = nameof(EmailTemplate.TemplateName),
                Subject = nameof(EmailTemplate.Subject),
                From = "trevk155@gmail.com"
            };
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task SendEmailAsyncTest(bool validEmailTemplate)
        {
            //Assign
            var sendEmailService = new SendGridEmailService(fakeEmailTemplateRepository, fakeMergeEmailContentService, fakeSendGridClientActions);

            var sendRequest = new SendEmailRequest
            {
                TemplateName = nameof(SendEmailRequest.TemplateName),
                Content = nameof(SendEmailRequest.Content)
            };

            A.CallTo(() => fakeEmailTemplateRepository.GetByTemplateName(A<string>._))
                .Returns(validEmailTemplate ? goodEmailTemplate : null);
            A.CallTo(() => fakeMergeEmailContentService.MergeTemplateBodyWithContent(A<string>._, A<string>._))
                .Returns(nameof(IMergeEmailContent.MergeTemplateBodyWithContent));
            A.CallTo(() => fakeMergeEmailContentService.MergeTemplateBodyWithContentWithHtml(A<string>._, A<string>._))
                .Returns(nameof(IMergeEmailContent.MergeTemplateBodyWithContentWithHtml));
            A.CallTo(() => fakeSendGridClientActions.SendEmailAsync(A<SendGridClient>._, A<SendGridMessage>._)).Returns(new Response(HttpStatusCode.Accepted, new StringContent(string.Empty), null));

            //Act
            var result = await sendEmailService.SendEmailAsync(sendRequest);

            //Assert
            A.CallTo(() => fakeEmailTemplateRepository.GetByTemplateName(A<string>._)).MustHaveHappened();
            if (validEmailTemplate)
            {
                A.CallTo(() => fakeMergeEmailContentService.MergeTemplateBodyWithContent(A<string>._, A<string>._))
                    .MustHaveHappened();
                A.CallTo(() =>
                        fakeMergeEmailContentService.MergeTemplateBodyWithContentWithHtml(A<string>._, A<string>._))
                    .MustHaveHappened();
                A.CallTo(() => fakeSendGridClientActions.SendEmailAsync(A<SendGridClient>._, A<SendGridMessage>._)).MustHaveHappened();
                result.Success.Should().BeTrue();
            }
            else
            {
                A.CallTo(() => fakeSendGridClientActions.SendEmailAsync(A<SendGridClient>._, A<SendGridMessage>._)).MustNotHaveHappened();
                A.CallTo(() => fakeMergeEmailContentService.MergeTemplateBodyWithContent(A<string>._, A<string>._))
                    .MustNotHaveHappened();
                A.CallTo(() =>
                        fakeMergeEmailContentService.MergeTemplateBodyWithContentWithHtml(A<string>._, A<string>._))
                    .MustNotHaveHappened();
                result.Success.Should().BeFalse();
            }
        }
    }
}