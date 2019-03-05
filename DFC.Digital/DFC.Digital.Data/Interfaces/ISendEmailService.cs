using DFC.Digital.Data.Model;
using System.Threading.Tasks;

namespace DFC.Digital.Data.Interfaces
{
    public interface ISendEmailService
    {
        Task<SendEmailResponse> SendEmailAsync(SendEmailRequest sendEmailRequest);
    }
}
