using DFC.Digital.Core;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telerik.Sitefinity.DynamicModules.Model;

namespace DFC.Digital.Web.Sitefinity.Core
{
    public class DynamicContentAction : IDynamicContentAction
    {
        public MessageAction GetDynamicContentEventAction(DynamicContent item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            if (item.GetType().Name.ToString() == Constants.JobProfile && item.ApprovalWorkflowState.Value == Constants.WorkflowStatusPublished && item.Visible && item.Status.ToString() == Constants.ItemStatusLive)
            {
                return MessageAction.Published;
            }
            else if (item.GetType().Name.ToString() == Constants.JobProfile && item.ApprovalWorkflowState.Value == Constants.WorkflowStatusDraft && item.Status.ToString() == Constants.ItemStatusMaster)
            {
                return MessageAction.Draft;
            }
            else if (item.ApprovalWorkflowState.Value == Constants.WorkflowStatusPublished && !item.Visible && item.Status.ToString() == Constants.ItemStatusLive)
            {
                return MessageAction.Deleted;
            }
            else if (item.Status.ToString() == Constants.ItemActionDeleted)
            {
                if (item.GetType().Name == Constants.SOCSkillsMatrix || item.GetType().Name == Constants.JobProfile)
                {
                    return MessageAction.Ignored;
                }

                return MessageAction.Deleted;
            }
            else if (item.GetType().Name.ToString() != Constants.SOCSkillsMatrix && item.GetType().Name.ToString() != Constants.JobProfile && item.ApprovalWorkflowState.Value == Constants.WorkflowStatusUnpublished && item.Status.ToString() == Constants.ItemStatusMaster)
            {
                //Unpublished
                return MessageAction.Deleted;
            }
            else if (item.GetType().Name.ToString() != Constants.JobProfile && item.ApprovalWorkflowState.Value == Constants.WorkflowStatusPublished && item.Status.ToString() == Constants.ItemStatusMaster)
            {
                return MessageAction.Published;
            }

            return MessageAction.Ignored;
        }
    }
}