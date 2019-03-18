using System;
using System.ComponentModel.DataAnnotations;

namespace DFC.Digital.Web.Sitefinity.ContactUsModule.Mvc.Models
{
    public class ContactUsTechnicalViewModel
    {
        public ContactOption ContactOption { get; set; }

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