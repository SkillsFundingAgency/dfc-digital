using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net;
using System.Threading.Tasks;

namespace DFC.Digital.Services.SendGrid
{
    public class SendGridEmailService : ISendEmailService<ContactAdvisorRequest>
    {
        private readonly IEmailTemplateRepository emailTemplateRepository;
        private readonly IMergeEmailContent<ContactAdvisorRequest> mergeEmailContentService;
        private readonly IAuditEmailRepository auditRepository;
        private readonly ISimulateEmailResponses simulateEmailResponsesService;
        private readonly ISendGridClient sendGridClient;

        public SendGridEmailService(
            IEmailTemplateRepository emailTemplateRepository,
            IMergeEmailContent<ContactAdvisorRequest> mergeEmailContentService,
            IAuditEmailRepository auditRepository,
            ISimulateEmailResponses simulateEmailResponsesService,
            ISendGridClient sendGridClient)
        {
            this.emailTemplateRepository = emailTemplateRepository;
            this.mergeEmailContentService = mergeEmailContentService;
            this.auditRepository = auditRepository;
            this.simulateEmailResponsesService = simulateEmailResponsesService;
            this.sendGridClient = sendGridClient;
        }

        public async Task<SendEmailResponse> SendEmailAsync(ContactAdvisorRequest sendEmailRequest)
        {
            var response = new SendEmailResponse();

            var simulateResponse = simulateEmailResponsesService.SimulateEmailResponse(sendEmailRequest.Email);

            if (simulateResponse.ValidSimulationEmail)
            {
                response.EmailSimulationResponse = true;
                response.Success = simulateResponse.SuccessResponse;
                return response;
            }

            var template = emailTemplateRepository.GetByTemplateName(sendEmailRequest.TemplateName);

            if (template != null)
            {
                var from = new EmailAddress(sendEmailRequest.Email, $"{sendEmailRequest.FirstName} {sendEmailRequest.LastName}");
                var subject = sendEmailRequest.Subject;
                var to = new EmailAddress(template.To, template.To);
                var plainTextContent =
                    mergeEmailContentService.MergeTemplateBodyWithContent(sendEmailRequest, template.BodyNoHtml);
                var htmlContent =
                    mergeEmailContentService.MergeTemplateBodyWithContentWithHtml(sendEmailRequest, template.Body);
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                var clientResponse = await sendGridClient.SendEmailAsync(msg);

                response.Success = clientResponse.StatusCode.Equals(HttpStatusCode.Accepted);

                auditRepository.AuditContactAdvisorEmailData(sendEmailRequest, template, response);
            }

            return response;
        }
    }
}
