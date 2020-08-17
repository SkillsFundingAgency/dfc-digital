using DFC.Digital.Core;
using Telerik.Sitefinity.DynamicModules.Model;

namespace DFC.Digital.Repository.SitefinityCMS.Extensions
{
    public static class DynamicContentExtensions
    {
        public static string GetContentItemIdKey(this DynamicContent content)
        {
            return content.ApprovalWorkflowState == Constants.WorkflowStatusDraft ? Constants.ContentId : Constants.OriginalContentId;
        }
    }
}