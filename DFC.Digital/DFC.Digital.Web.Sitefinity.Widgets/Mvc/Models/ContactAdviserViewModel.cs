using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DFC.Digital.Web.Sitefinity.Widgets.Mvc.Models
{
    public class ContactAdviserViewModel
    {
        public string FirstName { get; set; }

        public string SurName { get; set; }

        public string Email { get; set; }

        public string ConfirmEmail { get; set; }

        public string ContactOptionType { get; set; }

        public string DateOfBirth { get; set; }

        public string Postcode { get; set; }

        public string ContactAdivserQuestionType { get; set; }

        public string FeedbackQuestionType { get; set; }

        public bool IsContactable { get; set; }

        public bool TermsAndConditionsChecked { get; set; }

        public string ContactAdvisorSendToEmailAddress { get; set; }

        public string FeedbackSendToEmailAddress { get; set; }

        public string TechnicalSupportSendToEmailAddress { get; set; }
    }
}