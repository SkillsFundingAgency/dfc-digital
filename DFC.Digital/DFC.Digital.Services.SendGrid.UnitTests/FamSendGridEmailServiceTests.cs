using AutoMapper;
using DFC.Digital.Core;
using DFC.Digital.Core.Logging;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Services.SendGrid.Models;
using FakeItEasy;
using Newtonsoft.Json;
using Polly.CircuitBreaker;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using IConfigurationProvider = DFC.Digital.Core.IConfigurationProvider;

namespace DFC.Digital.Services.SendGrid.UnitTests
{
    public class FamSendGridEmailServiceTests
    {
        private const string EmailAddressFromAreaRouting = "fromAreaRoutingApi@apim.com";
        private const string DefaultFromEmailAddress = "test@test.com";
        private const string DefaultSubject = "test subject";
        private readonly IEmailTemplateRepository fakeEmailTemplateRepository;
        private readonly IMergeEmailContent<ContactUsRequest> fakeMergeEmailContentService;
        private readonly ISimulateEmailResponses fakeSimulateEmailResponsesService;
        private readonly ISendGridClient fakeSendGridClient;
        private readonly IAuditNoncitizenEmailRepository<ContactUsRequest> fakeAuditRepository;
        private readonly IMapper fakeMapper;
        private readonly IHttpClientService<INoncitizenEmailService<ContactUsRequest>> fakeHttpClientService;
        private readonly IConfigurationProvider fakeConfigurationProvider;
        private readonly IApplicationLogger fakeApplicationLogger;

        public FamSendGridEmailServiceTests()
        {
            var defaultTemplate = new EmailTemplate
            {
                To = "dummyTest@test.com",
                Body = "Body",
                Subject = DefaultSubject,
            };

            fakeEmailTemplateRepository = A.Fake<IEmailTemplateRepository>();
            A.CallTo(() => fakeEmailTemplateRepository.GetByTemplateName(A<string>.Ignored)).Returns(defaultTemplate);

            fakeMergeEmailContentService = A.Fake<IMergeEmailContent<ContactUsRequest>>();
            fakeSendGridClient = A.Fake<ISendGridClient>();
            var defaultResponse = new Response(HttpStatusCode.Accepted, A.Fake<HttpContent>(), null);
            A.CallTo(() => fakeSendGridClient.SendEmailAsync(A<SendGridMessage>.Ignored, A<CancellationToken>.Ignored)).Returns(defaultResponse);

            fakeSimulateEmailResponsesService = A.Fake<ISimulateEmailResponses>();
            fakeAuditRepository = A.Fake<IAuditNoncitizenEmailRepository<ContactUsRequest>>();
            fakeHttpClientService = A.Fake<IHttpClientService<INoncitizenEmailService<ContactUsRequest>>>();

            var areaRoutingApiResponse = new AreaRoutingApiResponse { EmailAddress = EmailAddressFromAreaRouting };
            var httpResponseMessage = new HttpResponseMessage { Content = new StringContent(JsonConvert.SerializeObject(areaRoutingApiResponse), Encoding.Default, "application/json") };
            A.CallTo(() => fakeHttpClientService.GetAsync(A<string>.Ignored, A<FaultToleranceType>.Ignored)).Returns(httpResponseMessage);
            fakeMapper = A.Fake<IMapper>();
            fakeConfigurationProvider = A.Fake<IConfigurationProvider>();
            fakeApplicationLogger = A.Fake<IApplicationLogger>();
        }

        [Fact]
        public async Task SendEmailAsyncWhenContactOptionNotContactAdvisorThenAreaRoutingApiAndSharedConfigNotCalled()
        {
            // Arrange
            var sendEmailRequest = new ContactUsRequest { ContactOption = "Feedback", Email = "test@test.com" };
            var service = new FamSendGridEmailService(fakeEmailTemplateRepository, fakeMergeEmailContentService, fakeAuditRepository, fakeSimulateEmailResponsesService, fakeSendGridClient, fakeMapper, fakeHttpClientService, fakeConfigurationProvider, fakeApplicationLogger);

            // Act
            var result = await service.SendEmailAsync(sendEmailRequest).ConfigureAwait(false);

            // Assert
            Assert.True(result);
            A.CallTo(() => fakeHttpClientService.GetAsync(A<string>.Ignored, A<Func<HttpResponseMessage, bool>>.Ignored, A<FaultToleranceType>.Ignored)).MustNotHaveHappened();
        }

        [Fact]
        public async Task SendEmailAsyncWhenContactOptionIsContactAdvisorThenAreaRoutingApiCalled()
        {
            // Arrange
            var sendEmailRequest = new ContactUsRequest { ContactOption = "ContactAdviser", Email = DefaultFromEmailAddress };
            var service = new FamSendGridEmailService(fakeEmailTemplateRepository, fakeMergeEmailContentService, fakeAuditRepository, fakeSimulateEmailResponsesService, fakeSendGridClient, fakeMapper, fakeHttpClientService, fakeConfigurationProvider, fakeApplicationLogger);

            // Act
            var result = await service.SendEmailAsync(sendEmailRequest).ConfigureAwait(false);

            // Assert
            Assert.True(result);
            A.CallTo(() => fakeHttpClientService.GetAsync(A<string>.Ignored, A<FaultToleranceType>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeSendGridClient.SendEmailAsync(
                    A<SendGridMessage>.That.Matches(
                        msg => msg.Personalizations[0].Tos[0].Email == EmailAddressFromAreaRouting),
                    A<CancellationToken>.Ignored)).MustHaveHappened();
        }
    }
}