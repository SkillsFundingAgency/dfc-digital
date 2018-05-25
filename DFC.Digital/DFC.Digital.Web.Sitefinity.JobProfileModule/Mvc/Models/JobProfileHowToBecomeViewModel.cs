using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models
{
    /// <summary>
    /// Job Profile HowToBecome ViewModel
    /// </summary>
    public class JobProfileHowToBecomeViewModel
    {
        #region JobProfile Data

        public string Title { get; set; }

        public string UrlName { get; set; }

        public string HowToBecome { get; set; }

        public bool IsHTBCaDReady { get; set; }

        // How To Become section
        public string EntryRoutes { get; set; }

        // UNIVERSITY
        public string UniversityRelevantSubjects { get; set; }

        public string UniversityFurtherRouteInfo { get; set; }

        // choices
        public string UniversityRequirements { get; set; }

        // related
        public List<InfoItem> RelatedUniversityRequirement { get; set; }

        // related
        public List<LinkItem> RelatedUniversityLinks { get; set; }

        // COLLEGE
        public string CollegeRelevantSubjects { get; set; }

        public string CollegeFurtherRouteInfo { get; set; }

        // choices
        public string CollegeRequirements { get; set; }

        // related
        public List<InfoItem> RelatedCollegeRequirements { get; set; }

        public List<LinkItem> RelatedCollegeLinks { get; set; }

        // APPRENTICESHIP
        public string ApprenticeshipRelevantSubjects { get; set; }

        public string ApprenticeshipFurtherRouteInfo { get; set; }

        // choices
        public string ApprenticeshipRequirements { get; set; }

        // related
        public List<InfoItem> RelatedApprenticeshipRequirements { get; set; }

        public List<LinkItem> RelatedApprenticeshipLinks { get; set; }

        // OTHER
        public string Work { get; set; }

        public string Volunteering { get; set; }

        public string DirectApplication { get; set; }

        public string OtherRoutes { get; set; }

        // related
        public List<InfoItem> RelatedRestrictions { get; set; }

        public string OtherRequirements { get; set; }

        // related
        public List<InfoItem> RelatedRegistrations { get; set; }

        public string ProfessionalAndIndustryBodies { get; set; }

        public string CareerTips { get; set; }

        public string FurtherInformation { get; set; }

        #endregion JobProfile Data

        #region supporting fields

        public string SectionTitle { get; set; }

        public string SubsectionUniversity { get; set; }

        public string SubsectionUniversityRequirements { get; set; }

        public string SubsectionUniversityMoreInformation { get; set; }

        public string SubsectionCollege { get; set; }

        public string SubsectionCollegeRequirements { get; set; }

        public string SubsectionCollegeMoreInformation { get; set; }

        public string SubsectionApprenticeship { get; set; }

        public string SubsectionApprenticeshipRequirements { get; set; }

        public string SubsectionApprenticeshipMoreInformation { get; set; }

        public string SubsectionWork { get; set; }

        public string SubsectionVolunteering { get; set; }

        public string SubsectionDirectApplication { get; set; }

        public string SubsectionOtherRoutes { get; set; }

        public string SubsectionRestrictions { get; set; }

        public string SubsectionRestrictionsOpeningText { get; set; }

        public string SubsectionOtherRequirements { get; set; }

        public string SubsectionMoreInfo { get; set; }

        public string SubsectionMoreInfoRegistration { get; set; }

        public string SubsectionMoreInfoRegistrationOpeningText { get; set; }

        public string SubsectionMoreInfoTips { get; set; }

        public string SubsectionMoreInfoBodies { get; set; }

        public string SubsectionMoreInfoFurtherInfo { get; set; }

        #endregion supporting fields
    }
}