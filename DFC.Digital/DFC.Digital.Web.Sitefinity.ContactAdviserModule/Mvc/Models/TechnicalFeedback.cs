﻿using System.ComponentModel.DataAnnotations;

namespace DFC.Digital.Web.Sitefinity.ContactUsModule.Mvc.Models
{
    public class TechnicalFeedback
    {
        [Required(ErrorMessage = "Enter a message describing the issue")]
        [StringLength(1000, ErrorMessage = "Feedback too long (max. 1000)")]
        [Display(Name = "Include links to the problem page and any page headings. This will help us to fix the issue more quickly.")]
        public string Message { get; set; }
    }
}