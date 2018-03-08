using DFC.Digital.Data.Interfaces;
using DFC.Digital.Web.Core.Base;
using DFC.Digital.Web.Sitefinity.Core.Utility;
using DFC.Digital.Web.Sitefinity.Widgets.Mvc.Models;
using System.ComponentModel;
using System.Web;
using System.Web.Mvc;
using Telerik.Sitefinity.Mvc;

namespace DFC.Digital.Web.Sitefinity.Widgets.Mvc.Controllers
{
    /// <summary>
    /// Custom Widget for customising page titles
    /// </summary>
    /// <seealso cref="DFC.Digital.Web.Core.Base.BaseDfcController" />
    [ControllerToolboxItem(Name = "SetContentRelatedPageTitle", Title = "Set Content Related Page Title", SectionName = SitefinityConstants.CustomWidgetSection)]
    public class SetContentRelatedPageTitleController : BaseDfcController
    {
        #region Private Fields

        private IJobProfileCategoryRepository categoryRepo;
        private IJobProfileRepository jobProfileRepository;
        private IWebAppContext webAppContext;

        #endregion Private Fields

        #region Constructors
        public SetContentRelatedPageTitleController(IJobProfileCategoryRepository categoryRepo, IJobProfileRepository jobProfileRepo, IWebAppContext webAppContext, IApplicationLogger applicationLogger) : base(applicationLogger)
        {
            this.categoryRepo = categoryRepo;
            this.jobProfileRepository = jobProfileRepo;
            this.webAppContext = webAppContext;
        }

        #endregion Constructors
        #region Public Properties

        /// <summary>
        /// Gets or sets the suffix used for page title.
        /// </summary>
        /// <value>
        /// The Suffix for page title.
        /// </value>
        [DisplayName("Page Title Suffix (e.g. Find a career )")]
        public string PageTitleSuffix { get; set; } = "Find a career ";

        [DisplayName("Title Seperator (e.g. |)")]
        public string PageTitleSeparator { get; set; } = "|";

        #endregion Public Properties

        #region Actions

        // GET: for CustomisePageTitle

        /// <summary>
        /// entry point to the widget to customise page title.
        /// </summary>
        /// <returns>result</returns>
        [HttpGet]
        [RelativeRoute("")]
        public ActionResult Index()
        {
            if (!webAppContext.IsContentAuthoringSite && webAppContext.IsSearchResultsPage)
            {
                var searchTerm = HttpUtility.HtmlEncode(webAppContext.RequestQueryString["searchTerm"]);
                this.ViewBag.Title = $"{searchTerm} {PageTitleSeparator} Search {PageTitleSeparator} {PageTitleSuffix}";
            }

            //Need to return a blank view so that we can check the view bag in tests.
            return View(new SetContentRelatedPageTitleModel { DisplayView = webAppContext.IsContentAuthoringAndNotPreviewMode });
        }

        /// <summary>
        /// CustomisePageTitle on Index of the specified UrlName.
        /// </summary>
        /// <param name="urlName">The UrlName.</param>
        /// <returns>result</returns>
        [HttpGet]
        [RelativeRoute("{urlName}")]
        public ActionResult Index(string urlName)
        {
            if (webAppContext.IsCategoryPage && !string.IsNullOrEmpty(urlName))
            {
                var category = categoryRepo.GetByUrlName(urlName);
                if (category != null)
                {
                    this.ViewBag.Title = $"{category.Title} {PageTitleSeparator} {PageTitleSuffix}";
                }
            }
            else if (webAppContext.IsJobProfilePage && !string.IsNullOrEmpty(urlName))
            {
                var jobProfile = jobProfileRepository.GetByUrlName(urlName);
                if (jobProfile != null)
                {
                    this.ViewBag.Title = $"{jobProfile.Title} {PageTitleSeparator} {PageTitleSuffix}";
                }
            }

            //Need to return a blank view so that we can check the view bag in tests.
            return View(new SetContentRelatedPageTitleModel { DisplayView = webAppContext.IsContentAuthoringAndNotPreviewMode });
        }

        #endregion Actions
    }
}