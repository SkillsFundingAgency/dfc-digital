using System.ComponentModel.DataAnnotations;

namespace DFC.Digital.Web.Sitefinity.ContactUsModule.Mvc.Models
{
    public class GeneralFeedbackViewModel : GeneralFeedback
    {
        public string Title { get; set; }

        public string Hint { get; set; } = "Do not include any personal or sign in information.";

        public string CharacterLimit { get; set; } = "Character limit is 1000.";
    }
}