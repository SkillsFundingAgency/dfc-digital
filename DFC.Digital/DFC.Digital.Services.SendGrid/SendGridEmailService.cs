using AutoMapper;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DFC.Digital.Services.SendGrid
{
    public class SendGridEmailService : INoncitizenEmailService<ContactUsRequest>
    {
        private readonly IEmailTemplateRepository emailTemplateRepository;
        private readonly IMergeEmailContent<ContactUsRequest> mergeEmailContentService;
        private readonly IAuditNoncitizenEmailRepository<ContactUsRequest> auditRepository;
        private readonly ISimulateEmailResponses simulateEmailResponsesService;
        private readonly ISendGridClient sendGridClient;
        private readonly IMapper mapper;

        public SendGridEmailService(
            IEmailTemplateRepository emailTemplateRepository,
            IMergeEmailContent<ContactUsRequest> mergeEmailContentService,
            IAuditNoncitizenEmailRepository<ContactUsRequest> auditRepository,
            ISimulateEmailResponses simulateEmailResponsesService,
            ISendGridClient sendGridClient,
            IMapper mapper)
        {
            this.emailTemplateRepository = emailTemplateRepository;
            this.mergeEmailContentService = mergeEmailContentService;
            this.auditRepository = auditRepository;
            this.simulateEmailResponsesService = simulateEmailResponsesService;
            this.sendGridClient = sendGridClient;
            this.mapper = mapper;
        }

        public async Task<bool> SendEmailAsync(ContactUsRequest sendEmailRequest)
        {
            if (simulateEmailResponsesService.IsThisSimulationRequest(sendEmailRequest.Email))
            {
                return simulateEmailResponsesService.SimulateEmailResponse(sendEmailRequest.Email);
            }

            var template = emailTemplateRepository.GetByTemplateName(sendEmailRequest.TemplateName);
            if (template != null)
            {
                var from = new EmailAddress(sendEmailRequest.Email, $"{sendEmailRequest.FirstName} {sendEmailRequest.LastName}");
                var subject = template.Subject;
                var to = template.To?.Split(';').Select(toEmail => new EmailAddress(toEmail.Trim(), toEmail.Trim())).ToList();
                var plainTextContent = mergeEmailContentService.MergeTemplateBodyWithContent(sendEmailRequest, template.BodyNoHtml);
                var htmlContent = mergeEmailContentService.MergeTemplateBodyWithContent(sendEmailRequest, template.Body);
                var msg = MailHelper.CreateSingleEmailToMultipleRecipients(from, to, subject, plainTextContent, htmlContent);
                var clientResponse = await sendGridClient.SendEmailAsync(msg);
                var auditResponse = mapper.Map<SendEmailResponse>(clientResponse);
                var result = clientResponse.StatusCode.Equals(HttpStatusCode.Accepted);

                auditRepository.CreateAudit(sendEmailRequest, template, auditResponse);
                return result;
            }

            return false;
        }
    }
}
