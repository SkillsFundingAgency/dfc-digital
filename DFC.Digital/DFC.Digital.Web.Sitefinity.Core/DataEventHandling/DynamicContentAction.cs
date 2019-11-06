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
        public DynamicContentEventAction GetDynamicContentEventAction(DynamicContent item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            if (item.Status.ToString() == Constants.ItemActionDeleted)
            {
                return DynamicContentEventAction.Deleted;
            }
            else if (item.ApprovalWorkflowState.Value == Constants.WorkflowStatusUnpublished && item.Status.ToString() == Constants.ItemStatusMaster)
            {
                //Unpublished
                return DynamicContentEventAction.Deleted;
            }
            else if (item.GetType().Name == Constants.SOCSkillsMatrix || item.GetType().Name == Constants.JobProfile)
            {
                if (item.ApprovalWorkflowState.Value == Constants.WorkflowStatusPublished && item.Visible && item.Status.ToString() == Constants.ItemStatusLive)
                {
                    return DynamicContentEventAction.Published;
                }
            }
            else if (item.ApprovalWorkflowState.Value == Constants.WorkflowStatusPublished && item.Status.ToString() == Constants.ItemStatusMaster)
            {
                return DynamicContentEventAction.Published;
            }

            return DynamicContentEventAction.Ignored;
        }
    }
}