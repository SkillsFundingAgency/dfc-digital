namespace DFC.Digital.Web.Sitefinity.ContactUsModule.Mvc.Models
{
    public class ContactUsViewModel
    {
        public string PageTitle { get; set; }

        public string PageIntroduction { get; set; }

        public string PageIntroductionTwo { get; set; }

        public bool ShouldAskAgeAndPostcode { get; set; }

        public PersonalContactDetails PersonalContactDetails { get; set; }

        public string NextPageUrl { get; set; }
    }
}