namespace DFC.Digital.Data.Model
{
    public class ContactUs
    {
        public ContactUsOption ContactUsOption { get; set; }

        public GeneralFeedback GeneralFeedback { get; set; }

        public TechnicalFeedback TechnicalFeedback { get; set; }

        public ContactAnAdviserFeedback ContactAnAdviserFeedback { get; set; }

        public PersonalContactDetails PersonalContactDetails { get; set; }

        public bool TermsAndConditions { get; set; }
    }
}