using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models
{
    public class MigrationToolViewModel
    {
        public MigrationToolViewModel()
        {
            JobProfiles = new List<JobProfile>();
            SocCodes = new List<SocCode>();
        }

        public List<JobProfile> JobProfiles { get; set; }

        public List<SocCode> SocCodes { get; set; }
    }
}