﻿using DFC.Digital.Data.Model;
using Microsoft.Azure.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Sitefinity.DynamicModules.Model;

namespace DFC.Digital.Web.Sitefinity.Core
{
    public interface IServiceBusMessageProcessor
    {
        Task SendContentPageMessage(MicroServicesPublishingPageData contentPageData, string contentType, string eventAction);

        Task SendJobProfileMessage(JobProfileMessage dynamicContent, string itemType, string eventAction);

        Task SendOtherRelatedTypeMessages(IEnumerable<RelatedContentItem> relatedContentItems, string contentType, string actionType);
    }
}
