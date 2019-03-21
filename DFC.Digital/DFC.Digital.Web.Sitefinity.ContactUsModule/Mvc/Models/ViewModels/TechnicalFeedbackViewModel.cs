namespace DFC.Digital.Web.Sitefinity.ContactUsModule.Mvc.Models
{
    public class TechnicalFeedbackViewModel
    {
        public string Title { get; set; }

        public string PageIntroduction { get; set; }

        public string MessageLabel { get; set; }

        public string PersonalInformation { get; set; }

        public TechnicalFeedback TechnicalFeedback { get; set; }

        public string CharacterLimit { get; set; }

        public string NextPageUrl { get; set; }
    }
}