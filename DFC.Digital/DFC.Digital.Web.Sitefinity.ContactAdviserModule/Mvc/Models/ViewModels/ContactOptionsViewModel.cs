using System.ComponentModel.DataAnnotations;

namespace DFC.Digital.Web.Sitefinity.ContactUsModule.Mvc.Models
{
    public class ContactOptionsViewModel
    {
        public string Title { get; set; }

        [Required(ErrorMessage = "Choose a reason for contacting us")]
        public ContactUsOption ContactUsOption { get; set; }
    }
}