using System.Collections.Generic;

namespace DFC.Digital.Web.Sitefinity.CmsExtensions.MVC.Models
{
    public class OnetImportResultsViewModel : OnetDataImportViewModel
    {
        public string SummaryDetails { get; set; }

        public string ErrorMessages { get; set; }

        public string ImportDetails { get; set; }

        public string ActionCompleted { get; set; }

        public string OtherMessage { get; set; }

        public IDictionary<string, IList<string>> AuditRecords { get; set; }
    }
}