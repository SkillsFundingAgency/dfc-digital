using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace DFC.Digital.Web.Core
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class CustomHeaderModule : IHttpModule
    {
        private const string MetaAttrKey = "name";
        private const string MetaAttrValue = "Generator";

        public void Init(HttpApplication context)
        {
            if (context != null)
            {
                context.PreSendRequestHeaders += OnPreSendRequestHeaders;
            }
        }

        public void Dispose()
        {
        }

        private void OnPreSendRequestHeaders(object sender, EventArgs e)
        {
            HttpContext.Current?.Response.Headers.Remove("Server");
            HttpContext oContext = ((HttpApplication)sender).Context;
            if (oContext.Handler is Page)
            {
                Page objPage = (Page)oContext.Handler;
                objPage.PreRender += new EventHandler(ObjPage_PreRender);
            }

            // Or you can set something misleading
            //HttpContext.Current.Response.Headers.Set("Server", "NCS Server");
        }

        private void ObjPage_PreRender(object sender, EventArgs e)
        {
            HttpContext oContext = ((HttpApplication)sender).Context; // HttpContext.Current.CurrentHandler as Page;

            //throw new NotImplementedException();
            if (oContext.Handler is Page page)
            {
                var headerControls = page.Header.Controls;
                foreach (var control in headerControls)
                {
                    if (control is HtmlMeta metaTag)
                    {
                        if (metaTag.Attributes[MetaAttrKey] == MetaAttrValue)
                        {
                            headerControls.Remove(metaTag);
                            break;
                        }
                    }
                }
            }
        }
    }
}