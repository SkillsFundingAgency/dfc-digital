namespace DFC.Digital.Web.Sitefinity.ContactUsModule.Mvc.Models
{
    public class ContactUsWithConsentViewModel : ContactUsViewModel
    {
        public ConsentDetails ConsentDetails { get; set; } = new ConsentDetails();
    }
}