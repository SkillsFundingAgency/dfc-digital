using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DFC.Digital.Web.Sitefinity.ContactUsModule.Mvc.Models
{
    public class ContactUsSession
    {
        public ContactOption ContactOption { get; set; }

        public FeedbackQuestionType FeedbackQuestionType { get; set; }

        public ContactAdivserQuestionType ContactAdviserQuestionType { get; set; }

        public string Message { get; set; }

    }
}