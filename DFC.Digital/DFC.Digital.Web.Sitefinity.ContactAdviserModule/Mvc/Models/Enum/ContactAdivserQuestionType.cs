using System.ComponentModel.DataAnnotations;

namespace DFC.Digital.Web.Sitefinity.ContactUsModule.Mvc.Models
{
    public enum FeedbackQuestionType
    {
        [Display(Name = "Careers", Order = 1)]
        Careers,

        [Display(Name = "Qualifications", Order = 2)]
        Qualifications,

        [Display(Name = "Courses", Order = 3)]
        Findingacourse,

        [Display(Name = "Funding", Order = 4)]
        Funding,

        [Display(Name = "Something else", Order = 5)]
        Generalfeedback
    }
}