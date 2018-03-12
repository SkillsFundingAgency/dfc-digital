using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.Core.Interface;
using DFC.Digital.Web.Sitefinity.Core.Utility;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models;
using System.Web.Mvc;
using Telerik.Sitefinity.Mvc;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Controllers
{
    /// <summary>
    /// Job Profile Section Controller
    /// </summary>
    /// <seealso cref="DFC.Digital.Web.Core.Base.BaseDfcController" />
    [ControllerToolboxItem(Name = "JobProfileSection", Title = "Job Profile Section", SectionName = SitefinityConstants.CustomWidgetSection)]
    public class JobProfileSectionController : BaseJobProfileWidgetController
    {
        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="JobProfileSectionController" /> class.
        /// </summary>
        /// <param name="jobProfileRepository">The job profile repository.</param>
        /// <param name="webAppContext">The web application context.</param>
        /// <param name="applicationLogger">application logger</param>
        /// <param name="sitefinityPage">sitefinity</param>
        public JobProfileSectionController(IJobProfileRepository jobProfileRepository, IWebAppContext webAppContext, IApplicationLogger applicationLogger, ISitefinityPage sitefinityPage)
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
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the name of the property.
        /// </summary>
        /// <value>
        /// The name of the property.
        /// </value>
        public string PropertyName { get; set; }

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
            var propertyValue = GetPropertyValue(CurrentJobProfile);

            return View("Index", new JobProfileSectionViewModel { TopSectionContent = TopSectionContent, BottomSectionContent = BottomSectionContent, PropertyValue = propertyValue, Title = Title, PropertyName = PropertyName });
        }

        /// <summary>
        /// Gets the property value.
        /// </summary>
        /// <param name="jobProfile">The urlname.</param>
        /// <returns>string</returns>
        private string GetPropertyValue(JobProfile jobProfile)
        {
            var propertyValue = string.Empty;
            if (!string.IsNullOrWhiteSpace(PropertyName))
            {
                propertyValue = jobProfile.GetType()
                    .GetProperty(PropertyName)
                    ?.GetValue(jobProfile, null) as string;
            }

            return propertyValue;
        }

        #endregion Actions
    }
}