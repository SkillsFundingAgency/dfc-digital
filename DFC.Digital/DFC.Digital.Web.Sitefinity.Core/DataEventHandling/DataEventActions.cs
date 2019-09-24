using DFC.Digital.Core;
using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Sitefinity.Data.Events;
using Telerik.Sitefinity.DynamicModules.Events;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Pages.Model;
using Telerik.Sitefinity.Utilities.TypeConverters;

namespace DFC.Digital.Web.Sitefinity.Core
{
    public class DataEventActions : IDataEventActions
    {
        private readonly ISitefinityDataEventProxy sitefinityDataEventProxy;

        public DataEventActions(ISitefinityDataEventProxy sitefinityDataEventProxy)
        {
            this.sitefinityDataEventProxy = sitefinityDataEventProxy;
        }

        public MicroServicesDataEventAction GetEventAction(IDataEvent eventInfo)
        {
            if (eventInfo == null)
            {
                throw new ArgumentNullException("eventInfo");
            }

            var itemAction = eventInfo.Action;
            var workFlowStatus = sitefinityDataEventProxy.GetPropertyValue<string>(eventInfo, Constants.ApprovalWorkflowState);
            var itemStatus = sitefinityDataEventProxy.GetPropertyValue<string>(eventInfo, Constants.ItemStatus);
            var recycleBinAction = sitefinityDataEventProxy.GetPropertyValue<RecycleBinAction>(eventInfo, Constants.RecycleBinAction);
            var itemActionData = eventInfo.Action;

            if (itemAction == Constants.ItemActionDeleted || (workFlowStatus == Constants.WorkflowStatusUnpublished && recycleBinAction != RecycleBinAction.RestoreFromRecycleBin))
            {
                //Unpublished or deleted Check for this first
                return MicroServicesDataEventAction.UnpublishedOrDeleted;
            }
            else if (workFlowStatus == Constants.WorkflowStatusPublished && itemStatus == Constants.ItemStatusLive)
            {
                //Published
                return MicroServicesDataEventAction.PublishedOrUpdated;
            }

            return MicroServicesDataEventAction.Ignored;
        }

        public MicroServicesDataEventAction GetDynamicContentEventAction(IDynamicContentUpdatedEvent eventInfo)
        {
            if (eventInfo == null)
            {
                throw new ArgumentNullException("eventInfo");
            }

            //var itemAction = eventInfo.Item.Status;
            var workFlowStatus = sitefinityDataEventProxy.GetDynamicContentPropertyValue<string>(eventInfo, Constants.ApprovalWorkflowState);
            var itemStatus = sitefinityDataEventProxy.GetDynamicContentPropertyValue<string>(eventInfo, Constants.ItemStatus);
            var recycleBinAction = sitefinityDataEventProxy.GetDynamicContentPropertyValue<RecycleBinAction>(eventInfo, Constants.RecycleBinAction);

            if (workFlowStatus == Constants.ItemActionDeleted || (workFlowStatus == Constants.WorkflowStatusUnpublished && recycleBinAction != RecycleBinAction.RestoreFromRecycleBin))
            {
                //Unpublished or deleted Check for this first
                return MicroServicesDataEventAction.UnpublishedOrDeleted;
            }
            else if (workFlowStatus == Constants.WorkflowStatusPublished && itemStatus == Constants.ItemStatusLive)
            {
                //Published
                return MicroServicesDataEventAction.PublishedOrUpdated;
            }

            return MicroServicesDataEventAction.Ignored;
        }

        public bool ShouldExportPage(IDataEvent dataEvent)
        {
            //Ignore any workflow property changes
            var changedProperties = sitefinityDataEventProxy.GetPropertyValue<IDictionary<string, PropertyChange>>(dataEvent, Constants.ChangedProperties);
            var filteredProperties = changedProperties.Where(p => p.Key != Constants.ApprovalWorkflowState).Count();
            var hasPageChanged = sitefinityDataEventProxy.GetPropertyValue<bool>(dataEvent, Constants.HasPageDataChanged);
            return hasPageChanged || filteredProperties > 0;
         }

        public bool ShouldExportDynamicModuleData(IDynamicContentUpdatedEvent dataEvent)
        {
            IDictionary<string, PropertyChange> changedProperties = sitefinityDataEventProxy.GetDynamicContentPropertyValue<IDictionary<string, PropertyChange>>(dataEvent, Constants.ChangedProperties);
            var filteredProperties = changedProperties.Where(p => p.Key != Constants.ApprovalWorkflowState).Count();
            return filteredProperties > 0;

            //var hasPageChanged = sitefinityDataEventProxy.GetDynamicContentPropertyValue<bool>(dataEvent, Constants.HasPageDataChanged);
            //return hasPageChanged || filteredProperties > 0;
        }
    }
}