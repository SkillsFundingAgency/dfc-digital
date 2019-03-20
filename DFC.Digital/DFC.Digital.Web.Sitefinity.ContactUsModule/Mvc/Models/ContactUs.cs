namespace DFC.Digital.Web.Sitefinity.ContactUsModule.Mvc.Models
{
    public class ContactUs
    {
        public ContactUsOption ContactUsOption { get; set; }

        public GeneralFeedback GeneralFeedback { get; set; }

        public TechnicalFeedback TechnicalFeedback { get; set; }

        public ContactAnAdviserFeedback ContactAnAdviserFeedback { get; set; }
    }
}