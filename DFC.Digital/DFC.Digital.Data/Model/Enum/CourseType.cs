using System.ComponentModel.DataAnnotations;

namespace DFC.Digital.Data.Model
{
    public enum CourseType
    {
        [Display(Name = "All", Order = 1)]
        All,

        [Display(Name = "Classroom based", Order = 2)]
        ClassroomBased,

        [Display(Name = "Distance learning", Order = 3)]
        DistanceLearning,

        [Display(Name = "Online", Order = 4)]
        Online,

        [Display(Name = "Work Based", Order = 5)]
        WorkBased
    }
}
