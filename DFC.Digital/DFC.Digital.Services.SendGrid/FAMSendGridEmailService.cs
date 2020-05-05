using AutoMapper;
using DFC.Digital.Core;
using DFC.Digital.Core.Logging;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Data.Model.Enum;
using DFC.Digital.Services.SendGrid.Models;
using SendGrid;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using IConfigurationProvider = DFC.Digital.Core.IConfigurationProvider;

namespace DFC.Digital.Services.SendGrid
{
    public class FamSendGridEmailService : SendGridEmailService, IServiceStatus
    {
        private const string SharedConfigServiceName = "dfc-fam";
        private const string SharedConfigKeyName = "FamFallbackApiResponse";
        private readonly IEmailTemplateRepository emailTemplateRepository;
        private readonly IHttpClientService<INoncitizenEmailService<ContactUsRequest>> httpClientService;
        private readonly IConfigurationProvider configuration;
        private readonly IApplicationLogger applicationLogger;

        public FamSendGridEmailService(
            IEmailTemplateRepository emailTemplateRepository,
            IMergeEmailContent<ContactUsRequest> mergeEmailContentService,
            IAuditNoncitizenEmailRepository<ContactUsRequest> auditRepository,
            ISimulateEmailResponses simulateEmailResponsesService,
            ISendGridClient sendGridClient,
            IMapper mapper,
            IHttpClientService<INoncitizenEmailService<ContactUsRequest>> httpClientService,
            IConfigurationProvider configuration,
            IApplicationLogger applicationLogger) : base(emailTemplateRepository, mergeEmailContentService, auditRepository, simulateEmailResponsesService, sendGridClient, mapper)
        {
            this.emailTemplateRepository = emailTemplateRepository;
            this.httpClientService = httpClientService;
            this.configuration = configuration;
            this.applicationLogger = applicationLogger;
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
                    var response = await this.httpClientService.GetAsync(url);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var areaRoutingApiResponse = await response.Content.ReadAsAsync<AreaRoutingApiResponse>();
                        template.To = areaRoutingApiResponse.EmailAddress;
                    }
                }
                catch (LoggedException)
                {
                    return await base.SendEmailAsync(sendEmailRequest, template).ConfigureAwait(false);
                }

                return await base.SendEmailAsync(sendEmailRequest, template).ConfigureAwait(false);
            }
            else
            {
                return await base.SendEmailAsync(sendEmailRequest, null).ConfigureAwait(false);
            }
        }

        public async Task<ServiceStatus> GetCurrentStatusAsync()
        {
            const string ServiceName = "FAM Area Routing API";
            const string DummyPostcode = "CV1 2WT";

            var serviceStatus = new ServiceStatus { Name = ServiceName, Status = ServiceState.Red, CheckCorrelationId = Guid.NewGuid().ToString() };

            try
            {
                var url = $"{configuration.GetConfig<string>(Constants.AreaRoutingApiServiceUrl)}?location={DummyPostcode}";
                var accessKey = configuration.GetConfig<string>(Constants.AreaRoutingApiSubscriptionKey);

                httpClientService.AddHeader(Constants.OcpApimSubscriptionKey, accessKey);
                var response = await this.httpClientService.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var areaRoutingApiResponse = await response.Content.ReadAsAsync<AreaRoutingApiResponse>();
                    if (!string.IsNullOrWhiteSpace(areaRoutingApiResponse.EmailAddress))
                    {
                        serviceStatus.Status = ServiceState.Green;
                        serviceStatus.CheckCorrelationId = string.Empty;
                    }
                    else
                    {
                        serviceStatus.Status = ServiceState.Amber;
                        applicationLogger.Warn($"{nameof(FamSendGridEmailService)}.{nameof(GetCurrentStatusAsync)} : {Constants.ServiceStatusWarnLogMessage} - Correlation Id [{serviceStatus.CheckCorrelationId}] - Called using postcode [{DummyPostcode}], Received email address [{areaRoutingApiResponse.EmailAddress}], Api Response was [{response.StatusCode}]");
                    }
                }
                else
                {
                    applicationLogger.Error($"{nameof(FamSendGridEmailService)}.{nameof(GetCurrentStatusAsync)} : {Constants.ServiceStatusFailedLogMessage} - Correlation Id [{serviceStatus.CheckCorrelationId}] - Called using postcode [{DummyPostcode}]. Api Response was [{response.StatusCode}], [{response.ReasonPhrase}]");
                }
            }
            catch (Exception e)
            {
                applicationLogger.ErrorJustLogIt($"{nameof(FamSendGridEmailService)}.{nameof(GetCurrentStatusAsync)} : {Constants.ServiceStatusFailedLogMessage} - Correlation Id [{serviceStatus.CheckCorrelationId}] - Called using postcode [{DummyPostcode}]", e);
            }

            return serviceStatus;
        }
    }
}