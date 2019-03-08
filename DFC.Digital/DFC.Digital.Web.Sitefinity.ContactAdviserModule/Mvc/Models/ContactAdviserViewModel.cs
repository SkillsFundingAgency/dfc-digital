using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DFC.Digital.Web.Sitefinity.ContactAdviserModule.Mvc.Models
{
    public class ContactAdviserViewModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string ContactOption { get; set; }

        public string DOB { get; set; }

        public string PostCode { get; set; }

        public string ContactAdviserQuestionType { get; set; }

        public string Message { get; set; }

        public bool IsContactable { get; set; }

        public string FeedbackQuestionType { get; set; }

        public bool TandCChecked { get; set; }
    }
}