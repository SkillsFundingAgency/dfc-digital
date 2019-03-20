using System.ComponentModel.DataAnnotations;

namespace DFC.Digital.Web.Sitefinity.ContactUsModule.Mvc.Models
{
    public class GeneralFeedbackViewModel
    {
        public string Title { get; set; }

        public string Hint { get; set; } = "Do not include any personal or sign in information.";

        public GeneralFeedback GeneralFeedback { get; set; }

        public string CharacterLimit { get; set; } = "Character limit is 1000.";

        public string NextPageUrl { get; set; }

        [EnumDataType(typeof(FeedbackQuestionType), ErrorMessage = "Choose a reason for your feedback")]
        public FeedbackQuestionType FeedbackQuestionType { get; set; }
    }
}