using DFC.Digital.Web.Core;
using System.ComponentModel.DataAnnotations;

namespace DFC.Digital.Web.Sitefinity.ContactUsModule.Mvc.Models
{
    public class GeneralFeedback
    {
        [TrimCarriageReturnStringLength(1000, ErrorMessage = "Message must be 1000 characters or less")]
        [Display(Name = "Enter your feedback in the box below. If you're commenting on particular pages, list them.")]
        [Required(ErrorMessage = "Enter your feedback")]
        public string Feedback { get; set; }

        [EnumDataType(typeof(FeedbackQuestionType), ErrorMessage = "Choose a reason for your feedback")]
        [Required(ErrorMessage = "Choose a reason for your feedback")]
        public FeedbackQuestionType FeedbackQuestionType { get; set; }
    }
}