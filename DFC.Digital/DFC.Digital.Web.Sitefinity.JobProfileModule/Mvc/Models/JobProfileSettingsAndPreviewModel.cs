namespace DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models
{
    public class JobProfileSettingsAndPreviewModel
    {
        public string DefaultJobProfileUrl { get; set; }

        public bool RefreshAllWidgets { get; set; }

        public bool VocSetPersonalisationCookie { get; set; }

        public string VocSetPersonalisationCookieNameAndValue { get; set; }

        public bool SetCookieServerSide { get; set; }

        public string VocJobProfileUrl { get; set; }
    }
}