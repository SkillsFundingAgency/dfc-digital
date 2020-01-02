using System.ComponentModel.DataAnnotations;

namespace DFC.Digital.Data.Model
{
    public enum CourseType
    {
        [Display(Name = "All", Order = 1)]
        All,

        [Display(Name = "Classroom based", Order = 2)]
        ClassroomBased,

        [Display(Name = "Online", Order = 3)]
        Online,

        [Display(Name = "Work based", Order = 4)]
        WorkBased
    }
}
