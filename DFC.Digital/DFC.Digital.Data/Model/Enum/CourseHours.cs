using System.ComponentModel.DataAnnotations;

namespace DFC.Digital.Data.Model
{
    public enum CourseHours
    {
        [Display(Name = "All", Order = 1)]
        All,

        [Display(Name = "Full Time", Order = 2)]
        Fulltime,

        [Display(Name = "Part Time", Order = 3)]
        PartTime,

        [Display(Name = "Flexible", Order = 4)]
        Flexible
    }
}
