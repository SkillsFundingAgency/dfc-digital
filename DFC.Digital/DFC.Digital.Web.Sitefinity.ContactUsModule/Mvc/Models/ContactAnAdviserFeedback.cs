using DFC.Digital.Web.Sitefinity.ContactUsModule.Extensions;
using System.ComponentModel.DataAnnotations;

namespace DFC.Digital.Web.Sitefinity.ContactUsModule.Mvc.Models
{
    public class ContactAnAdviserFeedback
    {
        [Required(ErrorMessage = "Enter a message describing the issue")]
        [CustomStringLength(1000, ErrorMessage = "Message must be 1000 characters or less")]
        [Display(Name = "Enter your query for the adviser.")]
        public string Message { get; set; }

        [EnumDataType(typeof(ContactAdivserQuestionType), ErrorMessage = "Choose a category")]
        [Required(ErrorMessage = "Choose a category")]
        public ContactAdivserQuestionType ContactAdviserQuestionType { get; set; }
    }
}