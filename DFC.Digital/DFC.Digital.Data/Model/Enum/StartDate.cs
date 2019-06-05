using System.ComponentModel.DataAnnotations;

namespace DFC.Digital.Data.Model
{
    public enum StartDate
    {
        [Display(Name = "Anytime", Order = 1)]
        Anytime = 1,

        [Display(Name = "From Today", Order = 2)]
        FromToday = 2,

        [Display(Name = "Select date from", Order = 3)]
        SelectDateFrom = 3
    }
}
