using DFC.Digital.Data.Model;
using System.Threading.Tasks;

namespace DFC.Digital.Data.Interfaces
{
    public interface IAuditEmailRepository
    {
        void AuditContactUsResponses(ContactUsRequest emailRequest, EmailTemplate emailTemplate, SendEmailResponse response);
    }
}
