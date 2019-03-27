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
        CourseSearch,

        [Display(Name = "Funding", Order = 4)]
        Funding,

        [Display(Name = "Website", Order = 5)]
        Website,

        [Display(Name = "Customer service", Order = 6)]
        CustomerService,

        [Display(Name = "Something else", Order = 7)]
        General
    }
}