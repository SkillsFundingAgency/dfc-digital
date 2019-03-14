using DFC.Digital.Data.Model;
using System.Threading.Tasks;

namespace DFC.Digital.Data.Interfaces
{
    public interface IAuditNoncitizenEmailRepository<T>
    {
        void CreateAudit(T emailRequest, EmailTemplate emailTemplate, SendEmailResponse response);
    }
}
