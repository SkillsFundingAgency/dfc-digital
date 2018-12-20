using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Core;
using DFC.Digital.Web.Sitefinity.Core;
using DFC.Digital.Web.Sitefinity.Widgets.Mvc.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using Telerik.Sitefinity.Mvc;

namespace DFC.Digital.Web.Sitefinity.Widgets.Mvc.Controllers
{
    /// <summary>
    /// Custom Widget for having Breadcrumb
    /// </summary>
    /// <seealso cref="DFC.Digital.Web.Core.BaseDfcController" />
    [ControllerToolboxItem(Name = "DfcBreadcrumb", Title = "DFC Breadcrumb", SectionName = SitefinityConstants.CustomWidgetSection)]
    public class DfcBreadcrumbController : BaseDfcController
    {
        #region Private Fields

        private IJobProfileCategoryRepository categoryRepo;
        private IJobProfileRepository jobProfileRepository;
        private ISitefinityCurrentContext sitefinityCurrentContext;

        #endregion Private Fields

        #region Constructors

        public DfcBreadcrumbController(IJobProfileCategoryRepository categoryRepo, IJobProfileRepository jobProfileRepo, ISitefinityCurrentContext sitefinityCurrentContext, IApplicationLogger applicationLogger) : base(applicationLogger)
        {
            this.categoryRepo = categoryRepo;
            this.jobProfileRepository = jobProfileRepo;
            this.sitefinityCurrentContext = sitefinityCurrentContext;
        }

        #endregion Constructors

        #region Public Properties

        /// <summary>
        /// Gets or sets the Link of the home page.
        /// </summary>
        /// <value>
        /// The Link of the home page.
        /// </value>
        [DisplayName("Home page Link")]
        public string HomepageLink { get; set; } = "/";

        /// <summary>
        /// Gets or sets the Text of the home page.
        /// </summary>
        /// <value>
        /// The Text of the home page.
        /// </value>
        [DisplayName("Home page text")]
        public string HomepageText { get; set; } = "Explore careers home";

        [DisplayName("Alerts page breadcrumb text")]
        public string AlertsPageText { get; set; } = "Error";

        [DisplayName("Job categories URL segment (Upper case)")]
        public string JobCategoriesPathSegment { get; set; } = "JOB-CATEGORIES";

        [DisplayName("Job profiles URL segment (Upper case)")]
        public string JobProfilesPathSegment { get; set; } = "JOB-PROFILES";

        [DisplayName("Alerts URL segment (Upper case)")]
        public string AlertsPathSegment { get; set; } = "ALERTS";

        #endregion Public Properties

        #region Actions

        // GET: DfcBreadcrumb

        /// <summary>
        /// entry point to the widget to show a breadcrumb.
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpGet]
        [RelativeRoute("")]
        public ActionResult Index()
        {
            return GetBreadcrumbView(string.Empty);
        }

        /// <summary>
        /// Breadcrumb on Index of the specified urlname.
        /// </summary>
        /// <param name="urlName">The urlname.</param>
        /// <returns>ActionResult</returns>
        [HttpGet]
        [RelativeRoute("{urlname}")]
        public ActionResult Index(string urlName)
        {
            return GetBreadcrumbView(urlName);
        }

        /// <summary>
        /// Gets the job profile details view.
        /// </summary>
        /// <param name="urlName">The urlname.</param>
        /// <returns>ActionResult</returns>
        private ActionResult GetBreadcrumbView(string urlName)
        {
            var breadCrumbLink = new BreadcrumbLink();
            var model = new DfcBreadcrumbViewModel
            {
                HomepageText = HomepageText,
                HomepageLink = HomepageLink,
                BreadcrumbLinks = new List<BreadcrumbLink>
                {
                    breadCrumbLink
                }
            };

            // We get the page node we are on
            var currentPageNode = sitefinityCurrentContext.GetCurrentDfcPageNode();
            if (currentPageNode != null)
            {
                var nodeUrl = currentPageNode.Url.OriginalString;
                switch (nodeUrl.ToUpperInvariant())
                {
                    case var jobCategoriesPage when !string.IsNullOrEmpty(urlName) && jobCategoriesPage.Contains(JobCategoriesPathSegment):
                        {
                            var category = categoryRepo.GetByUrlName(urlName);
                            breadCrumbLink.Text = (category == null) ? currentPageNode.Title : category.Title;
                            break;
                        }

                    case var jobProfilePage when !string.IsNullOrEmpty(urlName) && jobProfilePage.Contains(JobProfilesPathSegment):
                        {
                            var jobProfile = jobProfileRepository.GetByUrlName(urlName);
                            breadCrumbLink.Text = (jobProfile == null) ? currentPageNode.Title : jobProfile.Title;
                            break;
                        }

                    case var alertsPage when alertsPage.Contains(AlertsPathSegment):
                        {
                            breadCrumbLink.Text = AlertsPageText;
                            break;
                        }

                    default:
                        {
                            model.BreadcrumbLinks = sitefinityCurrentContext.BreadcrumbToParent();

                            //the current page should not be linked
                            model.BreadcrumbLinks.FirstOrDefault().Link = null;
                            model.BreadcrumbLinks = model.BreadcrumbLinks.Reverse().ToList();
                            break;
                        }
                }
            }

            return View(model);
        }

        #endregion Actions
    }
}