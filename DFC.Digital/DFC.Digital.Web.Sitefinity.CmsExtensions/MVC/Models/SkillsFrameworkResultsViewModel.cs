using System.Collections.Generic;

namespace DFC.Digital.Web.Sitefinity.CmsExtensions
{
    public class SkillsFrameworkResultsViewModel : SkillsFrameworkImportViewModel
    {
        public string ActionCompleted { get; set; }

        public string OtherMessage { get; set; }

        public IDictionary<string, IList<string>> AuditRecords { get; set; }
    }
}