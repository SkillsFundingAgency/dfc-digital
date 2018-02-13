﻿using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Configuration;
using System.Web.Hosting;
using Telerik.Sitefinity.Web;

namespace DFC.Digital.Web.Sitefinity.Core.HttpModules
{
    public class AppStatusSitefinityHttpModule : SitefinityHttpModule
    {
        private static readonly string SystemRestartingHtml = File.ReadAllText(HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["WillBeBackSoonPage"]));

        private static readonly string SystemUpgradingHtml = File.ReadAllText(HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["WillBeBackSoonPage"]));

        private static readonly string RedirectToUrlScript = File.ReadAllText(HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["WillBeBackSoonJScript"]));

        protected override void OnSystemRestarting(HttpContext context, string html = null, string scriptUrl = null)
        {
            CustomErrorsSection customErrorsSection = (CustomErrorsSection)ConfigurationManager.GetSection("system.web/customErrors");

            if (customErrorsSection.Mode.Equals(CustomErrorsMode.Off) || (customErrorsSection.Mode.Equals(CustomErrorsMode.RemoteOnly) && HttpContext.Current.Request.IsLocal))
            {
                base.OnSystemRestarting(context, html, scriptUrl);
            }
            else
            {
                DisplayPage(context, SystemRestartingHtml);
            }
        }

        protected override void OnSystemUpgrading(HttpContext context, string html = null, string scriptUrl = null)
        {
            CustomErrorsSection customErrorsSection = (CustomErrorsSection)ConfigurationManager.GetSection("system.web/customErrors");

            if (customErrorsSection.Mode.Equals(CustomErrorsMode.Off) || (customErrorsSection.Mode.Equals(CustomErrorsMode.RemoteOnly) && HttpContext.Current.Request.IsLocal))
            {
                base.OnSystemUpgrading(context, html, scriptUrl);
            }
            else
            {
                DisplayPage(context, SystemUpgradingHtml);
            }
        }

        private static void DisplayPage(HttpContext context, string html)
        {
            var statuscodeSetting = ConfigurationManager.AppSettings["sf:AppStatusPageResponseCode"];
            int statuscode;
            if (!int.TryParse(statuscodeSetting, out statuscode))
            {
                statuscode = 200;
            }

            var applicationVirtualPath = HostingEnvironment.ApplicationVirtualPath.TrimEnd('/') ?? string.Empty;

            context.Response.AddHeader("Content-Type", "text/html; charset=" + context.Response.Charset);
            context.Response.StatusCode = statuscode;
            context.Response.Write(html.Replace("_applicat_virtual_path_", applicationVirtualPath).Replace("_inject_javascript_", RedirectToUrlScript));
            context.ApplicationInstance.CompleteRequest();
        }
    }
}