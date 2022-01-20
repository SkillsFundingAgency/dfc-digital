﻿using System;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Telerik.Sitefinity.Abstractions;

namespace DFC.Digital.Web.Sitefinity
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            //Fix for ITHC Jquery version issue
            if (HttpContext.Current.Request.Url.AbsoluteUri.Contains("jquery-1.11.0.min.js"))
            {
                HttpContext.Current.Response.Redirect($"{ConfigurationManager.AppSettings["DFC.Digital.CDNLocation"]}/nationalcareers_toolkit/js/jquerybundle.min.js", true);
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

        protected void Application_Start(object sender, EventArgs e)
        {
            Bootstrapper.Bootstrapped += Bootstrapper_Bootstrapped;
        }

        private void Bootstrapper_Bootstrapped(object sender, EventArgs e)
        {
            System.Web.Mvc.RouteCollectionExtensions.MapRoute(RouteTable.Routes, "Classic", "mt/{controller}/{action}/{id}", new { controller = "MigrationTool", action = "Index", id = (string)null });
        }
    }
}
