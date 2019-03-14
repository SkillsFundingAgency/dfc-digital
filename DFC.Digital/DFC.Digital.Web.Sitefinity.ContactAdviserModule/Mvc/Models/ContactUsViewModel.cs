using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

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

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string DOBDay { get; set; }

        public string DOBMonth { get; set; }

        public string DOBYear { get; set; }

        public string PostCode { get; set; }

        public string Message { get; set; }

        public bool IsContactable { get; set; }

        public bool TandCChecked { get; set; }

        public string NextPageUrl { get; set; }
    }
}