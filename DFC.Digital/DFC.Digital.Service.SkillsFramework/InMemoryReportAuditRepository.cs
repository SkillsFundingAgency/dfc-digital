using System.Collections.Generic;
using DFC.Digital.Data.Interfaces;

namespace DFC.Digital.Service.SkillsFramework
{
    public class InMemoryReportAuditRepository : IReportAuditRepository
    {
        private readonly Dictionary<string, IList<string>> auditRecords = new Dictionary<string, IList<string>>();

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
        public IDictionary<string, IList<string>> GetAllAuditRecords()
        {
            return auditRecords;
        }
    }
}