using AutoMapper;
using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.Core;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Telerik.Sitefinity.Mvc;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Controllers
{
    /// <summary>
    /// Custom Widget for the Search box and Search results for Job Profiles
    /// </summary>
    /// <seealso cref="DFC.Digital.Web.Core.BaseDfcController" />
    [ControllerToolboxItem(Name = "JobProfileHowToBecome", Title = "JobProfile HowToBecome", SectionName = SitefinityConstants.CustomWidgetSection)]
    public class JobProfileHowToBecomeController : BaseJobProfileWidgetController
    {
        #region Private Fields

        private readonly IMapper mapper;

        #endregion Private Fields

        #region Constructors

        public JobProfileHowToBecomeController(
            IWebAppContext webAppContext,
            IJobProfileRepository jobProfileRepository,
            IApplicationLogger applicationLogger,
            ISitefinityPage sitefinityPage,
            IMapper mapper)
            : base(webAppContext, jobProfileRepository, applicationLogger, sitefinityPage)
        {
            this.mapper = mapper;
        }

        #endregion Constructors

        #region Public Properties

        [DisplayName("Section Title")]
        public string SectionTitle { get; set; } = "How to become";

        [DisplayName("Subsection University")]
        public string SubsectionUniversity { get; set; } = "University";

        [DisplayName("Subsection University Requirements")]
        public string SubsectionUniversityRequirements { get; set; } = "Requirements";

        [DisplayName("Subsection University More Information")]
        public string SubsectionUniversityMoreInformation { get; set; } = "More information";

        [DisplayName("Subsection College")]
        public string SubsectionCollege { get; set; } = "College";

        [DisplayName("Subsection College Requirements")]
        public string SubsectionCollegeRequirements { get; set; } = "Requirements";

        [DisplayName("Subsection College More Information")]
        public string SubsectionCollegeMoreInformation { get; set; } = "More information";

        [DisplayName("Subsection Apprenticeship")]
        public string SubsectionApprenticeship { get; set; } = "Apprenticeship";

        [DisplayName("Subsection Apprenticeship Requirements")]
        public string SubsectionApprenticeshipRequirements { get; set; } = "Requirements";

        [DisplayName("Subsection Apprenticeship More Information")]
        public string SubsectionApprenticeshipMoreInformation { get; set; } = "More information";

        [DisplayName("Subsection Work")]
        public string SubsectionWork { get; set; } = "Work";

        [DisplayName("Subsection Volunteering")]
        public string SubsectionVolunteering { get; set; } = "Volunteering";

        [DisplayName("Subsection Direct Application")]
        public string SubsectionDirectApplication { get; set; } = "Direct Application";

        [DisplayName("Subsection Other Routes")]
        public string SubsectionOtherRoutes { get; set; } = "Other Routes";

        [DisplayName("Subsection Restrictions")]
        public string SubsectionRestrictions { get; set; } = "Restrictions";

        [DisplayName("Subsection Other Requirements")]
        public string SubsectionOtherRequirements { get; set; } = "Other Requirements";

        [DisplayName("Subsection Registration")]
        public string SubsectionRegistration { get; set; } = "Registration";

        [DisplayName("Subsection Industry Bodies, tips and more info (IBTMI)")]
        public string SubsectionIndBodiesTipsMoreInfo { get; set; } = "Industry Bodies, tips and more info";

        [DisplayName("Subsection (IBTMI) Professional and Industry Bodies")]
        public string SubsectionIndBodiesTipsMoreInfoBodies { get; set; } = "Professional and Industry Bodies";

        [DisplayName("Subsection (IBTMI) Tips")]
        public string SubsectionIndBodiesTipsMoreInfoTips { get; set; } = "Tips";

        [DisplayName("Subsection (IBTMI) More info")]
        public string SubsectionIndBodiesTipsMoreInfoMoreInfo { get; set; } = "More info";

        #endregion Public Properties

        #region Actions

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>Action Result</returns>
        [HttpGet]
        [RelativeRoute("")]
        public ActionResult Index()
        {
            return BaseIndex();
        }

        /// <summary>
        /// Indexes the specified urlname.
        /// </summary>
        /// <param name="urlName">The urlname.</param>
        /// <returns>Action Result</returns>
        [HttpGet]
        [RelativeRoute("{urlName}")]
        public ActionResult Index(string urlName)
        {
            return BaseIndex(urlName);
        }

        protected override ActionResult GetDefaultView()
        {
            return GetJobProfileDetailsView();
        }

        protected override ActionResult GetEditorView()
        {
            // Sitefinity cannot handle async very well. So initialising it on current UI thread.
            var model = mapper.Map<JobProfileHowToBecomeViewModel>(CurrentJobProfile);
            return GetJobProfileDetailsView(model);
        }

        /// <summary>
        /// Gets the job profile details view.
        /// </summary>
        /// <param name="model">Job profile view model</param>
        /// <returns>Index View</returns>
        private ActionResult GetJobProfileDetailsView(JobProfileHowToBecomeViewModel model = null)
        {
            if (model == null)
            {
                model = mapper.Map<JobProfileHowToBecomeViewModel>(CurrentJobProfile);
            }

            model.SectionTitle = SectionTitle;
            model.SubsectionUniversity = SubsectionUniversity;
            model.SubsectionUniversityRequirements = SubsectionUniversityRequirements;
            model.SubsectionUniversityMoreInformation = SubsectionUniversityMoreInformation;
            model.SubsectionCollege = SubsectionCollege;
            model.SubsectionCollegeRequirements = SubsectionCollegeRequirements;
            model.SubsectionCollegeMoreInformation = SubsectionCollegeMoreInformation;
            model.SubsectionApprenticeship = SubsectionApprenticeship;
            model.SubsectionApprenticeshipRequirements = SubsectionApprenticeshipRequirements;
            model.SubsectionApprenticeshipMoreInformation = SubsectionApprenticeshipMoreInformation;
            model.SubsectionWork = SubsectionWork;
            model.SubsectionVolunteering = SubsectionVolunteering;
            model.SubsectionDirectApplication = SubsectionDirectApplication;
            model.SubsectionOtherRoutes = SubsectionOtherRoutes;
            model.SubsectionRestrictions = SubsectionRestrictions;
            model.SubsectionOtherRequirements = SubsectionOtherRequirements;
            model.SubsectionRegistration = SubsectionRegistration;
            model.SubsectionIndBodiesTipsMoreInfo = SubsectionIndBodiesTipsMoreInfo;
            model.SubsectionIndBodiesTipsMoreInfoBodies = SubsectionIndBodiesTipsMoreInfoBodies;
            model.SubsectionIndBodiesTipsMoreInfoTips = SubsectionIndBodiesTipsMoreInfoTips;
            model.SubsectionIndBodiesTipsMoreInfoMoreInfo = SubsectionIndBodiesTipsMoreInfoMoreInfo;

            return View("Index", model);
        }

        #endregion Actions
    }
}