using System.ComponentModel.DataAnnotations;

namespace DFC.Digital.Web.Sitefinity.ContactUsModule.Mvc.Models
{
    public class TechnicalFeedback
    {
        [Required(ErrorMessage = "Enter a message describing the issue")]
        [StringLength(1000, ErrorMessage = "Feedback too long (max. 1000)")]
        [Display(Name = "Enter your feedback in the box below. If you're commenting on particular pages, list them.")]
        public string Message { get; set; }
    }
}