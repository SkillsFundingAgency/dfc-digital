using AutoMapper;
using DFC.Digital.Core;
using DFC.Digital.Core.Logging;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Data.Model.Enum;
using DFC.Digital.Services.SendGrid.Models;
using Dfc.SharedConfig.Services;
using Polly.CircuitBreaker;
using SendGrid;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using IConfigurationProvider = DFC.Digital.Core.IConfigurationProvider;

namespace DFC.Digital.Services.SendGrid
{
    public class FamSendGridEmailService : SendGridEmailService
    {
        private const string SharedConfigServiceName = "dfc-fam";
        private const string SharedConfigKeyName = "FamFallbackApiResponse";
        private readonly IEmailTemplateRepository emailTemplateRepository;
        private readonly IHttpClientService<INoncitizenEmailService<ContactUsRequest>> httpClientService;
        private readonly ISharedConfigurationService sharedConfigurationService;
        private readonly IConfigurationProvider configuration;

        public FamSendGridEmailService(
            IEmailTemplateRepository emailTemplateRepository,
            IMergeEmailContent<ContactUsRequest> mergeEmailContentService,
            IAuditNoncitizenEmailRepository<ContactUsRequest> auditRepository,
            ISimulateEmailResponses simulateEmailResponsesService,
            ISendGridClient sendGridClient,
            IMapper mapper,
            IHttpClientService<INoncitizenEmailService<ContactUsRequest>> httpClientService,
            ISharedConfigurationService sharedConfigurationService,
            IConfigurationProvider configuration) : base(emailTemplateRepository, mergeEmailContentService, auditRepository, simulateEmailResponsesService, sendGridClient, mapper)
        {
            this.emailTemplateRepository = emailTemplateRepository;
            this.httpClientService = httpClientService;
            this.sharedConfigurationService = sharedConfigurationService;
            this.configuration = configuration;
        }

        public override async Task<bool> SendEmailAsync(ContactUsRequest sendEmailRequest, EmailTemplate template = null)
        {
            if (sendEmailRequest.ContactOption == nameof(ContactOption.ContactAdviser))
            {
                if (template == null)
                {
                    template = emailTemplateRepository.GetByTemplateName(sendEmailRequest.TemplateName);
                }

                try
                {
                    var url = $"{configuration.GetConfig<string>(Constants.AreaRoutingApiServiceUrl)}?location={sendEmailRequest.Postcode}";
                    var accessKey = configuration.GetConfig<string>(Constants.AreaRoutingApiSubscriptionKey);

                    httpClientService.AddHeader(Constants.OcpApimSubscriptionKey, accessKey);
                    var response = await this.httpClientService.GetAsync(url, (httpResponseMessage) => !httpResponseMessage.IsSuccessStatusCode || httpResponseMessage.StatusCode != HttpStatusCode.NoContent);
                    var areaRoutingApiResponse = await response.Content.ReadAsAsync<AreaRoutingApiResponse>();

                    template.To = areaRoutingApiResponse.EmailAddress;
                }
                catch (LoggedException)
                {
                    var fallbackResponse = await sharedConfigurationService.GetConfigAsync<AreaRoutingApiResponse>(SharedConfigServiceName, SharedConfigKeyName);
                    template.To = fallbackResponse.EmailAddress;
                }

                return await base.SendEmailAsync(sendEmailRequest, template).ConfigureAwait(false);
            }
            else
            {
                return await base.SendEmailAsync(sendEmailRequest, null).ConfigureAwait(false);
            }
        }
    }
}