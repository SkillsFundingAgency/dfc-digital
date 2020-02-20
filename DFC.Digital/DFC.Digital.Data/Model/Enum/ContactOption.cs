using System.ComponentModel.DataAnnotations;

namespace DFC.Digital.Data.Model.Enum
{
    public enum ContactOption
    {
        [Display(Name = "Contact an adviser", Order = 1)]
        ContactAdviser = 1,

        [Display(Name = "Report a technical issue", Order = 2)]
        Technical = 2,

        [Display(Name = "Give feedback", Order = 3)]
        Feedback = 3
    }
}