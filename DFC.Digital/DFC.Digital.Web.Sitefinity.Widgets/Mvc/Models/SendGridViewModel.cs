using System;

namespace DFC.Digital.Web.Sitefinity.Widgets.Mvc.Models
{
    public class SendGridViewModel
    {
        public string TemplateName { get; set; }

        public string EmailAddress { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string SendResult { get; set; }

        public bool IsContactable { get; set; }

        public string ContactOption { get; set; }

        public string ContactAdviserQuestionType { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string FeedbackQuestionType { get; set; }

        public string Message { get; set; }

        public string Postcode { get; set; }

        public bool AcceptTermsAndConditions { get; set; }
    }
}