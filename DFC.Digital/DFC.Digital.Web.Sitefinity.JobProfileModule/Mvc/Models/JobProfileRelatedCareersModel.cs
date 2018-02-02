using DFC.Digital.Data.Model;
using System.Collections.Generic;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models
{
    public class JobProfileRelatedCareersModel
    {
        public string Title { get; set; }

        public IEnumerable<JobProfileRelatedCareer> RelatedCareers { get; set; }
    }
}