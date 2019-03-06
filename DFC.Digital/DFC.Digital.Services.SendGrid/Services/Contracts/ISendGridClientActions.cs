using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace DFC.Digital.Services.SendGrid
{
    public interface ISendGridClientActions
    {
        Task<Response> SendEmailAsync(SendGridClient sendGridClient, SendGridMessage sendGridMessage);
    }
}
