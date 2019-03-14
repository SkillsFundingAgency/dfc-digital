using DFC.Digital.Data.Model;
using System.Threading.Tasks;

namespace DFC.Digital.Data.Interfaces
{
    public interface INonCitizenEmailService<in T>
        where T : class
    {
        Task<bool> SendEmailAsync(T sendEmailRequest);
    }
}
