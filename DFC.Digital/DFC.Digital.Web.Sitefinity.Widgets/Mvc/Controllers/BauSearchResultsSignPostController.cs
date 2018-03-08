using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Web.Core.Base;
using DFC.Digital.Web.Sitefinity.Core.Utility;
using DFC.Digital.Web.Sitefinity.Widgets.Mvc.Models;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Telerik.Sitefinity.Mvc;

namespace DFC.Digital.Web.Sitefinity.Widgets.Mvc.Controllers
{
    /// <summary>
    /// Custom Widget for VOC Survey email Capture
    /// </summary>
    /// <seealso cref="BaseDfcController" />
    [ControllerToolboxItem(Name = "BauSearchResultsSignPost", Title = "Bau Search Results SignPost", SectionName = SitefinityConstants.CustomWidgetSection)]
    public class BauSearchResultsSignpostController : BaseDfcController
    {
        #region Constructors
        public BauSearchResultsSignpostController(IApplicationLogger applicationLogger) : base(applicationLogger)
        {
        }

        #endregion

        #region Public Properties

        [DisplayName("Banner Content (HTML) - {0} will be replaced by search term if there")]
        public string BannerContent { get; set; } = "<a class=\"signpost\" href=\"https://dev.nationalcareersservice.org.uk/job-profiles/search-results?indexCatalogue=job-profiles&amp;searchQuery={0}&amp;wordsMode=AllWords\"><p class=\"signpost_arrow\"><span>Back to the National Careers Service</span> where you'll find all the job profiles</p></a>";
        #endregion
        #region Actions

        [HttpGet]
        [RelativeRoute("")]
        public ActionResult Index(string searchTerm)
        {
            return View("Index", new BauSearchResultsViewModel { Content = BannerContent.Replace("{0}", !string.IsNullOrWhiteSpace(searchTerm) ? StripInvalidCharsAndEncode(searchTerm) : string.Empty) });
        }

        private static string StripInvalidCharsAndEncode(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return string.Empty;
            }

            searchTerm = Regex.Replace(searchTerm, Constants.ValidBAUSearchCharacters, string.Empty);

            return HttpUtility.UrlEncode(searchTerm);
        }
        #endregion

    }
}