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
        private readonly IConfigurationProvider configuration;

        public SendGridEmailService(IEmailTemplateRepository emailTemplateRepository, IMergeEmailContent mergeEmailContentService, ISendGridClientActions sendGridClientActions, IConfigurationProvider configuration)
        {
            this.emailTemplateRepository = emailTemplateRepository;
            this.mergeEmailContentService = mergeEmailContentService;
            this.sendGridClientActions = sendGridClientActions;
            this.configuration = configuration;
        }

        public string SendGridApiKey => configuration.GetConfig<string>(Constants.SendGridApiKey);

        public async Task<bool> SendEmailAsync(SendEmailRequest sendEmailRequest)
        {
            var response = false;

            var template = emailTemplateRepository.GetByTemplateName(sendEmailRequest.TemplateName);

            if (template != null)
            {
                var client = new SendGridClient(SendGridApiKey);
                var from = new EmailAddress(sendEmailRequest.Email);
                var subject = sendEmailRequest.Subject;
                var to = new EmailAddress(template.To);
                var plainTextContent =
                    mergeEmailContentService.MergeTemplateBodyWithContent(sendEmailRequest, template.Body);
                var htmlContent = mergeEmailContentService.MergeTemplateBodyWithContentWithHtml(sendEmailRequest, template.Body);
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                response = await sendGridClientActions.SendEmailAsync(client, msg);
            }

            return response;
        }
    }
}
