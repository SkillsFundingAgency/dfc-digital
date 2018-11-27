using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Web.Core;
using DFC.Digital.Web.Sitefinity.Core;
using System;
using System.Web.Mvc;
using System.Web.UI.HtmlControls;
using Telerik.Sitefinity.Frontend.Mvc.Infrastructure;
using Telerik.Sitefinity.Mvc;

namespace DFC.Digital.Web.Sitefinity.Widgets.Mvc.Controllers
{
    /// <summary>
    /// Custom Widget for customising page titles
    /// </summary>
    /// <seealso cref="DFC.Digital.Web.Core.BaseDfcController" />
    [ControllerToolboxItem(Name = "SetCanonicalUrl", Title = "Set Canonical Url", SectionName = SitefinityConstants.CustomWidgetSection)]
    public class SetCanonicalUrlController : BaseDfcController
    {
        #region Private Fields

        private const string canonicalAttrKey = "rel";
        private const string canonicalAttrValue = "canonical";
        #endregion Private Fields

        #region Constructors

        public SetCanonicalUrlController(IApplicationLogger applicationLogger) : base(applicationLogger)
        {
        }

        #endregion Constructors

        #region Public Properties
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
            return PageHandlerResult();
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
            return PageHandlerResult();
        }

        #endregion Actions

        #region Private Methods

        private ActionResult PageHandlerResult()
        {
            var page = HttpContext.CurrentHandler.GetPageHandler();

            if (page != null)
            {
                page.PreRenderComplete += Page_PreRenderComplete;
            }

            return new EmptyResult();
        }

        private void Page_PreRenderComplete(object sender, EventArgs e)
        {
            var page = HttpContext.CurrentHandler.GetPageHandler();
            var link = new HtmlLink();
            link.Attributes.Add(canonicalAttrKey, canonicalAttrValue);
            link.Href = Request?.Url?.AbsoluteUri;

            if (!string.IsNullOrWhiteSpace(link.Href))
            {
                page.Header.Controls.Add(link);
            }
        }

        #endregion

    }
}