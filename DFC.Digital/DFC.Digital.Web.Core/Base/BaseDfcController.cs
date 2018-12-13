using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using System;
using System.Web.Mvc;
using System.Web.UI.HtmlControls;
using Telerik.Sitefinity.Frontend.Mvc.Infrastructure;

namespace DFC.Digital.Web.Core
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dfc", Justification = "Reviewed. Project name in correct spelling")]
    public class BaseDfcController : Controller
    {
        #region Private Fields

        private const string CanonicalAttrKey = "rel";
        private const string CanonicalAttrValue = "canonical";
        #endregion Private Fields

        public BaseDfcController(IApplicationLogger loggingService)
        {
            this.Log = loggingService;
        }

        protected IApplicationLogger Log { get; private set; }

        public void AddCanonicalTag()
        {
            var page = HttpContext.CurrentHandler.GetPageHandler();
            if (page != null)
            {
                var link = new HtmlLink();
                link.Attributes.Add(CanonicalAttrKey, CanonicalAttrValue);
                link.Href = Request?.Url?.AbsoluteUri;

                if (!string.IsNullOrWhiteSpace(link.Href))
                {
                    page.Header.Controls.Add(link);
                }
            }
        }

        public void RemoveCanonicalTag()
        {
            var page = HttpContext.CurrentHandler.GetPageHandler();
            if (page != null)
            {
                var headerControls = page.Header.Controls;
                foreach (var control in headerControls)
                {
                    if (control is HtmlLink link)
                    {
                        if (link.Attributes[CanonicalAttrKey] == CanonicalAttrValue)
                        {
                            headerControls.Remove(link);
                            break;
                        }
                    }
                }
            }
        }

        public void SetupPreRenderEventHandler()
        {
            var page = HttpContext.CurrentHandler.GetPageHandler();

            if (page != null)
            {
                page.PreRenderComplete += Page_PreRenderComplete;
            }
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            var ex = filterContext?.Exception;

            //Logging the Exception
            Log.Error($"Controller '{Convert.ToString(filterContext?.RouteData.Values["controller"])}' Action : '{Convert.ToString(filterContext?.RouteData.Values["action"])}' - failed with exception.", ex);
        }

        private void Page_PreRenderComplete(object sender, EventArgs e)
        {
            RemoveCanonicalTag();
            AddCanonicalTag();
        }
    }
}