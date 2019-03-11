using DFC.Digital.Data.Model;
using System.Threading.Tasks;

namespace DFC.Digital.Data.Interfaces
{
    public interface IAuditEmailRepository : IAuditRepository
    {
        void AuditContactAdvisorEmailData(ContactAdvisorRequest emailRequest, EmailTemplate emailTemplate, SendEmailResponse response);
    }
}
