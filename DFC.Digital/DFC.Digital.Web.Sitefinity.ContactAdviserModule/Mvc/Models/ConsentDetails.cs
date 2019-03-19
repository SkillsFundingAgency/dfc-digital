namespace DFC.Digital.Web.Sitefinity.ContactUsModule.Mvc.Models
{
    public class ConsentDetails
    {
        public bool IsContactable { get; set; }

        [Accept(ErrorMessage = "You must accept our Terms and Conditions")]
        [Display(Name = "I accept the <a href=\"/about-us/terms-and-conditions\">terms and conditions</a> and I am 13 or over")]
        public bool AcceptTermsAndConditions { get; set; }
    }
}