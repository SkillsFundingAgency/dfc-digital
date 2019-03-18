using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DFC.Digital.Web.Sitefinity.ContactUsModule.Mvc.Models
{
    public class ContactUsFeedbackViewModel
    {
        public GeneralFeedback GeneralFeedback { get; set; }

        public string MessageLabel { get; set; }

        public string NextPageUrl { get; set; }

        public string Title { get; set; }

        public string PageIntroduction { get; set; }

        public string PersonalInformation { get; set; }

        public string CharacterLimit { get; set; }
    }
}