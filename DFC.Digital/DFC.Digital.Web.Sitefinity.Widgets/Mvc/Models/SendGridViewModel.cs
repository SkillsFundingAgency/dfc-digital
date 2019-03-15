using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DFC.Digital.Web.Sitefinity.Widgets.Mvc.Models
{
    public class SendGridViewModel
    {
        public string TemplateName { get; set; }

        public string EmailAddress { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string SendResult { get; set; }
    }
}