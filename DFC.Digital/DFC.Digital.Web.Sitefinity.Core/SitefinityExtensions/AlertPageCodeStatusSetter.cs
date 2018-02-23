using System.Web;
using System.Web.UI;
using Telerik.Sitefinity.Web;

namespace DFC.Digital.Web.Sitefinity.Core
{
    /// <summary>
    /// Alert Page status codes
    /// </summary>
    /// <seealso cref="System.Web.UI.Page" />
    public class AlertPageCodeStatusSetter : Page
    {
        /// <summary>
        /// Gets the actual current node.
        /// </summary>
        /// <value>
        /// The actual current node.
        /// </value>
        public static SiteMapNode ActualCurrentNode => SiteMapBase.GetActualCurrentNode();

        /// <summary>
        /// Initializes the <see cref="T:System.Web.UI.HtmlTextWriter" /> object and calls on the child controls of the <see cref="T:System.Web.UI.Page" /> to render.
        /// Setting status codes for the pages as setup on redirect
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter" /> that receives the page content.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);

            if (ActualCurrentNode != null && !this.IsDesignMode())
            {
                var urlname = ((PageSiteNode)ActualCurrentNode).UrlName;
                switch (urlname)
                {
                    case "403":
                        Response.Status = "403 Forbidden";
                        Response.StatusCode = 403;
                        break;

                    case "400":
                        Response.Status = "400 Bad Request";
                        Response.StatusCode = 400;
                        break;

                    case "404":
                        Response.Status = "404 Not found";
                        Response.StatusCode = 404;
                        break;

                    case "500":
                        Response.Status = "500 Internal Server Error";
                        Response.StatusCode = 500;
                        break;

                    case "503":
                        Response.Status = "503 Service Unavailable";
                        Response.StatusCode = 503;
                        break;
                }
            }
        }
    }
}