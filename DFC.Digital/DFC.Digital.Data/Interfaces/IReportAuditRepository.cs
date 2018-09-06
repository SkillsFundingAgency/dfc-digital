using System.Collections.Generic;

namespace DFC.Digital.Data.Interfaces
{
    public interface IReportAuditRepository
    {
        void CreateAudit(string category, string auditItem);

        IDictionary<string, IList<string>> GetAllAuditRecords();
    }
}