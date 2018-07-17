using System;
using System.Collections.Generic;
using System.Linq;
using DFC.Digital.Data.Interfaces;

namespace DFC.Digital.Service.OnetService.Services
{
    public class InMemoryReportAuditRepository : IReportAuditRepository
    {
        private List<KeyValuePair<string, object>> auditRecords = new List<KeyValuePair<string, object>>();

        public void CreateAudit(KeyValuePair<string, object> auditItem)
        {
            auditRecords.Add(auditItem);
        }

        public IEnumerable<KeyValuePair<string, object>> GetAllAuditRecords()
        {
            return auditRecords;
        }
    }
}
