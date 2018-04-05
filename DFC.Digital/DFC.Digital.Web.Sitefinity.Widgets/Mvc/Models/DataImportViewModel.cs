namespace DFC.Digital.Web.Sitefinity.Widgets.Mvc.Models
{
    public class DataImportViewModel
    {
        public string NotAllowedMessage { get; set; }

        public bool IsAdmin { get; set; }

        public string ResultText { get; set; }

        public string PageTitle { get; internal set; }
    }
}