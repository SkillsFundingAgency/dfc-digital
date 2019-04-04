using System.ComponentModel.DataAnnotations;

namespace DFC.Digital.Web.Sitefinity.ContactUsModule.Mvc.Models
{
    public class ContactAdviserViewModel : ContactAnAdviserFeedback
    {
        public string Title { get; set; }

        public string Hint { get; set; }

        public string CharacterLimit { get; set; }

        public string NextPage { get; set; }
    }
}