using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.Core.Utility;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using Telerik.Sitefinity.Mvc;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Controllers
{
    /// <summary>
    /// Custom Widget for displaying job profile categories
    /// </summary>
    /// <seealso cref="Web.Core.Base.BaseDfcController" />
    [ControllerToolboxItem(Name = "JobProfileCategories", Title = "JobProfile Categories", SectionName = SitefinityConstants.CustomWidgetSection)]
    public class JobProfileCategoriesController : Web.Core.Base.BaseDfcController
    {
        #region Private Fields

        private readonly IJobProfileCategoryRepository jobProfileCategoryRepository;
        private readonly IWebAppContext webAppContext;

        #endregion Private Fields
        #region Constructors

        public JobProfileCategoriesController(IJobProfileCategoryRepository repository, IWebAppContext webAppContext, IApplicationLogger applicationLogger) : base(applicationLogger)
        {
            jobProfileCategoryRepository = repository;
            this.webAppContext = webAppContext;
        }

        #endregion Constructors

        #region Public Properties

        [DisplayName("Jobprofile Category Page")]
        public string JobProfileCategoryPage { get; set; } = "/jobprofile-category";

        [DisplayName("Other Job Categories Title")]
        public string OtherJobCategoriesTitle { get; set; } = "Other job categories";

        [DisplayName("Widget displayed in the side bar")]
        public bool SidePageDisplay { get; set; }

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
            if (SidePageDisplay)
            {
                return Index(string.Empty);
            }

            var jobProfileCategories = GetJobProfileCategories().ToList();

            UpdateJobProfileCategoryUrl(jobProfileCategories);

            return View("Index", new JobProfileCategoriesViewModel { JobProfileCategories = jobProfileCategories, IsContentAuthoring = webAppContext.IsContentAuthoringSite });
        }

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>Action Result</returns>
        [HttpGet]
        [RelativeRoute("{urlName}")]
        public ActionResult Index(string urlName)
        {
            var jobProfileCategories = GetJobProfileCategories()?.Where(jpCat => jpCat.Url.ToLower() != urlName.ToLower())
                .ToList();

            UpdateJobProfileCategoryUrl(jobProfileCategories);

            return View("RelatedJobCategories", new RelatedJobProfileCategoriesViewModel { JobProfileCategories = jobProfileCategories, IsContentAuthoring = webAppContext.IsContentAuthoringSite, OtherCategoriesTitle = OtherJobCategoriesTitle });
        }

        /// <summary>
        /// Called when a request matches this controller, but no method with the specified action name is found in the controller.
        /// </summary>
        /// <param name="actionName">The name of the attempted action.</param>
        protected override void HandleUnknownAction(string actionName)
        {
            Index().ExecuteResult(ControllerContext);
        }

        private IQueryable<JobProfileCategory> GetJobProfileCategories()
        {
            return jobProfileCategoryRepository.GetJobProfileCategories()
                ?.OrderBy(x => x.Title);
        }

        private void UpdateJobProfileCategoryUrl(List<JobProfileCategory> jobProfileCategories)
        {
            if (jobProfileCategories != null)
            {
                foreach (var jobProfileCategory in jobProfileCategories)
                {
                    jobProfileCategory.Url = $"{JobProfileCategoryPage}/{jobProfileCategory.Url}";
                }
            }
        }

        #endregion Actions
    }
}