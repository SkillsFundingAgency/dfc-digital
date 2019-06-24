using System;
using Telerik.Sitefinity;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.Pages.Model;
using Telerik.Sitefinity.Workflow.Model.Tracking;

namespace DFC.Digital.Web.Sitefinity.Core
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class SitefinityPageNodeProxy : ISitefinityPageNodeProxy
    {
        public ApprovalTrackingRecord GetCurrentApprovalTrackingRecord(PageNode pageNode)
        {
            return pageNode.GetCurrentApprovalTrackingRecord();
        }

        public string GetCustomField(PageNode pageNode, string customFieldName)
        {
            var customField = pageNode.GetValue(customFieldName) as Lstring;
            return customField.Value;
        }

        public DateTime GetLastPublishedDate(PageNode pageNode)
        {
            //ToSitefinityUITime converts the time to what is displayed in the UI taking into account local time zones,
            //daylight saving etc
            return pageNode.GetCurrentApprovalTrackingRecord().DateCreated.ToSitefinityUITime();
        }

        public string GetURLName(PageNode pageNode)
        {
            return pageNode.UrlName.Value;
        }
    }
}