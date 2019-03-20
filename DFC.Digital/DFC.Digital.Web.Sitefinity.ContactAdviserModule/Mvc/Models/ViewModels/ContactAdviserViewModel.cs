using System.ComponentModel.DataAnnotations;

namespace DFC.Digital.Web.Sitefinity.ContactUsModule.Mvc.Models
{
    public class ContactAdviserViewModel
    {
        public string Title { get; set; }

        public string Hint { get; set; }

        public ContactAnAdviserFeedback ContactAnAdviserFeedback { get; set; }

        public string CharacterLimit { get; set; }

        public string NextPageUrl { get; set; }

        [EnumDataType(typeof(ContactAdivserQuestionType), ErrorMessage = "Choose a category")]
        public ContactAdivserQuestionType ContactAdviserQuestionType { get; set; }
    }
}