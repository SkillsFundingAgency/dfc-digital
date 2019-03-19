using System.ComponentModel.DataAnnotations;

namespace DFC.Digital.Web.Sitefinity.ContactUsModule.Mvc.Models
{
    public class ContactUsOption
    {
        [EnumDataType(typeof(ContactOption), ErrorMessage = "Choose a reason for contacting us")]
        public ContactOption? ContactOptionType { get; set; }
    }
}