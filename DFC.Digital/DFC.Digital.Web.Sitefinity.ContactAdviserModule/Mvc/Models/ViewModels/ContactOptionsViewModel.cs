using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DFC.Digital.Web.Sitefinity.ContactUsModule.Mvc.Models
{
    public class ContactOptionsViewModel
    {
        public ContactUsOption ContactUsOption { get; set; }

        public string Title { get; set; }
    }
}