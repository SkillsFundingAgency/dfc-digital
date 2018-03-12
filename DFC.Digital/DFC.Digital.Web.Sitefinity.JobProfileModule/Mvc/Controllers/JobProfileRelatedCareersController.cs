using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.Core.Interface;
using DFC.Digital.Web.Sitefinity.Core.Utility;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;
using Telerik.Sitefinity.Mvc;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Controllers
{
    [ControllerToolboxItem(Name = "JobProfileRelatedCareersController", Title = "JobProfile Related Careers", SectionName = SitefinityConstants.CustomWidgetSection)]
    public class JobProfileRelatedCareersController : Web.Core.Base.BaseDfcController
    {
        #region Private Fields

        private IJobProfileRelatedCareersRepository jobProfileRelatedCareersRepository;
        private IWebAppContext webAppContext;
        private ISitefinityPage sitefinityPage;

        #endregion Private Fields

        #region Constructors

        public JobProfileRelatedCareersController(IJobProfileRelatedCareersRepository repository, IWebAppContext webAppContext, IApplicationLogger applicationLogger, ISitefinityPage sitefinityPage) : base(applicationLogger)
        {
            this.jobProfileRelatedCareersRepository = repository;
            this.webAppContext = webAppContext;
            this.sitefinityPage = sitefinityPage;
        }

        #endregion Constructors

        #region Public Properties

        [DisplayName("Section Title")]
        public string SectionTitle { get; set; } = "Related careers";

        [DisplayName("Default Job Profile Url Name - Mainly for preview purposes")]
        public string DefaultJobProfileUrlName { get; set; } = "plumber";

        [DisplayName("Maximum number of links to display")]
        public int MaximumLinksToDisplay { get; set; } = 5;

        #endregion Public Properties

        #region Actions

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns><see cref="ActionResult"/> or Redirect result</returns>
        [HttpGet]
        [RelativeRoute("")]
        public ActionResult Index()
        {
            if (webAppContext.IsContentAuthoringSite)
            {
                return GetRelatedJobProfilesView(sitefinityPage.GetDefaultJobProfileToUse(DefaultJobProfileUrlName));
            }

            return Redirect("\\");
        }

        /// <summary>
        /// Indexes the specified urlname.
        /// </summary>
        /// <param name="urlName">The urlname.</param>
        /// <returns><see cref="ActionResult"/></returns>
        [HttpGet]
        [RelativeRoute("{urlName}")]
        public ActionResult Index(string urlName)
        {
            return GetRelatedJobProfilesView(urlName);
        }

        private ActionResult GetRelatedJobProfilesView(string urlname)
        {
            IEnumerable<JobProfileRelatedCareer> relatedJobProfiles;

            if (webAppContext.IsContentPreviewMode)
            {
                relatedJobProfiles = jobProfileRelatedCareersRepository.GetByParentNameForPreview(urlname, MaximumLinksToDisplay);
            }
            else
            {
                relatedJobProfiles = jobProfileRelatedCareersRepository.GetByParentName(urlname, MaximumLinksToDisplay);
            }

            if (relatedJobProfiles != null)
            {
                var model = new JobProfileRelatedCareersModel() { Title = SectionTitle, RelatedCareers = relatedJobProfiles };
                return View("Index", model);
            }

            return new EmptyResult();
        }

        #endregion Actions
    }
}