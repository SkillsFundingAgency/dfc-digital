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
            Registrations = new List<OcRegistration>();
            Restrictions = new List<OcRestriction>();
            ApprenticeshipLinks = new List<OcApprenticeshipLink>();
            CollegeLinks = new List<OcCollegeLink>();
            UniversityLinks = new List<OcUniversityLink>();
            ApprenticeshipRequirements = new List<OcApprenticeshipRequirement>();
            CollegeRequirements = new List<OcCollegeRequirement>();
            UniversityRequirements = new List<OcUniversityRequirement>();
            Uniforms = new List<OcUniform>();
            Locations = new List<OcLocation>();
            Environments = new List<OcEnvironment>();

            //FlatTaxaItems = new List<FlatTaxaItem>();
            HiddenAlternativeTitles = new List<OcHiddenAlternativeTitle>();
            ApprenticeshipEntryRequirements = new List<ApprenticeshipEntryRequirement>();
            WorkingPatterns = new List<OcWorkingPattern>();
            JobProfileCategories = new List<OcJobProfileCategory>();
            ApprenticeshipStandards = new List<OcApprenticeshipStandard>();
            UniversityEntryRequirements = new List<OcUniversityEntryRequirement>();
            CollegeEntryRequirements = new List<OcCollegeEntryRequirement>();
            JobProfileSpecialisms = new List<OcJobProfileSpecialism>();
            WorkingPatternDetails = new List<OcWorkingPatternDetail>();
            WorkingHoursDetails = new List<OcWorkingHoursDetail>();
            SocCodes = new List<OcSocCode>();
            JobProfiles = new List<OcJobProfile>();
            FilteringQuestions = new List<OcFilteringQuestion>();
        }

        public List<OcRegistration> Registrations { get; set; }

        public List<OcRestriction> Restrictions { get; set; }

        public List<OcApprenticeshipLink> ApprenticeshipLinks { get; set; }

        public List<OcCollegeLink> CollegeLinks { get; set; }

        public List<OcUniversityLink> UniversityLinks { get; set; }

        public List<OcApprenticeshipRequirement> ApprenticeshipRequirements { get; set; }

        public List<OcCollegeRequirement> CollegeRequirements { get; set; }

        public List<OcUniversityRequirement> UniversityRequirements { get; set; }

        public List<OcUniform> Uniforms { get; set; }

        public List<OcLocation> Locations { get; set; }

        public List<OcEnvironment> Environments { get; set; }

        //public List<FlatTaxaItem> FlatTaxaItems { get; set; }
        public List<OcHiddenAlternativeTitle> HiddenAlternativeTitles { get; set; }

        public List<ApprenticeshipEntryRequirement> ApprenticeshipEntryRequirements { get; set; }

        public List<OcWorkingPattern> WorkingPatterns { get; set; }

        public List<OcJobProfileCategory> JobProfileCategories { get; set; }

        public List<OcApprenticeshipStandard> ApprenticeshipStandards { get; set; }

        public List<OcUniversityEntryRequirement> UniversityEntryRequirements { get; set; }

        public List<OcCollegeEntryRequirement> CollegeEntryRequirements { get; set; }

        public List<OcJobProfileSpecialism> JobProfileSpecialisms { get; set; }

        public List<OcWorkingPatternDetail> WorkingPatternDetails { get; set; }

        public List<OcWorkingHoursDetail> WorkingHoursDetails { get; set; }

        public List<OcSocCode> SocCodes { get; set; }

        public List<OcJobProfile> JobProfiles { get; set; }

        public List<OcFilteringQuestion> FilteringQuestions { get; set; }

        public string SelectedItemType { get; set; }

        public string Message { get; set; }

        public string ErrorMessage { get; set; }
    }
}