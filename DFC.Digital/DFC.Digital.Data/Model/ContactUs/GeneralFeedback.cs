using System.ComponentModel.DataAnnotations;

namespace DFC.Digital.Data.Model
{
    public class GeneralFeedback
    {
        [EnumDataType(typeof(FeedbackQuestionType), ErrorMessage = "Choose a reason for your feedback")]
        public FeedbackQuestionType FeedbackQuestionType { get; set; }

        [Required(ErrorMessage = "Enter a message describing the issue")]
        public string Message { get; set; }

    }
}