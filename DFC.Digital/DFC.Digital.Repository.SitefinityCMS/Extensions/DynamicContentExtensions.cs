using DFC.Digital.Core;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.GenericContent.Model;

namespace DFC.Digital.Repository.SitefinityCMS.Extensions
{
    public static class DynamicContentExtensions
    {
        public static string GetContentItemIdKey(this DynamicContent content)
        {
            return content.ApprovalWorkflowState == Constants.WorkflowStatusDraft ? Constants.ContentId : Constants.OriginalContentId;
        }

        public static string GetAdditionalContentItemIdKey(this DynamicContent content)
        {
            return content.Status == ContentLifecycleStatus.Live ? Constants.OriginalContentId : Constants.ContentId;
        }
    }
}