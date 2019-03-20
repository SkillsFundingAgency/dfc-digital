using System.ComponentModel.DataAnnotations;

namespace DFC.Digital.Web.Sitefinity.ContactUsModule.Mvc.Models
{
    public class ContactAnAdviserFeedback
    {
        [Required(ErrorMessage = "Enter a message describing the issue")]
        [StringLength(1000, ErrorMessage = "Message too long (max. 1000)")]
        [Display(Name = "Enter your query for the adviser.")]
        public string Message { get; set; }
    }
}