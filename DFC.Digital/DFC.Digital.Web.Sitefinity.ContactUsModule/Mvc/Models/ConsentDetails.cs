using DFC.Digital.Web.Core;
using System.ComponentModel.DataAnnotations;

namespace DFC.Digital.Web.Sitefinity.ContactUsModule.Mvc.Models
{
    public class ConsentDetails : PersonalContactDetails
    {
        public bool IsContactable { get; set; }
       
    }
}