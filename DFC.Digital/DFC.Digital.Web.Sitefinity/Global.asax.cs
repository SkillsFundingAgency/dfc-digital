using System;
using System.Configuration;
using System.Web;
using Telerik.Sitefinity.Abstractions;
using Telerik.Sitefinity.DynamicModules.Events;
using Telerik.Sitefinity.Services;
using Telerik.Sitefinity.Utilities.TypeConverters;

namespace DFC.Digital.Web.Sitefinity
{
    public class Global : System.Web.HttpApplication
    {
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

        protected void Application_Start(object sender, EventArgs e)
        {
            Bootstrapper.Bootstrapped += Bootstrapper_Bootstrapped;
        }

        private void Bootstrapper_Bootstrapped(object sender, EventArgs e)
        {
            EventHub.Subscribe<IDynamicContentUpdatedEvent>(evt => DynamicContentUpdatedEventHandler(evt));
        }

        private void DynamicContentUpdatedEventHandler(IDynamicContentUpdatedEvent eventInfo)
        {
            var eventBase = eventInfo as DynamicContentEventBase;

            if (eventBase != null)
            {
                var dynamicType = TypeResolutionService.ResolveType("Telerik.Sitefinity.DynamicTypes.Model.JobProfile.JobProfile");

                if (eventBase.ItemType == dynamicType && eventBase.Status == "Master")
                {
                    var changedProperties = eventBase.ChangedProperties;

                    var changedPropertiesValue = string.Empty;

                    foreach (var property in changedProperties)
                    {
                        changedPropertiesValue += string.Format("|{0} - old: {1} | new: {2} |", property.Key, property.Value.OldValue, property.Value.NewValue);
                    }

                    Log.Write(string.Format("SF SUPPORT: Item with id {0} had the following properties changed: {1}", eventBase.ItemId, changedPropertiesValue));
                }
            }
        }
    }
}
