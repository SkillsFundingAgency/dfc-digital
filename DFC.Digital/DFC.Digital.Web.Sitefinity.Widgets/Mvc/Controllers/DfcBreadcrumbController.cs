using DFC.Digital.Data.Interfaces;
using DFC.Digital.Web.Core.Base;
using DFC.Digital.Web.Sitefinity.Core;
using DFC.Digital.Web.Sitefinity.Core.Utility;
using DFC.Digital.Web.Sitefinity.Widgets.Mvc.Models;
using System.ComponentModel;
using System.Web.Mvc;
using Telerik.Sitefinity.Mvc;

namespace DFC.Digital.Web.Sitefinity.Widgets.Mvc.Controllers
{
    /// <summary>
    /// Custom Widget for having Breadcrumb
    /// </summary>
    /// <seealso cref="DFC.Digital.Web.Core.Base.BaseDfcController" />
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
        public string HomePageLink { get; set; } = "/";

        /// <summary>
        /// Gets or sets the Text of the home page.
        /// </summary>
        /// <value>
        /// The Text of the home page.
        /// </value>
        [DisplayName("Home page Text")]
        public string HomePageText { get; set; } = "Find a career home";

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
        /// <param name="urlname">The urlname.</param>
        /// <returns>ActionResult</returns>
        [HttpGet]
        [RelativeRoute("{urlname}")]
        public ActionResult Index(string urlname)
        {
            return GetBreadcrumbView(urlname);
        }

        /// <summary>
        /// Gets the job profile details view.
        /// </summary>
        /// <param name="urlName">The urlname.</param>
        /// <returns>ActionResult</returns>
        private ActionResult GetBreadcrumbView(string urlName)
        {
            var model = new DfcBreadcrumbViewModel
            {
                HomePageText = HomePageText,
                HomePageLink = HomePageLink
            };

            // We get the page node we are on
            var currentPageNode = sitefinityCurrentContext.GetCurrentDfcPageNode();
            if (currentPageNode != null)
            {
                string nodeUrl = currentPageNode.Url;

                // If we are on JobCategories page(s)
                if (nodeUrl.ToLowerInvariant().Contains("job-categories") && !string.IsNullOrEmpty(urlName))
                {
                    var category = categoryRepo.GetByUrlName(urlName);

                    if (category != null)
                    {
                        model.BreadcrumbedPageTitleText = category.Title;
                    }
                    else
                    {
                        model.BreadcrumbedPageTitleText = currentPageNode.Title;
                    }
                } // If we are on JobProfileDetalails page(s)
                else if (nodeUrl.ToLowerInvariant().Contains("job-profiles") && !string.IsNullOrEmpty(urlName))
                {
                    var jobProfile = jobProfileRepository.GetByUrlName(urlName);

                    if (jobProfile != null)
                    {
                        model.BreadcrumbedPageTitleText = jobProfile.Title;
                    }
                    else
                    {
                        model.BreadcrumbedPageTitleText = currentPageNode.Title;
                    }
                } // If we are on Alerts page(s)
                else if (nodeUrl.ToLowerInvariant().Contains("alerts"))
                {
                    model.BreadcrumbedPageTitleText = "Error";
                } // Or we are on any other page
                else
                {
                    model.BreadcrumbedPageTitleText = currentPageNode.Title;
                }
            }
            else
            {
                model.BreadcrumbedPageTitleText = "Breadcrumb cannot establish the page node";
            }

            return View(model);
        }

        #endregion Actions
    }
}