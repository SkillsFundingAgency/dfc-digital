﻿using DFC.Digital.Web.Core;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace DFC.Digital.Web.Sitefinity.ContactUsModule.Mvc.Models
{
    public class PersonalContactDetails
    {
        [Display(Name = "Title")]
        [Required(ErrorMessage = "Select your title")]
        public string Title { get; set; }

        [StringLength(50, ErrorMessage = "First name too long (max. 50)")]
        [Display(Name = "First name")]
        [Required(ErrorMessage = "Enter your first name")]
        public string Firstname { get; set; }

        [StringLength(50, ErrorMessage = "Last name too long (max. 50)")]
        [Display(Name = "Last name")]
        [Required(ErrorMessage = "Enter your last name")]
        public string Lastname { get; set; }

        [StringLength(1000, ErrorMessage = "Feedback too long (max. 1000)")]
        [Display(Name = "Enter your questions for the advisers in the box below.")]
        [Required(ErrorMessage = "Enter your feedback")]
 
        [Display(Name = "Email address")]
        [Required(ErrorMessage = "Enter your email address")]
        [EmailAddress(ErrorMessage = "Enter a valid email address")]
        public string EmailAddress { get; set; }

        [Compare("EmailAddress", ErrorMessage = "Email addresses don't match")]
        [Display(Name = "Confirm email address")]
        public string ConfirmEmailAddress { get; set; }

       
      
    }
}