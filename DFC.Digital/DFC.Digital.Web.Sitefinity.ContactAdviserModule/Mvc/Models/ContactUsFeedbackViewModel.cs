using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DFC.Digital.Web.Sitefinity.ContactUsModule.Mvc.Models
{
    public class ContactUsFeedbackViewModel
    {
        public ContactOption ContactOption { get; set; }

        [EnumDataType(typeof(FeedbackQuestionType), ErrorMessage = "Choose a reason for your feedback")]
        public FeedbackQuestionType FeedbackQuestionType { get; set; }

        [Required(ErrorMessage = "Enter a message describing the issue")]
        public string Message { get; set; }

        public string MessageLabel { get; set; }

        public string NextPageUrl { get; set; }

        public string Title { get; set; }

        public string PageIntroduction { get; set; }

        public string PersonalInformation { get; set; }

        public string CharacterLimit { get; set; }
    }
}