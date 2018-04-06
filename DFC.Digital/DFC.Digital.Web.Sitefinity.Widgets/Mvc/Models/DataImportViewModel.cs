using System.ComponentModel.DataAnnotations;
using System.Web;

namespace DFC.Digital.Web.Sitefinity.Widgets.Mvc.Models
{
    public class DataImportViewModel
    {
        public string NotAllowedMessage { get; set; }

        public bool IsAdmin { get; set; }

        public string ResultText { get; set; }

        public string PageTitle { get; internal set; }

        [Required, Microsoft.Web.Mvc.FileExtensions(Extensions = "csv", ErrorMessage = "Specify a CSV file. (Comma-separated values)")]
        public HttpPostedFileBase JobProfileImportDataFile { get; set; }

        public string InstructionsText { get; set; }
    }
}