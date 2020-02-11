using AutoMapper;
using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Services.SendGrid.Models;
using Dfc.SharedConfig.Services;
using Polly.CircuitBreaker;
using SendGrid;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace DFC.Digital.Services.SendGrid
{
    public class FamSendGridEmailService : SendGridEmailService
    {
        private const string SharedConfigServiceName = "dfc-fam";
        private const string SharedConfigKeyName = "FamFallbackApiResponse";
        private readonly IEmailTemplateRepository emailTemplateRepository;
        private readonly IHttpClientService<INoncitizenEmailService<ContactUsRequest>> httpClientService;
        private readonly ISharedConfigurationService sharedConfigurationService;

        public FamSendGridEmailService(
            IEmailTemplateRepository emailTemplateRepository,
            IMergeEmailContent<ContactUsRequest> mergeEmailContentService,
            IAuditNoncitizenEmailRepository<ContactUsRequest> auditRepository,
            ISimulateEmailResponses simulateEmailResponsesService,
            ISendGridClient sendGridClient,
            IMapper mapper,
            IHttpClientService<INoncitizenEmailService<ContactUsRequest>> httpClientService,
            ISharedConfigurationService sharedConfigurationService) : base(emailTemplateRepository, mergeEmailContentService, auditRepository, simulateEmailResponsesService, sendGridClient, mapper)
        {
            this.emailTemplateRepository = emailTemplateRepository;
            this.httpClientService = httpClientService;
            this.sharedConfigurationService = sharedConfigurationService;
        }

        public override async Task<bool> SendEmailAsync(ContactUsRequest sendEmailRequest)
        {
            if (sendEmailRequest.ContactOption == "ContactAdviser")
            {
                var template = emailTemplateRepository.GetByTemplateName(sendEmailRequest.TemplateName);

                try
                {
                    var url = $"{ConfigurationManager.AppSettings[Constants.AreaRoutingApiServiceUrl]}?location={sendEmailRequest.Postcode}";
                    var accessKey = ConfigurationManager.AppSettings[Constants.AreaRoutingApiSubscriptionKey];

                    httpClientService.AddHeader(Constants.OcpApimSubscriptionKey, accessKey);
                    var response = await this.httpClientService.GetWhereAsync(url, (httpResponseMessage) => !httpResponseMessage.IsSuccessStatusCode && httpResponseMessage.StatusCode != HttpStatusCode.NoContent);
                    var areaRoutingApiResponse = await response.Content.ReadAsAsync<AreaRoutingApiResponse>();

                    template.To = areaRoutingApiResponse.EmailAddress;
                }
                catch (BrokenCircuitException)
                {
                    var fallbackResponse = await sharedConfigurationService.GetConfigAsync<AreaRoutingApiResponse>(SharedConfigServiceName, SharedConfigKeyName);
                    template.To = fallbackResponse.EmailAddress;
                }

                return await SendEmail(sendEmailRequest, template).ConfigureAwait(false);
            }
            else
            {
                return await base.SendEmailAsync(sendEmailRequest).ConfigureAwait(false);
            }
        }
    }
}