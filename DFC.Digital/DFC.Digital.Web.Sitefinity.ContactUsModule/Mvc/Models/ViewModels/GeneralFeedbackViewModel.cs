using System.ComponentModel.DataAnnotations;

namespace DFC.Digital.Web.Sitefinity.ContactUsModule.Mvc.Models
{
    public class GeneralFeedbackViewModel : GeneralFeedback
    {
        public string Title { get; set; }

        public string Hint { get; set; }

        public string CharacterLimit { get; set; }

        public string MessageLabel { get; set; }

        public string PersonalInformation { get; set; }

        public string NextPage { get; set; }
    }
}