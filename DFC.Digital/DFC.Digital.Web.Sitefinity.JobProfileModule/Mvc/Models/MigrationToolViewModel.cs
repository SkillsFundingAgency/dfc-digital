using DFC.Digital.Data.Model;
using DFC.Digital.Data.Model.OrchardCore;
using DFC.Digital.Data.Model.OrchardCore.Uniform;
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
            Uniforms = new List<OcUniform>();
            ApprenticeshipLinks = new List<OcApprenticeshipLink>();
            SocCodes = new List<SocCode>();
            FlatTaxaItems = new List<FlatTaxaItem>();
            ApprenticeshipEntryRequirements = new List<ApprenticeshipEntryRequirement>();
        }

        public List<JobProfile> JobProfiles { get; set; }

        public List<OcUniform> Uniforms { get; set; }

        public List<OcApprenticeshipLink> ApprenticeshipLinks { get; set; }

        public List<SocCode> SocCodes { get; set; }

        public List<FlatTaxaItem> FlatTaxaItems { get; set; }

        public List<ApprenticeshipEntryRequirement> ApprenticeshipEntryRequirements { get; set; }

        public string SelectedItemType { get; set; }

        public string Message { get; set; }
    }
}