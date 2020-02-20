using DFC.Digital.Data.Model;
using System.Threading.Tasks;

namespace DFC.Digital.Data.Interfaces
{
    public interface INoncitizenEmailService<in T>
        where T : class
    {
        Task<bool> SendEmailAsync(T sendEmailRequest, EmailTemplate template = null);
    }
}