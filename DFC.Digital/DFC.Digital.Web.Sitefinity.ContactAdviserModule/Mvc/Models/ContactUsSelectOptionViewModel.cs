using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DFC.Digital.Web.Sitefinity.ContactUsModule.Mvc.Models
{
    public class ContactUsSelectOptionViewModel
    {
        [EnumDataType(typeof(ContactOption), ErrorMessage = "Choose a reason for contacting us")]
        public ContactOption? ContactOptionType { get; set; }
    }
}