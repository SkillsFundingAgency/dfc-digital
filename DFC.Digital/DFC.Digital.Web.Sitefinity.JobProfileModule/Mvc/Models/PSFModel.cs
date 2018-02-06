using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models
{
    public class PSFModel
    {
        public List<PSFSection> Sections { get; set; }

        public PSFSection Section { get; set; }

        public string OptionsSelected { get; set; }
    }
}