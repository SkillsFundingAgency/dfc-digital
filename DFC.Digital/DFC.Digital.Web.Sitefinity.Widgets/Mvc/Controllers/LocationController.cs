using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Core;
using DFC.Digital.Web.Sitefinity.Core;
using DFC.Digital.Web.Sitefinity.Widgets.Mvc.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Telerik.Sitefinity.Mvc;
using Telerik.Sitefinity.Personalization.Impl.GeoData;
using Telerik.Sitefinity.Personalization.Impl.Web.Services.ViewModel;

namespace DFC.Digital.Web.Sitefinity.Widgets.Mvc.Controllers
{
    /// <summary>
    /// Custom Widget for having Breadcrumb
    /// </summary>
    /// <seealso cref="DFC.Digital.Web.Core.BaseDfcController" />
    [ControllerToolboxItem(Name = "Location", Title = "Location", SectionName = SitefinityConstants.CustomWidgetSection)]
    public class LocationController : BaseDfcController
    {
        #region Private Fields

        private IJobProfileCategoryRepository categoryRepo;
        private IJobProfileRepository jobProfileRepository;
        private ISitefinityCurrentContext sitefinityCurrentContext;

        #endregion Private Fields

        #region Constructors

        public LocationController(IJobProfileCategoryRepository categoryRepo, IJobProfileRepository jobProfileRepo, ISitefinityCurrentContext sitefinityCurrentContext, IApplicationLogger applicationLogger) : base(applicationLogger)
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
            return GetLocationView(string.Empty);
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
            return GetLocationView(urlName);
        }

        /// <summary>
        /// Gets the job profile details view.
        /// </summary>
        /// <param name="urlName">The urlname.</param>
        /// <returns>ActionResult</returns>
        private ActionResult GetLocationView(string urlName)
        {
            var model = new Models.LocationViewModel();
            model = FindLocation();
            return View(FindLocation());
        }

        private Models.LocationViewModel FindLocation()
        {
            var ip = HttpRequestUtilities.GetIpAddress(HttpContext.Request);
            LookupService lookupService = new LookupService();
            var model = new Models.LocationViewModel();
            model.IPAddress = ip.ToString();
            if (lookupService.GeoIpStream != null)
            {
                var location = lookupService.getLocation(ip);
                model.Country = location == null ? null : location.countryName;
                model.CountryCode = location == null ? null : location.countryCode;
                model.Region = location == null ? null : location.city;
            }

            return model;
        }

        #endregion Actions
    }
}