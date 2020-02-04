using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace DFC.Digital.Web.Sitefinity
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (HttpContext.Current.Request.Url.AbsoluteUri.Contains("jquery-1.11.0.min.js"))
            {
                HttpContext.Current.Response.Redirect($"{ConfigurationManager.AppSettings["DFC.Digital.CDNLocation"]}/gds_service_toolkit/js/jquerybundle.min.js", true);
            }
        }
    }
}
