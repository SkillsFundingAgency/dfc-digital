﻿using System;
using System.ComponentModel.DataAnnotations;

namespace DFC.Digital.Web.Sitefinity.ContactUsModule.Mvc.Models
{
    public class ContactUsViewModel
    {
        public string Title { get; set; }

        [Required(ErrorMessage = "Enter your first name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Enter your last name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Enter your email")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }

        public string DobDay { get; set; }

        public string DobMonth { get; set; }

        public string DobYear { get; set; }

        [Required(ErrorMessage = "Enter a valid postcode")]
        [RegularExpression(@"([Gg][Ii][Rr] 0[Aa]{2})|((([A-Za-z][0-9]{1,2})|(([A-Za-z][A-Ha-hJ-Yj-y][0-9]{1,2})|(([A-Za-z][0-9][A-Za-z])|([A-Za-z][A-Ha-hJ-Yj-y][0-9][A-Za-z]?))))\s?[0-9][A-Za-z]{2})", ErrorMessage = "Please enter a valid UK Post code")]
        public string PostCode { get; set; }

        [Required(ErrorMessage = "Enter your message")]
        public string Message { get; set; }

        public bool IsContactable { get; set; }

        [Range(typeof(bool), "true", "true", ErrorMessage = "You must accept the terms and conditions")]
        public bool TermsAndConditions { get; set; }

        public string TandCChecked => TermsAndConditions ? "checked" : string.Empty;

        [Required(ErrorMessage = "Confirm your email")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [Compare("Email", ErrorMessage = "the email addresses do not match")]
        public string EmailConfirm { get; set; }

        public string NextPageUrl { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string PageTitle { get; set; }

        public string PageIntroduction { get; set; }

        public string PageIntroductionTwo { get; set; }

        public FormState FormState { get; set; }

        public string FormStateValue { get; set; }

        //public ContactOption ContactOption { get; set; }

        public object ContactAdivserQuestionType { get; set; }

        public object FeedbackQuestionType { get; set; }
    }
}