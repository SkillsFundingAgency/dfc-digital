using System;
using System.Collections.Generic;
using System.Linq;
using DFC.Digital.Data.Interfaces;

namespace DFC.Digital.Service.OnetService.Services
{
    public class InMemoryReportAuditRepository : IReportAuditRepository
    {
        private Dictionary<string, IList<string>> auditRecords = new Dictionary<string, IList<string>>();

        public void CreateAudit(string category, string auditItem)
        {
            if (auditRecords.ContainsKey(category))
            {
                auditRecords[category].Add(auditItem);
            }
            else
            {
                auditRecords.Add(category, new List<string> { auditItem });
            }
        }

        public IEnumerable<string> GetAllAuditRecordsByCategory(string category)
        {
            if (auditRecords.ContainsKey(category))
            {
                return auditRecords[category];
            }

            return default;
        }

        public IEnumerable<string> GetAllAuditRecordsCategories()
        {
            return auditRecords.Keys;
        }

        public IDictionary<string, IList<string>> GetAllAuditRecords()
        {
            return auditRecords;
        }
    }
}
