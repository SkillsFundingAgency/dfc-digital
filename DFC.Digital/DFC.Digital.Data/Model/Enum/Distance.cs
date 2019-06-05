using System.ComponentModel.DataAnnotations;

namespace DFC.Digital.Data.Model.Enum
{
    public enum Distance
    {
        [Display(Name = "5 Miles", Order = 1)]
        FiveMiles = 5,

        [Display(Name = "10 Miles", Order = 2)]
        TenMiles = 10,

        [Display(Name = "15 Miles", Order = 3)]
        FifteenMiles = 15,

        [Display(Name = "30 Miles", Order = 4)]
        ThirtyMiles = 30,

        [Display(Name = "45 Miles", Order = 5)]
        FortyFiveMiles = 45,

        [Display(Name = "60 Miles", Order = 6)]
        SixtyMiles = 60,

        [Display(Name = "Work Based", Order = 7)]
        SeventyFiveMiles = 77
    }
}
