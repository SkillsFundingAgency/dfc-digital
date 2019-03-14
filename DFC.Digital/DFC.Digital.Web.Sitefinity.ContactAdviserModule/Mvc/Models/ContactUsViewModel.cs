using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Http.ModelBinding;

namespace DFC.Digital.Web.Sitefinity.ContactUsModule.Mvc.Models
{
    public enum ContactOption
    {
        ContactAdviser,
        Technical,
        Feedback
    }

    public enum ContactAdivserQuestionType
    {
        Careers,
        Qualifications,
        Findingacourse,
        Generalfeedback,
        Funding
    }

    public enum FeedbackQuestionType
    {
        General,
        Funding,
        Careers,
        Qualifications,
        CourseSearch,
        Website,
        CustomerService
    }

    public class ContactUsViewModel
    {
        [Required(ErrorMessage = "Choose a reason for contacting us")] 
        [EnumDataType(typeof(ContactOption))]
        public ContactOption ContactOption { get; set; }

        public ContactAdivserQuestionType ContactAdivserQuestionType { get; set; }

        public FeedbackQuestionType FeedbackQuestionType { get; set; }

        public string Title { get; set; }

        [Required(ErrorMessage = "Enter your first name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Enter your last name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Enter your email")]
        public string Email { get; set; }

        public int DOBDay { get; set; }

        public int DOBMonth { get; set; }

        public int DOBYear { get; set; }

        [Required(ErrorMessage = "Enter a valid postcode")]
        public string PostCode { get; set; }

        public string Message { get; set; }

        public bool IsContactable { get; set; }

        [Required(ErrorMessage = "You must accept the terms and conditions")]
        public bool TandCChecked { get; set; }

        [Required(ErrorMessage = "Confirm your email")]
        public string EmailConfirm { get; set; }

        public string NextPageUrl { get; set; }
    }
}