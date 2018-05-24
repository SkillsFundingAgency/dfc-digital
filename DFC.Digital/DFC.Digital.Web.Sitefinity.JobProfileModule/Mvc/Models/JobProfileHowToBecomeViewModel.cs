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

        public string SpecificUniversityLinks { get; set; }

        public List<LinkItem> SpecificUniversityLinkList { get; set; }

        // related
        public List<LinkItem> UniversityLinks { get; set; }

        // related to be deleted
        //public string Universities { get; set; }
        public string UniversityInformation { get; set; }

        public string UniversityEntryRequirements { get; set; }

        // COLLEGE
        public string CollegeRelevantSubjects { get; set; }

        public string CollegeEntryRequirements { get; set; }

        // related
        public List<InfoItem> Colleges { get; set; }

        public string CollegeFindACourse { get; set; }

        public string CollegeFurtherRouteInfo { get; set; }

        // APPRENTICESHIP
        public string ApprenticeshipFurtherRouteInfo { get; set; }

        public string ApprenticeshipRelevantSubjects { get; set; }

        // related
        public List<InfoItem> Apprenticeships { get; set; }

        public string ApprenticeshipEntryRequirements { get; set; }

        // OTHER
        public string Work { get; set; }

        public string Volunteering { get; set; }

        public string DirectApplication { get; set; }

        public string OtherRoutes { get; set; }

        public string AgeLimitation { get; set; }

        // related
        public List<InfoItem> Restrictions { get; set; }

        public string DBSCheck { get; set; }

        public string DBScheckReason { get; set; }

        public string MedicalTest { get; set; }

        public string MedicalTestReason { get; set; }

        public string FullDrivingLicence { get; set; }

        public string OtherRestrictions { get; set; }

        public string OtherRequirements { get; set; }

        // related
        public List<InfoItem> CommonRegistrations { get; set; }

        public string OtherRegistration { get; set; }

        public string ProfessionalAndIndustryBodies { get; set; }

        public string CareerTips { get; set; }

        public string MoreInfo { get; set; }

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

        public string SubsectionOtherRequirements { get; set; }

        public string SubsectionRegistration { get; set; }

        public string SubsectionIndBodiesTipsMoreInfo { get; set; }

        public string SubsectionIndBodiesTipsMoreInfoBodies { get; set; }

        public string SubsectionIndBodiesTipsMoreInfoTips { get; set; }

        public string SubsectionIndBodiesTipsMoreInfoMoreInfo { get; set; }

        #endregion supporting fields
    }
}