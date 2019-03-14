using AutoMapper;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Services.SendGrid.Config;
using FakeItEasy;
using FluentAssertions;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DFC.Digital.Services.SendGrid.Tests
{
    public class SendGridEmailServiceTests
    {
        private readonly IEmailTemplateRepository fakeEmailTemplateRepository;
        private readonly IMergeEmailContent<ContactUsRequest> fakeMergeEmailContentService;
        private readonly ISendGridClient fakeSendGridClient;
        private readonly Core.IConfigurationProvider fakeConfiguration;
        private readonly IAuditNonCitizenEmailRepository<ContactUsRequest> fakeAuditRepository;
        private readonly ISimulateEmailResponses fakeSimulateEmailResponsesService;

        private readonly EmailTemplate goodEmailTemplate;
        private readonly IMapper fakeMapper;

        public SendGridEmailServiceTests()
        {
            fakeEmailTemplateRepository = A.Fake<IEmailTemplateRepository>(ops => ops.Strict());
            fakeMergeEmailContentService = A.Fake<IMergeEmailContent<ContactUsRequest>>(ops => ops.Strict());
            fakeSendGridClient = A.Fake<ISendGridClient>(ops => ops.Strict());
            fakeConfiguration = A.Fake<Core.IConfigurationProvider>(ops => ops.Strict());
            fakeSimulateEmailResponsesService = A.Fake<ISimulateEmailResponses>(ops => ops.Strict());
            fakeAuditRepository = A.Fake<IAuditNonCitizenEmailRepository<ContactUsRequest>>(ops => ops.Strict());
            fakeMapper = A.Fake<IMapper>(ops => ops.Strict());
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

        public static IEnumerable<object[]> SendEmailAsyncTestInput()
        {
            yield return new object[]
            {
                new ContactUsRequest(),
                true,
                true
            };

            yield return new object[]
            {
                new ContactUsRequest(),
                true,
                false
            };

            yield return new object[]
            {
                new ContactUsRequest
                {
                    TemplateName = nameof(ContactUsRequest.TemplateName),
                    Message = nameof(ContactUsRequest.Message),
                    Email = nameof(ContactUsRequest.Email)
                },
                false,
                true
            };

            yield return new object[]
            {
                new ContactUsRequest
                {
                    Message = nameof(ContactUsRequest.Message),
                    Email = nameof(ContactUsRequest.Email)
                },
                false,
                false
            };
        }

        //public static IEnumerable<object[]> SendGridAutoMapperProfileTestInput()
        //{
        //    yield return new object[]
        //    {
        //       new Response(HttpStatusCode.Accepted, new StringContent("dummy"), A.Dummy<HttpResponseHeaders>()),
        //       new SendEmailResponse
        //       {
        //           Headers = A.Dummy<HttpResponseHeaders>(),
        //           StatusCode = HttpStatusCode.Accepted
        //       }
        //    };

        //    //yield return new object[]
        //    //{
        //    //   new Response(HttpStatusCode.BadGateway, A.Dummy<HttpContent>(), A.Dummy<HttpResponseHeaders>()),
        //    //   new SendEmailResponse
        //    //   {
        //    //       Headers = A.Dummy<HttpResponseHeaders>(),
        //    //       StatusCode = HttpStatusCode.BadGateway
        //    //   }
        //    //};
        //}
        [Theory]
        [MemberData(nameof(SendEmailAsyncTestInput))]
        public async Task SendEmailAsyncTest(ContactUsRequest contactUsRequest, bool isThisSimulation, bool expectation)
        {
            //Assign
            var sendEmailService = new SendGridEmailService(fakeEmailTemplateRepository, fakeMergeEmailContentService, fakeAuditRepository, fakeSimulateEmailResponsesService, fakeSendGridClient, fakeMapper);

            A.CallTo(() => fakeEmailTemplateRepository.GetByTemplateName(A<string>._)).Returns(string.IsNullOrEmpty(contactUsRequest.TemplateName) ? null : goodEmailTemplate);
            A.CallTo(() => fakeMergeEmailContentService.MergeTemplateBodyWithContent(A<ContactUsRequest>._, A<string>._)).Returns(nameof(IMergeEmailContent<ContactUsRequest>.MergeTemplateBodyWithContent));
            A.CallTo(() => fakeSendGridClient.SendEmailAsync(A<SendGridMessage>._, A<CancellationToken>._)).Returns(new Response(HttpStatusCode.Accepted, null, null));
            A.CallTo(() => fakeConfiguration.GetConfig<string>(A<string>._)).Returns(string.Empty);
            A.CallTo(() => fakeSimulateEmailResponsesService.IsThisSimulationRequest(A<string>._)).Returns(isThisSimulation);
            A.CallTo(() => fakeSimulateEmailResponsesService.SimulateEmailResponse(A<string>._)).Returns(expectation);
            A.CallTo(() => fakeAuditRepository.CreateAudit(A<ContactUsRequest>._, A<EmailTemplate>._, A<SendEmailResponse>._)).DoesNothing();
            A.CallTo(() => fakeMapper.Map<SendEmailResponse>(A<object>._)).Returns(A.Dummy<SendEmailResponse>());

            //Act
            var result = await sendEmailService.SendEmailAsync(contactUsRequest);

            //Assert
            result.Should().Be(expectation);
            if (isThisSimulation)
            {
                A.CallTo(() => fakeEmailTemplateRepository.GetByTemplateName(A<string>._)).MustNotHaveHappened();
                A.CallTo(() => fakeSendGridClient.SendEmailAsync(A<SendGridMessage>._, A<CancellationToken>._)).MustNotHaveHappened();
                A.CallTo(() => fakeMergeEmailContentService.MergeTemplateBodyWithContent(A<ContactUsRequest>._, A<string>._)).MustNotHaveHappened();
                A.CallTo(() => fakeAuditRepository.CreateAudit(A<ContactUsRequest>._, A<EmailTemplate>._, A<SendEmailResponse>._)).MustNotHaveHappened();
            }
            else
            {
                A.CallTo(() => fakeEmailTemplateRepository.GetByTemplateName(A<string>._)).MustHaveHappened();
                if (expectation)
                {
                    A.CallTo(() => fakeMergeEmailContentService.MergeTemplateBodyWithContent(A<ContactUsRequest>._, A<string>._)).MustHaveHappened(Repeated.Exactly.Twice);
                    A.CallTo(() => fakeSendGridClient.SendEmailAsync(A<SendGridMessage>._, A<CancellationToken>._)).MustHaveHappened();
                    A.CallTo(() => fakeAuditRepository.CreateAudit(A<ContactUsRequest>._, A<EmailTemplate>._, A<SendEmailResponse>._)).MustHaveHappened();
                }
                else
                {
                    A.CallTo(() => fakeSendGridClient.SendEmailAsync(A<SendGridMessage>._, A<CancellationToken>._)).MustNotHaveHappened();
                    A.CallTo(() => fakeMergeEmailContentService.MergeTemplateBodyWithContent(A<ContactUsRequest>._, A<string>._)).MustNotHaveHappened();
                    A.CallTo(() => fakeAuditRepository.CreateAudit(A<ContactUsRequest>._, A<EmailTemplate>._, A<SendEmailResponse>._)).MustNotHaveHappened();
                }
            }
        }

        [Fact]

        //[MemberData(typeof(SendGridAutoMapperProfileTestInput))]
        public void SendGridAutoMapperProfileTest()
        {
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<SendGridAutoMapperProfile>());
            var mapper = mapperConfig.CreateMapper();

            //var response = new Response(HttpStatusCode.Accepted, new StringContent("dummy"), A.Dummy<HttpResponseHeaders>());
            //var result = mapper.Map<SendEmailResponse>(response);
            //var expectation = new SendEmailResponse
            //{
            //    Headers = A.Dummy<HttpResponseHeaders>(),
            //    StatusCode = HttpStatusCode.BadGateway
            //};

            //Assert
            //result.Should().BeEquivalentTo(expectation);
            mapperConfig.AssertConfigurationIsValid();
        }
    }
}