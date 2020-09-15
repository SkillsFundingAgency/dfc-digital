using DFC.Digital.Web.Sitefinity.Core;
using System;
using System.Configuration;
using System.Web;
using Telerik.Sitefinity.Abstractions;
using Telerik.Sitefinity.Services;

namespace DFC.Digital.Web.Sitefinity
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            //Fix for ITHC Jquery version issue
            if (HttpContext.Current.Request.Url.AbsoluteUri.Contains("jquery-1.11.0.min.js"))
            {
                HttpContext.Current.Response.Redirect($"{ConfigurationManager.AppSettings["DFC.Digital.CDNLocation"]}/gds_service_toolkit/js/jquerybundle.min.js", true);
            }
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            foreach (string sCookie in Response.Cookies)
            {
                Response.Cookies[sCookie].Secure = true;
                Response.Cookies[sCookie].Path += ";HttpOnly";
            }
        }
    }
}
