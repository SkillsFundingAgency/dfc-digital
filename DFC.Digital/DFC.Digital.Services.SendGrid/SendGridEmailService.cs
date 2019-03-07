using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Configuration;
using System.Net;
using System.Threading.Tasks;

namespace DFC.Digital.Services.SendGrid
{
    public class SendGridEmailService : ISendEmailService
    {
        private readonly IEmailTemplateRepository emailTemplateRepository;
        private readonly IMergeEmailContent mergeEmailContentService;
        private readonly ISendGridClientActions sendGridClientActions;

        public SendGridEmailService(IEmailTemplateRepository emailTemplateRepository, IMergeEmailContent mergeEmailContentService, ISendGridClientActions sendGridClientActions)
        {
            this.emailTemplateRepository = emailTemplateRepository;
            this.mergeEmailContentService = mergeEmailContentService;
            this.sendGridClientActions = sendGridClientActions;
        }

        public string SendGridApiKey => ConfigurationManager.AppSettings[Constants.SendGridApiKey];

        public async Task<SendEmailResponse> SendEmailAsync(SendEmailRequest sendEmailRequest)
        {
            var response = new SendEmailResponse();

            var template = emailTemplateRepository.GetByTemplateName(sendEmailRequest.TemplateName);

            if (template != null)
            {
                var client = new SendGridClient(SendGridApiKey);
                var from = new EmailAddress(template.From);
                var subject = template.Subject;
                var to = new EmailAddress(template.To);
                var plainTextContent =
                    mergeEmailContentService.MergeTemplateBodyWithContent(sendEmailRequest, template.Body);
                var htmlContent = mergeEmailContentService.MergeTemplateBodyWithContentWithHtml(sendEmailRequest, template.Body);
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                var sendResponse = await sendGridClientActions.SendEmailAsync(client, msg);

                response.Success = sendResponse.StatusCode.Equals(HttpStatusCode.Accepted);
            }

            return response;
        }
    }
}
