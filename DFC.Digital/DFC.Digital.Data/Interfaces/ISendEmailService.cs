using DFC.Digital.Data.Model;
using System.Threading.Tasks;

namespace DFC.Digital.Data.Interfaces
{
    public interface ISendEmailService<in T>
        where T : class
    {
        Task<SendEmailResponse> SendEmailAsync(T sendEmailRequest);
    }
}
