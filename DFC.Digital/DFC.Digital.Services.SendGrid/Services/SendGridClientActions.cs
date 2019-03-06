using DFC.Digital.Services.SendGrid;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace DFC.Digital.Services
{
    public class SendGridClientActions : ISendGridClientActions
    {
        public async Task<Response> SendEmailAsync(SendGridClient sendGridClient, SendGridMessage sendGridMessage)
        {
            return await sendGridClient.SendEmailAsync(sendGridMessage);
        }
    }
}
