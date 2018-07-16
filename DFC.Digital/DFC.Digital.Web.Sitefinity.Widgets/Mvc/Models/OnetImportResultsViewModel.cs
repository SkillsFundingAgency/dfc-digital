using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DFC.Digital.Web.Sitefinity.Widgets.Mvc.Models
{
    public class OnetImportResultsViewModel : OnetDataImportViewModel
    {
        public string SummaryDetails { get; set; }

        public string ErrorMessages { get; set; }

        public string ImportDetails { get; set; }

        public string ActionCompleted { get; set; }

        public string OtherMessage { get; set; }
    }
}