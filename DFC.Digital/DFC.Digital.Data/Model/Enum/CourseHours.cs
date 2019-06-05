using System.ComponentModel.DataAnnotations;

namespace DFC.Digital.Data.Model
{
    public enum CourseHours
    {
        [Display(Name = "All", Order = 1)]
        All = 1,

        [Display(Name = "Full Time", Order = 2)]
        FullTime = 2,

        [Display(Name = "Part Time", Order = 3)]
        PartTime = 3,

        [Display(Name = "Flexible", Order = 4)]
        Flexible = 4,
    }
}
