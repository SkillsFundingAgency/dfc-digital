using System.ComponentModel.DataAnnotations;

namespace DFC.Digital.Data.Model
{
    public enum CourseType
    {
        [Display(Name = "All", Order = 1)]
        All = 1,

        [Display(Name = "Classroom based", Order = 2)]
        ClassroomBased = 2,

        [Display(Name = "Distance learning", Order = 3)]
        DistanceLearning = 3,

        [Display(Name = "Online", Order = 4)]
        Online = 4,

        [Display(Name = "Work Based", Order = 5)]
        WorkBased = 5
    }
}
