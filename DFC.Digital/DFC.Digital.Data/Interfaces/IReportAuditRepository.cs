using System.Collections.Generic;

namespace DFC.Digital.Data.Interfaces
{
    public interface IReportAuditRepository
    {
        void CreateAudit(KeyValuePair<string, object> auditItem);

        IEnumerable<KeyValuePair<string, object>> GetAllAuditRecords();
    }
}