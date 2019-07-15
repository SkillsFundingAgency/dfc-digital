using DFC.Digital.Core;
using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Sitefinity.Data.Events;
using Telerik.Sitefinity.Pages.Model;

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

            if (itemAction == Constants.ItemActionDeleted || (workFlowStatus == Constants.WorkFlowStatusUnPublished && recycleBinAction != RecycleBinAction.RestoreFromRecycleBin))
            {
                //Unpublished or deleted Check for this first
                return MicroServicesDataEventAction.UnpublishedOrDeleted;
            }
            else if (workFlowStatus == Constants.WorkFlowStatusPublished && itemStatus == Constants.ItemStatusLive)
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
    }
}