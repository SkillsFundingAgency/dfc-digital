using System.ComponentModel.DataAnnotations;

namespace DFC.Digital.Web.Sitefinity.ContactUsModule.Mvc.Models
{
    public enum ContactAdivserQuestionType
    {
        [Display(Name = "Careers", Order = 1)]
        Careers = 1,

        [Display(Name = "Qualifications", Order = 2)]
        Qualifications = 2,

        [Display(Name = "Courses", Order = 3)]
        CourseSearch = 3,

        [Display(Name = "Funding", Order = 4)]
        Funding = 4,

        [Display(Name = "Website", Order = 5)]
        Website = 5,

        [Display(Name = "Customer service", Order = 6)]
        CustomerService = 6,

        [Display(Name = "Something else", Order = 7)]
        General = 7
    }
}