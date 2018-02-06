using DFC.Digital.Data.Model;
using System.Collections.Generic;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models
{
    public class JobProfileByCategoryViewModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public IEnumerable<JobProfile> JobProfiles { get; set; }

        public string JobProfileUrl { get; set; }
    }
}