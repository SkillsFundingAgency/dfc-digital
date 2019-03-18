namespace DFC.Digital.Web.Sitefinity.ContactUsModule.Mvc.Models
{
    public class ContactAdviserViewModel
    {
        public string Title { get; set; }

        public string Hint { get; set; }

        public string NextPageUrl { get; set; }

        public string CharacterLimitMessage { get; set; }

        public ContactAnAdviserFeedback ContactAnAdviserFeedback { get; set; }
    }
}