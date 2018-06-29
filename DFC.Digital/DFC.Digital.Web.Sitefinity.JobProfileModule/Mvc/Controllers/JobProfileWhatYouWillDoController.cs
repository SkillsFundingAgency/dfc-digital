using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.Core;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models;
using System.Web.Mvc;
using Telerik.Sitefinity.Mvc;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Controllers
{
    /// <summary>
    /// Job Profile Section Controller
    /// </summary>
    /// <seealso cref="DFC.Digital.Web.Core.BaseDfcController" />
    [ControllerToolboxItem(Name = "JobProfileWhatYouWillDo", Title = "JobProfile What You Will Do", SectionName = SitefinityConstants.CustomWidgetSection)]
    public class JobProfileWhatYouWillDoController : BaseJobProfileWidgetController
    {
        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="JobProfileWhatYouWillDoController" /> class.
        /// </summary>
        /// <param name="jobProfileRepository">The job profile repository.</param>
        /// <param name="webAppContext">The web application context.</param>
        /// <param name="applicationLogger">application logger</param>
        /// <param name="sitefinityPage">sitefinity</param>
        public JobProfileWhatYouWillDoController(IJobProfileRepository jobProfileRepository, IWebAppContext webAppContext, IApplicationLogger applicationLogger, ISitefinityPage sitefinityPage)
             : base(webAppContext, jobProfileRepository, applicationLogger, sitefinityPage)
        {
        }

        #endregion Ctor

        #region public Properties

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
        public string MainSectionTitle { get; set; } = "What you''ll do";

        /// <summary>
        /// Gets or sets the name of the property.
        /// </summary>
        /// <value>
        /// The name of the property.
        /// </value>
        public string SectionId { get; set; } = "Task";

        /// <summary>
        /// Gets or sets the what it takes section title.
        /// </summary>
        /// <value>
        /// The what it takes section title.
        /// </value>
        public string WhatYouWillDoSectionTitle { get; set; } = "Day-to-day tasks";

        #endregion private Properties

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

            var model = new JobProfileWhatYouWillDoViewModel
            {
                TopSectionContent = TopSectionContent,
                BottomSectionContent = BottomSectionContent,
                PropertyValue = CurrentJobProfile?.WhatYouWillDo,
                Title = MainSectionTitle,
                SectionId = SectionId,
                WhatyouWillDoSectionTitle = WhatYouWillDoSectionTitle,
                IsWhatYouWillDoCadView = CurrentJobProfile?.WhatYouWillDoData != null && (CurrentJobProfile != null && CurrentJobProfile.WhatYouWillDoData.IsWYDCaDReady),
                Location = CurrentJobProfile?.WhatYouWillDoData?.Location,
                Uniform = CurrentJobProfile?.WhatYouWillDoData?.Uniform,
                Environment = CurrentJobProfile?.WhatYouWillDoData?.Environment,
                EnvironmentTitle = CurrentJobProfile?.WhatYouWillDoData?.EnvironmentTitle
            };

            return View(model);
        }

        #endregion Actions
    }
}