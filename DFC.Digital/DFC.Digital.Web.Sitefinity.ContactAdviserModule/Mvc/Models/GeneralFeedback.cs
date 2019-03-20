using System.ComponentModel.DataAnnotations;

namespace DFC.Digital.Web.Sitefinity.ContactUsModule.Mvc.Models
{
    public class GeneralFeedback
    {
        [StringLength(1000, ErrorMessage = "Feedback too long (max. 1000)")]
        [Display(Name = "Enter your feedback in the box below. If you're commenting on particular pages, list them.")]
        [Required(ErrorMessage = "Enter your feedback")]
        public string Feedback { get; set; }
    }
}