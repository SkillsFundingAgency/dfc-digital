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
        public HowToBecome HowToBecome { get; set; }

        public bool IsHtbcaDReady { get; set; }

        public string HowToBecomeText { get; set; }

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