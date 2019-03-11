using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using FakeItEasy;
using FluentAssertions;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DFC.Digital.Services.SendGrid.Tests
{
    public class SendGridEmailServiceTests
    {
        private readonly IEmailTemplateRepository fakeEmailTemplateRepository;
        private readonly IMergeEmailContent<ContactAdvisorRequest> fakeMergeEmailContentService;
        private readonly ISendGridClient fakeSendGridClient;
        private readonly IConfigurationProvider fakeConfiguration;
        private readonly IAuditEmailRepository fakeAuditRepository;
        private readonly ISimulateEmailResponses fakeSimulateEmailResponsesService;

        private readonly EmailTemplate goodEmailTemplate;

        public SendGridEmailServiceTests()
        {
            fakeEmailTemplateRepository = A.Fake<IEmailTemplateRepository>(ops => ops.Strict());
            fakeMergeEmailContentService = A.Fake<IMergeEmailContent<ContactAdvisorRequest>>(ops => ops.Strict());
            fakeSendGridClient = A.Fake<ISendGridClient>(ops => ops.Strict());
            fakeConfiguration = A.Fake<IConfigurationProvider>(ops => ops.Strict());
            fakeSimulateEmailResponsesService = A.Fake<ISimulateEmailResponses>(ops => ops.Strict());
            fakeAuditRepository = A.Fake<IAuditEmailRepository>(ops => ops.Strict());
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
            var sendEmailService = new SendGridEmailService(fakeEmailTemplateRepository, fakeMergeEmailContentService, fakeAuditRepository, fakeSimulateEmailResponsesService, fakeSendGridClient);

            var sendRequest = new ContactAdvisorRequest
            {
                TemplateName = nameof(ContactAdvisorRequest.TemplateName),
                Message = nameof(ContactAdvisorRequest.Message)
            };

            A.CallTo(() => fakeEmailTemplateRepository.GetByTemplateName(A<string>._))
                .Returns(validEmailTemplate ? goodEmailTemplate : null);
            A.CallTo(() => fakeMergeEmailContentService.MergeTemplateBodyWithContent(A<ContactAdvisorRequest>._, A<string>._))
                .Returns(nameof(IMergeEmailContent<ContactAdvisorRequest>.MergeTemplateBodyWithContent));
            A.CallTo(() => fakeMergeEmailContentService.MergeTemplateBodyWithContentWithHtml(A<ContactAdvisorRequest>._, A<string>._))
                .Returns(nameof(IMergeEmailContent<ContactAdvisorRequest>.MergeTemplateBodyWithContentWithHtml));
            A.CallTo(() => fakeSendGridClient.SendEmailAsync(A<SendGridMessage>._, A<CancellationToken>._)).Returns(new Response(HttpStatusCode.Accepted, null, null));
            A.CallTo(() => fakeConfiguration.GetConfig<string>(A<string>._)).Returns(string.Empty);
            A.CallTo(() => fakeSimulateEmailResponsesService.SimulateEmailResponse(A<string>._)).Returns(new SimulateEmailResponse());
            A.CallTo(() => fakeAuditRepository.AuditContactAdvisorEmailData(A<ContactAdvisorRequest>._, A<EmailTemplate>._, A<SendEmailResponse>._)).DoesNothing();

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
                A.CallTo(() => fakeSendGridClient.SendEmailAsync(A<SendGridMessage>._, A<CancellationToken>._)).MustHaveHappened();
                result.Success.Should().BeTrue();
                A.CallTo(() => fakeAuditRepository.AuditContactAdvisorEmailData(A<ContactAdvisorRequest>._, A<EmailTemplate>._, A<SendEmailResponse>._)).MustHaveHappened();
            }
            else
            {
                A.CallTo(() => fakeSendGridClient.SendEmailAsync(A<SendGridMessage>._, A<CancellationToken>._)).MustNotHaveHappened();
                A.CallTo(() => fakeMergeEmailContentService.MergeTemplateBodyWithContent(A<ContactAdvisorRequest>._, A<string>._))
                    .MustNotHaveHappened();
                A.CallTo(() =>
                        fakeMergeEmailContentService.MergeTemplateBodyWithContentWithHtml(A<ContactAdvisorRequest>._, A<string>._))
                    .MustNotHaveHappened();
                result.Success.Should().BeFalse();
            }
        }
    }
}