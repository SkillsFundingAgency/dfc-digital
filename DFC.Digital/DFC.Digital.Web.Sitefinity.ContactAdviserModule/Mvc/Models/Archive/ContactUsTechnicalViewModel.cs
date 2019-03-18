using DFC.Digital.Data.Model;

namespace DFC.Digital.Web.Sitefinity.ContactUsModule.Mvc.Models
{
    public class ContactUsTechnicalViewModel
    {
        public TechnicalFeedback TechnicalFeedback { get; set; }

        public string MessageLabel { get; set; }

        public string NextPageUrl { get; set; }

        public string Title { get; set; }

        public string PageIntroduction { get; set; }

        public string PersonalInformation { get; set; }

        public string CharacterLimit { get; set; }
    }
}