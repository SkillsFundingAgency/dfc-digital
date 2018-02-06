using DFC.Digital.Data.Interfaces;
using System.Collections.Generic;

namespace DFC.Digital.AutomationTest.Utilities.Stubs
{
    public class InMemoryAuditRepository : IAuditRepository
    {
        private List<object> auditRecords = new List<object>();

        public void CreateAudit(object record)
        {
            auditRecords.Add(record);
        }
    }
}