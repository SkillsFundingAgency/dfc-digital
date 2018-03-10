using DFC.Digital.Data.Interfaces;
using DFC.Digital.Web.Sitefinity.Core.Utility;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models;
using System.ComponentModel;
using System.Web.Mvc;
using Telerik.Sitefinity.Mvc;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Controllers
{
    [ControllerToolboxItem(Name = "JobProfilesByCategoryController", Title = "JobProfiles by Category", SectionName = SitefinityConstants.CustomWidgetSection)]
    public class JobProfilesByCategoryController : Web.Core.Base.BaseDfcController
    {
        #region Private Fields

        private IJobProfileCategoryRepository categoryRepo;
        private IWebAppContext webAppContext;

        #endregion Private Fields

        #region Constructors

        public JobProfilesByCategoryController(IJobProfileCategoryRepository categoryRepo, IWebAppContext webAppContext, IApplicationLogger applicationLogger) : base(applicationLogger)
        {
            this.categoryRepo = categoryRepo;
            this.webAppContext = webAppContext;
        }

        #endregion Constructors
        #region Public Properties

        /// <summary>
        /// Gets or sets the job profile details page.
        /// </summary>
        /// <value>
        /// The job profile details page.
        /// </value>
        [DisplayName("Job Profile Details Page")]
        public string JobProfileDetailsPage { get; set; } = "/job-profiles/";

        [DisplayName("Default Job Profile Category Url Name - Mainly for preview purposes")]
        public string DefaultJobProfileCategoryUrlName { get; set; } = "Health";

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
            if (webAppContext.IsContentAuthoringSite)
            {
                return GetJobProfilesByCategoryView(DefaultJobProfileCategoryUrlName);
            }
            else
            {
                return Redirect("\\");
            }
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
            return GetJobProfilesByCategoryView(urlName);
        }

        private ActionResult GetJobProfilesByCategoryView(string urlName)
        {
            var category = categoryRepo.GetByUrlName(urlName);

            //if the category does not exist.
            if (category == null)
            {
                return HttpNotFound();
            }

            var jobProfiles = categoryRepo.GetRelatedJobProfiles(category.Title);
            var model = new JobProfileByCategoryViewModel
            {
                Title = category.Title,
                Description = category.Description,
                JobProfiles = jobProfiles,
                JobProfileUrl = JobProfileDetailsPage,
            };

            return View("Index", model);
        }

        #endregion Actions
    }
}