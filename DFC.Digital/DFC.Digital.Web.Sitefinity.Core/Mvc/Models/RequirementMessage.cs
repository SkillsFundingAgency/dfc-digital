using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DFC.Digital.Web.Sitefinity.Core.Mvc.Models
{
    public class RequirementMessage : SitefinityMessage
    {
        public Guid JobProfileId { get; set; }

        public string JobProfileUrlName { get; set; }

        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Info { get; set; }
    }
}