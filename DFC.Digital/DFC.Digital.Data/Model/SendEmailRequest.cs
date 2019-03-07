using System;

namespace DFC.Digital.Data.Model
{
    public class SendEmailRequest
    {
        public string TemplateName { get; set; }

        public string Message { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string PostCode { get; set; }

        public string ContactAdvisorQuestionType { get; set; }

        public string FeedbackQuestionType { get; set; }

        public bool IsContactable { get; set; }

        public string ContactOption { get; set; }

        public bool TermsAndConditions { get; set; }
    }
}
