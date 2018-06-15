using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.Core;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models;
using System;
using System.ComponentModel;
using System.Web.Mvc;
using Telerik.Sitefinity.Mvc;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Controllers
{
    /// <summary>
    /// Job Profile Section Controller
    /// </summary>
    /// <seealso cref="DFC.Digital.Web.Core.BaseDfcController" />
    [ControllerToolboxItem(Name = "JobProfileWhatItTakes", Title = "Job Profile What It Takes", SectionName = SitefinityConstants.CustomWidgetSection)]
    public class JobProfileWhatItTakesController : BaseJobProfileWidgetController
    {
        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="JobProfileWhatItTakesController" /> class.
        /// </summary>
        /// <param name="jobProfileRepository">The job profile repository.</param>
        /// <param name="webAppContext">The web application context.</param>
        /// <param name="applicationLogger">application logger</param>
        /// <param name="sitefinityPage">sitefinity</param>
        public JobProfileWhatItTakesController(IJobProfileRepository jobProfileRepository, IWebAppContext webAppContext, IApplicationLogger applicationLogger, ISitefinityPage sitefinityPage)
             : base(webAppContext, jobProfileRepository, applicationLogger, sitefinityPage)
        {
        }

        #endregion Ctor

        #region Public Properties

        /// <summary>
        /// Gets or sets the content of the top section.
        /// </summary>
        /// <value>
        /// The content of the top section.
        /// </value>
        public string TopSectionContent { get; set; }

        /// <summary>
        /// Gets or sets the content of the bottom section.
        /// </summary>
        /// <value>
        /// The content of the bottom section.
        /// </value>
        public string BottomSectionContent { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string MainSectionTitle { get; set; } = "What It Takes";

        /// <summary>
        /// Gets or sets the name of the property.
        /// </summary>
        /// <value>
        /// The name of the property.
        /// </value>
        public string SectionId { get; set; } = "Skills";

        /// <summary>
        /// Gets or sets the what it takes section title.
        /// </summary>
        /// <value>
        /// The what it takes section title.
        /// </value>
        public string WhatItTakesSectionTitle { get; set; } = "Skills and Knowledge";

        /// <summary>
        /// Gets or sets the restrictions title.
        /// </summary>
        /// <value>
        /// The restrictions title.
        /// </value>
        public string RestrictionsTitle { get; set; } = "Restrictions and requirements";

        /// <summary>
        /// Gets or sets the restrictions intro.
        /// </summary>
        /// <value>
        /// The restrictions intro.
        /// </value>
        public string RestrictionsIntro { get; set; } = "You'll need to:";

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
            return ReturnSectionView();
        }

        protected override ActionResult GetEditorView()
        {
            return ReturnSectionView();
        }

        /// <summary>
        /// Returns the section view.
        /// </summary>
        /// <returns>Action Result</returns>
        private ActionResult ReturnSectionView()
        {
            var model = new JobProfileWhatItTakesViewModel
            {
                TopSectionContent = TopSectionContent,
                BottomSectionContent = BottomSectionContent,
                PropertyValue = CurrentJobProfile.Skills,
                Title = MainSectionTitle,
                SectionId = SectionId,
                WhatItTakesSectionTitle = WhatItTakesSectionTitle,
                IsWhatItTakesCadView = CurrentJobProfile.HowToBecomeData.IsHTBCaDReady
            };

            if (model.IsWhatItTakesCadView)
            {
                model.RestrictionsOtherRequirements = new RestrictionsOtherRequirements
                {
                    Restrictions = CurrentJobProfile.Restrictions,
                    OtherRequirements = CurrentJobProfile.OtherRequirements,
                    SectionIntro = RestrictionsIntro,
                    SectionTitle = RestrictionsTitle
                };
            }

            return View(model);
        }

        #endregion Actions
    }
}