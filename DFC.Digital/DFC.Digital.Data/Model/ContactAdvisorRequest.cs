using Newtonsoft.Json;
using System;

namespace DFC.Digital.Data.Model
{
    public class ContactAdvisorRequest
    {
        public string TemplateName { get; set; }

        public string Message { get; set; }

        [JsonIgnore]
        public string FirstName { get; set; }

        [JsonIgnore]
        public string LastName { get; set; }

        [JsonIgnore]
        public string Email { get; set; }

        [JsonIgnore]
        public DateTime DateOfBirth { get; set; }

        [JsonIgnore]
        public string PostCode { get; set; }

        public string ContactAdviserQuestionType { get; set; }

        public string FeedbackQuestionType { get; set; }

        public bool IsContactable { get; set; }

        public string ContactOption { get; set; }

        public bool TermsAndConditions { get; set; }

        public string Subject { get; set; }
    }
}
