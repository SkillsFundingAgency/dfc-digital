using DFC.Digital.Data.Interfaces;
using DFC.Digital.Web.Sitefinity.Core.Utility;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;
using Telerik.Sitefinity.ContentLocations;
using Telerik.Sitefinity.Mvc;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Controllers
{
    [ControllerToolboxItem(Name = "JobProfileSettingsAndPreviewController", Title = "JobProfile Settings and Preview", SectionName = SitefinityConstants.CustomWidgetSection)]
    public class JobProfileSettingsAndPreviewController : Web.Core.Base.BaseDfcController, IContentLocatableView
    {
        #region Private Fields

        private IWebAppContext webAppContext;
        private IJobProfileRepository jobProfileRepository;

        #endregion Private Fields

        #region Constructors

        public JobProfileSettingsAndPreviewController(IJobProfileRepository repository, IWebAppContext webAppContext, IApplicationLogger applicationLogger) : base(applicationLogger)
        {
            jobProfileRepository = repository;
            this.webAppContext = webAppContext;
        }

        #endregion Constructors

        #region Public Properties

        [DisplayName("Default Job Profile Url name for all widgets on this page, mainly for preview purposes")]
        public string DefaultJobProfileUrlName { get; set; } = "plumber";

        public bool? DisableCanonicalUrlMetaTag { get; set; }

        public string PreviousURLname { get; set; }

        #endregion Public Properties

        #region Actions

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpGet]
        [RelativeRoute("")]
        public ActionResult Index()
        {
            if (webAppContext.IsContentAuthoringAndNotPreviewMode)
            {
                var model = new JobProfileSettingsAndPreviewModel() { DefaultJobProfileUrl = DefaultJobProfileUrlName };
                return View("Index", model);
            }

            return new EmptyResult();
        }

        [NonAction]
        public IEnumerable<IContentLocationInfo> GetLocations()
        {
            yield return new ContentLocationInfo
            {
                ContentType = jobProfileRepository.GetContentType(),
                ProviderName = jobProfileRepository.GetProviderName()
            };
        }
    }

    #endregion Actions
}