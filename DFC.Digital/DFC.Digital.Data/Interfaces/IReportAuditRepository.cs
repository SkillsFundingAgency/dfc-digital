using System.Collections.Generic;

namespace DFC.Digital.Data.Interfaces
{
    public interface IReportAuditRepository
    {
        void CreateAudit(string category, string auditItem);

        IEnumerable<string> GetAllAuditRecordsByCategory(string category);

        IEnumerable<string> GetAllAuditRecordsCategories();

        IDictionary<string, IList<string>> GetAllAuditRecords();
    }
}