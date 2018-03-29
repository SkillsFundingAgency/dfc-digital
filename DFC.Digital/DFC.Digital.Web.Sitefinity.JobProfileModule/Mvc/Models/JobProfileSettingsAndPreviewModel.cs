namespace DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models
{
    public class JobProfileSettingsAndPreviewModel
    {
        public string DefaultJobProfileUrl { get; set; }

        public bool RefreshAllWidgets { get; set; }

        public bool ShouldSetVocCookie { get; set; }

        public string VocSetPersonalisationCookieNameAndValue { get; set; }
    }
}