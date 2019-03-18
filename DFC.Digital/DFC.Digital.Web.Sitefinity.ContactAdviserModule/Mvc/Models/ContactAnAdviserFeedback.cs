using System.ComponentModel.DataAnnotations;

namespace DFC.Digital.Data.Model
{
    public class ContactAnAdviserFeedback
    {
        [EnumDataType(typeof(ContactAdivserQuestionType), ErrorMessage = "Choose what your query is about")]
        public ContactAdivserQuestionType ContactAdviserQuestionType { get; set; }

        [Required(ErrorMessage = "Enter a message describing the issue")]
        public string Message { get; set; }
    }
}