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
        public string MainSectionTitle { get; set; } = "How to become";

        [DisplayName("Section Id- for anchor link href target")]
        public string SectionId { get; set; } = "how-to-become";

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

        [DisplayName("Subsection Restrictions - Opening Text")]
        public string SubsectionRestrictionsOpeningText { get; set; }

        [DisplayName("Subsection Other Requirements")]
        public string SubsectionOtherRequirements { get; set; } = "Other Requirements";

        [DisplayName("Subsection More information (MI)")]
        public string SubsectionMoreInfo { get; set; } = "More information";

        [DisplayName("Subsection (MI) Registration")]
        public string SubsectionMoreInfoRegistration { get; set; } = "Registration";

        [DisplayName("Subsection (MI) Registration - Opening Text")]
        public string SubsectionMoreInfoRegistrationOpeningText { get; set; }

        [DisplayName("Subsection (MI) Career Tips")]
        public string SubsectionMoreInfoTips { get; set; } = "Career tips";

        [DisplayName("Subsection (MI) Professional and Industry Bodies")]
        public string SubsectionMoreInfoBodies { get; set; } = "Professional and industry bodies";

        [DisplayName("Subsection (MI) More info")]
        public string SubsectionMoreInfoFurtherInfo { get; set; } = "Further information";

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
            return GetJobProfileDetailsView();
        }

        private ActionResult GetJobProfileDetailsView()
        {
           var model = LoadViewModel();

            return View("Index", model);
        }

        private JobProfileHowToBecomeViewModel LoadViewModel()
        {
            var model = mapper.Map<JobProfileHowToBecomeViewModel>(this);

            if (CurrentJobProfile != null)
            {
                model.HowToBecomeText = CurrentJobProfile.HowToBecome;
                model.HowToBecome = CurrentJobProfile.HowToBecomeData;
            }

            return model;
        }

        #endregion Actions
    }
}