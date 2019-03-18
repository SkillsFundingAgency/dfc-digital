using System.ComponentModel.DataAnnotations;

namespace DFC.Digital.Web.Sitefinity.ContactUsModule.Mvc.Models
{
    public enum ContactOption
    {
        [Display(Name = "Contact an adviser", Order = 1)]
        ContactAdviser,

        [Display(Name = "Report a technical issue", Order = 2)]
        Technical,

        [Display(Name = "Give feedback", Order = 3)]
        Feedback
    }
}