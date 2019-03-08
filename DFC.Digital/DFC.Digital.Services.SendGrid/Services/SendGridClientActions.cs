using DFC.Digital.Services.SendGrid;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net;
using System.Threading.Tasks;

namespace DFC.Digital.Services
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class SendGridClientActions : ISendGridClientActions
    {
        public async Task<bool> SendEmailAsync(SendGridClient sendGridClient, SendGridMessage sendGridMessage)
        {
            var response = await sendGridClient.SendEmailAsync(sendGridMessage);

            return response.StatusCode.Equals(HttpStatusCode.Accepted);
        }
    }
}
