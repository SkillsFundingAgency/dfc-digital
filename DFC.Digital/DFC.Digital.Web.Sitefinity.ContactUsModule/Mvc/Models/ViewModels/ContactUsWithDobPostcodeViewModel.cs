using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace DFC.Digital.Web.Sitefinity.ContactUsModule.Mvc.Models
{
    public class ContactUsWithDobPostcodeViewModel : DateOfBirthPostcodeDetails
    {
        public string PageTitle { get; set; }

        public string PageIntroduction { get; set; }

        public string PageIntroductionTwo { get; set; }

        public string TermsAndConditionsText { get; set; }

        public string DateOfBirthHint { get; set; }

        public string PostcodeHint { get; set; }

        public string SendButtonText { get; set; }
    }
}