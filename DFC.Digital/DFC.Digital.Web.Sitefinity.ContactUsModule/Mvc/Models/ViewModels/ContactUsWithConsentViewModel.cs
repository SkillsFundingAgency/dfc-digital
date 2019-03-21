namespace DFC.Digital.Web.Sitefinity.ContactUsModule.Mvc.Models
{
    public class ContactUsWithConsentViewModel : ConsentDetails
    {
        public string PageTitle { get; set; }

        public string PageIntroduction { get; set; }

        public string TermsAndConditionsText { get; set; }

        public string DoYouWantUsToContactUsText { get; set; }

        public string SendButtonText { get; set; }
    }
}