using System.ComponentModel.DataAnnotations;

namespace DFC.Digital.Data.Model
{
    public enum StartDate
    {
        [Display(Name = "Anytime", Order = 1)]
        Anytime,

        [Display(Name = "From today", Order = 2)]
        FromToday,

        [Display(Name = "Enter a date", Order = 3)]
        SelectDateFrom
    }
}
