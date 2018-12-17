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
using Telerik.Sitefinity.Pages.Model;
using Telerik.Sitefinity.Web;

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
        [DisplayName("Home page Text")]
        public string HomepageText { get; set; } = "Explore careers home";

        [DisplayName("Job Categories URL Segment")]
        public string JobCategoriesURLSegment { get; set; } = "JOB-CATEGORIES";

        [DisplayName("Job Profiles URL Segment")]
        public string JobProfilesURLSegment { get; set; } = "JOB-PROFILES";

        [DisplayName("Alerts URL Segment")]
        public string AlertsURLSegment { get; set; } = "ALERTS";

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
            var breadCrumbLink = new BreadCrumbLink();
            var model = new DfcBreadcrumbViewModel
            {
                HomepageText = HomepageText,
                HomepageLink = HomepageLink,
                BreadcrumbLinks = new List<BreadCrumbLink>
                {
                    breadCrumbLink
                }
            };

            // We get the page node we are on
            var currentPageNode = sitefinityCurrentContext.GetCurrentDfcPageNode();
            if (currentPageNode != null)
            {
                string nodeUrl = currentPageNode.Url.OriginalString;

                // If we are on JobCategories page(s)
                if (nodeUrl.ToUpperInvariant().Contains(JobCategoriesURLSegment) && !string.IsNullOrEmpty(urlName))
                {
                    var category = categoryRepo.GetByUrlName(urlName);
                    breadCrumbLink.Text = (category == null) ? currentPageNode.Title : category.Title;
                } // If we are on JobProfileDetalails page(s)
                else if (nodeUrl.ToUpperInvariant().Contains(JobProfilesURLSegment) && !string.IsNullOrEmpty(urlName))
                {
                    var jobProfile = jobProfileRepository.GetByUrlName(urlName);
                    breadCrumbLink.Text = (jobProfile == null) ? currentPageNode.Title : jobProfile.Title;
                } // If we are on Alerts page(s)
                else if (nodeUrl.ToUpperInvariant().Contains(AlertsURLSegment))
                {
                    breadCrumbLink.Text = "Error";
                } // Or we are on any other page
                else
                {
                    model.BreadcrumbLinks.Clear();
                    var pageNode = SiteMapBase.GetActualCurrentNode();
                    while (pageNode.ParentNode != null)
                    {
                        if (pageNode.NodeType == NodeType.Standard && pageNode.Visible)
                        {
                            var pageBreadCrumbLink = new BreadCrumbLink
                            {
                                Text = pageNode.Title,
                                Link = pageNode.Url
                            };
                            model.BreadcrumbLinks.Add(pageBreadCrumbLink);
                        }

                        pageNode = pageNode.ParentNode as PageSiteNode;
                    }

                    //the current page should not be linked
                    model.BreadcrumbLinks.FirstOrDefault().Link = string.Empty;
                    model.BreadcrumbLinks = model.BreadcrumbLinks.Reverse().ToList();
                }
            }

            //Test to see is it behaves as expected if we dont do this GSR
            // else
            // {
            //     breadCrumbLink.Text = string.Empty;
            // }
            return View(model);
        }

        #endregion Actions
    }
}